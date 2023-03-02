using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SoulsLike
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimationManager;
        EnemyStats enemyStats;
        public NavMeshAgent navmeshAgent;

        public bool isPerformingAction;
        public bool isInteracting;
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 1.5f;

        public State currentState;
        public CharacterStats currentTarget;
        public Rigidbody enemyRigidbody;

        [Header("Combat Flags")]
        public bool canDoCombo;

        [Header("A.I Settings")]
        public float detectionRadius = 20;
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;

        public float currentRecoveryTime = 0;

        [Header("A.I Combat Settings")]
        public bool allowAIToPerformCombos;
        public bool isPhaseShifting;
        public float comboLikelyHood;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimationManager = GetComponentInChildren<EnemyAnimatorManager>();
            navmeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyStats = GetComponent<EnemyStats>();
            enemyRigidbody = GetComponent<Rigidbody>();
            navmeshAgent.enabled = false;
        }

        private void Start()
        {
            enemyRigidbody.isKinematic = false;
        }

        private void Update()
        {
            HandleRecoveryTime();
            //if (canBeRiposted) return;
            HandleStateMachine();

            isRotatingWithRootMotion = enemyAnimationManager.anim.GetBool("isRotatingWithRootMotion");
            isInteracting = enemyAnimationManager.anim.GetBool("isInteracting");
            isPhaseShifting = enemyAnimationManager.anim.GetBool("isPhaseShifting");
            isInvulnerable = enemyAnimationManager.anim.GetBool("isInvulnerable");
            canDoCombo = enemyAnimationManager.anim.GetBool("canDoCombo");
            canRotate = enemyAnimationManager.anim.GetBool("canRotate");
            enemyAnimationManager.anim.SetBool("isDead", enemyStats.isDead);
        }

        private void LateUpdate()
        {
            navmeshAgent.transform.localPosition = Vector3.zero;
            navmeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleStateMachine()
        {
            if(currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimationManager);

                if(nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        

        private void HandleRecoveryTime()
        {
            if(currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if(currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }

        
    }
}
