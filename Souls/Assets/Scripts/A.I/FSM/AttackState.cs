using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class AttackState : State
    {
        public EnemyAttackAction[] enemyAttacks;
        public EnemyAttackAction currentAttack;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimationManager)
        {
            //select one of our attacks based on scores
            //if the selected attack is not able to be used select a new one
            //if the attack is viable stop movement and attack
            //set recovery timer
            //return the combat stance
            return this;
        }
    }
}
