using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public EnemyAttackAction[] enemyAttacks;
        public PersueTargetState persueTargetState;

        protected bool randomDestinationSet = false;
        protected float verticalMovementValue = 0;
        protected float horizontalMovementValue = 0;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimationManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            enemyAnimationManager.anim.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemyAnimationManager.anim.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            attackState.hasPerformedAttack = false;

            if (enemyManager.isInteracting)
            {
                enemyAnimationManager.anim.SetFloat("Vertical", 0);
                enemyAnimationManager.anim.SetFloat("Horizontal", 0);
                return this;
            }

            if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return persueTargetState;
            }

            if (!randomDestinationSet)
            {
                randomDestinationSet = true;
                DecideCirclingAction(enemyAnimationManager);
            }

            HandleRotateTowardsTarget(enemyManager);

            if(enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                randomDestinationSet = false;
                return attackState;
            }
            else
            {
                GetNewAttack(enemyManager);
            }
            return this;
        }

        protected void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            //rotate manually
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = enemyManager.transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.rotationSpeed);
            }
            //rotate with pathfining (navmesh)
            else
            {
                Vector3 relativeDirection = enemyManager.transform.InverseTransformDirection(enemyManager.navmeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.enemyRigidbody.velocity;

                enemyManager.navmeshAgent.enabled = true;
                enemyManager.navmeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.enemyRigidbody.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navmeshAgent.transform.rotation, enemyManager.rotationSpeed / Time.deltaTime);
            }
        }

        protected void DecideCirclingAction(EnemyAnimatorManager enemyAnimatorManager)
        {
            //Circle with only forward vertical movement
            //circle with running
            WalkAroundTarget(enemyAnimatorManager);
        }

        protected void WalkAroundTarget(EnemyAnimatorManager enemyAnimatorManager)
        {
            //verticalMovementValue = Random.Range(0, 1); //we only want forward motion (no negative value) so we walk towards target.
            verticalMovementValue = 0.5f;
            //if(verticalMovementValue <=1 && verticalMovementValue > 0)
            //{
            //    verticalMovementValue = 0.5f;
            //}
            //else if(verticalMovementValue >= -1 && verticalMovementValue < 0)
            //{
            //    verticalMovementValue = -0.5f;
            //}

            horizontalMovementValue = Random.Range(-1, 1);

            if(horizontalMovementValue <=1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if(horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }
        }

        #region Attacks
        protected virtual void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, enemyManager.transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximimDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.minimuimDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximimDistanceNeededToAttack && distanceFromTarget >= enemyAttackAction.minimuimDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null)
                        {
                            return;
                        }

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
