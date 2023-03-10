using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    [CreateAssetMenu(menuName = "Spells/Projectile Spell")]
    public class ProjectileSpell : SpellItem
    {
        [Header("Projectile Damage")]
        public float baseDamage;

        [Header("Projectile Physics")]
        public float projectileForwardVelocity;
        public float projectileUpwardsVelocity;
        public float projectileMass;
        public bool isEffectedByGravity;
        Rigidbody rigidBody;

        public override void AttemptToCastSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, PlayerWeaponSlotManager weaponSlotManager)
        {
            base.AttemptToCastSpell(animatorHandler, playerStats, weaponSlotManager);
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, weaponSlotManager.rightHandSlot.transform);
            //instantiatedWarmUpSpellFX.gameObject.transform.localScale = new Vector3(100, 100, 100);
            animatorHandler.PlayTargetAnimation(spellAnimation, true);
        }

        public override void SuccessfullyCastSpell(PlayerAnimatorManager animatorHandler, PlayerStatsManager playerStats, CameraHandler cameraHandler, PlayerWeaponSlotManager weaponSlotManager)
        {
            base.SuccessfullyCastSpell(animatorHandler, playerStats, cameraHandler, weaponSlotManager);

            GameObject instantiatedSpellFX = Instantiate(spellCastFX, weaponSlotManager.rightHandSlot.transform.position, cameraHandler.cameraPivotTransform.rotation);
            rigidBody = instantiatedSpellFX.GetComponent<Rigidbody>();
            //spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();

            if(cameraHandler.currentLockOnTarget != null)
            {
                instantiatedSpellFX.transform.LookAt(cameraHandler.currentLockOnTarget.transform);
            }
            else
            {
                instantiatedSpellFX.transform.rotation = Quaternion.Euler(cameraHandler.cameraPivotTransform.eulerAngles.x, playerStats.transform.eulerAngles.y, 0);
            }

            rigidBody.AddForce(instantiatedSpellFX.transform.forward * projectileForwardVelocity);
            rigidBody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardsVelocity);
            rigidBody.useGravity = isEffectedByGravity;
            rigidBody.mass = projectileMass;
            instantiatedSpellFX.transform.parent = null;
        }
    }
}
