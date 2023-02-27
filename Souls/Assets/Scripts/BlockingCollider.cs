using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class BlockingCollider : MonoBehaviour
    {
        public BoxCollider blockingCollider;

        public float blockingPhysicalDamageAbsorption;

        public void Awake()
        {
            blockingCollider = GetComponent<BoxCollider>();
        }

        public void SetColliderDamageAbsorption(WeaponItem weapon)
        {
            if (weapon != null)
            {
                blockingPhysicalDamageAbsorption = weapon.physicalDamageAbsorption;
            }
        }

        public void EnableBlockingCollider()
        {
            blockingCollider.enabled = true;
        }

        public void DisableBlockingCollider()
        {
            blockingCollider.enabled = false;
        }
    }
}
