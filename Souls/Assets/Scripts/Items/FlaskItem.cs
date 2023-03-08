using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    [CreateAssetMenu(menuName = "Items/Consumables/Flask")]
    public class FlaskItem : ConsumableItem
    {
        [Header("Flash Type")]
        public bool estusFlask;
        public bool ashenFlash;

        [Header("Recovery Amount")]
        public int healthRecoveryAmount;
        public int focusPointRecoveryAmount;

        [Header("Recovery FX")]
        public GameObject recoveryFX;

        public override void AttemptToConsumeItem(PlayerAnimatorManager playerAnimatorManager, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
        {
            base.AttemptToConsumeItem(playerAnimatorManager, weaponSlotManager, playerEffectsManager);
            GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
            playerEffectsManager.currentParticleFX = recoveryFX;
            playerEffectsManager.amountToBeHealed = healthRecoveryAmount;
            playerEffectsManager.instantiatedFXModel = flask;
            weaponSlotManager.rightHandSlot.UnloadWeapon();
        }
    }
}
