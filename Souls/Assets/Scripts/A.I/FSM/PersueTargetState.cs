using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class PersueTargetState : State
    {
        public CombatStanceState combatStanceState;
        public RotateTowardsTargetState rotateTowardsTargetState;
        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStats, EnemyAnimatorManager enemyAnimationManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

            HandleRotateTowardsTarget(enemyManager);

            if (enemyManager.isInteracting)
            {
                return this;
            }

            if (enemyManager.isPerformingAction)
            {
                enemyAnimationManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                enemyAnimationManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            if(distanceFromTarget <= enemyManager.maximumAggroRadius)
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }
        }

        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
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
    }
}
