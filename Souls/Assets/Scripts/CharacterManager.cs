using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        [Header("Combat Colliders")]
        public BoxCollider backStabBoxTrigger;
        public BackStabCollider backStabCollider;

        //damage will be inflicted during an animation event
        //used in backstab or riposite animations
        public int pendingCriticalDamage;
    }
}
