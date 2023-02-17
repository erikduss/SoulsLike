using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public abstract class State : MonoBehaviour
    {
        public abstract State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimationManager);
    }
}