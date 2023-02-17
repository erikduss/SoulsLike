using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class CombatStanceState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimationManager)
        {
            //check for attack range
            //potentially circle player or walka round them
            //if in attack range return attack state
            return this;
        }
    }
}
