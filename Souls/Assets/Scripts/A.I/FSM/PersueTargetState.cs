using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class PersueTargetState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimationManager)
        {
            //chase the target
            //if within attack range, switch to combat stance state
            return this;
        }
    }
}
