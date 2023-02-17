using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    [CreateAssetMenu(menuName = "A.I/Enemy Actions/Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
        public int attackScore = 3;
        public float recoveryTime = 2;

        public float maximumAttackAngle = 35;
        public float minimumAttackAngle = -35;

        public float minimuimDistanceNeededToAttack = 0;
        public float maximimDistanceNeededToAttack = 3;
    }
}
