using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class PlayerCombatManager : MonoBehaviour
    {
        CameraHandler cameraHandler;
        PlayerAnimatorManager playerAnimatorManager;
        PlayerEquipmentManager playerEquipmentManager;
        PlayerManager playerManager;
        PlayerStatsManager playerStatsManager;
        PlayerInventoryManager playerInventoryManager;
        InputHandler inputHandler;
        PlayerWeaponSlotManager playerWeaponSlotManager;
        public string lastAttack;

        LayerMask backStabLayer = 1 << 14;
        LayerMask riposteLayer = 1 << 15;
        private void Awake()
        {
            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerManager = GetComponent<PlayerManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
            inputHandler = GetComponent<InputHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }

            if (inputHandler.comboFlag)
            {
                playerAnimatorManager.anim.SetBool("canDoCombo", false);

                if (lastAttack == weapon.OH_Light_Attack_01)
                {
                    playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_02, true);
                }
                else if(lastAttack == weapon.th_light_attack_01)
                {
                    playerAnimatorManager.PlayTargetAnimation(weapon.th_light_attack_02, true);
                }
                else if(lastAttack == weapon.OH_Heavy_Attack_01)
                {
                    playerAnimatorManager.PlayTargetAnimation(weapon.OH_Heavy_Attack_02, true);
                }
                else if(lastAttack == weapon.th_Heavy_attack_01)
                {
                    playerAnimatorManager.PlayTargetAnimation(weapon.th_Heavy_attack_02, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }

            playerWeaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.th_light_attack_01, true);
                lastAttack = weapon.th_light_attack_01;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.OH_Light_Attack_01, true);
                lastAttack = weapon.OH_Light_Attack_01;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }

            playerWeaponSlotManager.attackingWeapon = weapon;

            if (inputHandler.twoHandFlag)
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.th_Heavy_attack_01, true);
                lastAttack = weapon.th_Heavy_attack_01;
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(weapon.OH_Heavy_Attack_01, true);
                lastAttack = weapon.OH_Heavy_Attack_01;
            }
        }

        #region Input Actions
        public void HandleRBAction()
        {
            if (playerInventoryManager.rightWeapon.isMeleeWeapon)
            {
                PerformRBMeleeAction();
            }
            else if (playerInventoryManager.rightWeapon.isSpellCaster || playerInventoryManager.rightWeapon.isFaithCaster || playerInventoryManager.rightWeapon.isPyroCaster)
            {
                PerformRBMagicAction(playerInventoryManager.rightWeapon);
            }
        }

        public void HandleRTAction()
        {
            if (playerInventoryManager.rightWeapon.isMeleeWeapon)
            {
                PerformRTMeleeAction();
            }
            else if (playerInventoryManager.rightWeapon.isSpellCaster || playerInventoryManager.rightWeapon.isFaithCaster || playerInventoryManager.rightWeapon.isPyroCaster)
            {
                PerformRBMagicAction(playerInventoryManager.rightWeapon);
            }
        }

        public void HandleLBAction()
        {
            PerformLBBlockingAction();
        }

        public void HandleLTAction()
        {
            if (playerInventoryManager.leftWeapon.isShieldWeapon)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);
                //perform shield weapon art
            }
            else if (playerInventoryManager.leftWeapon.isMeleeWeapon)
            {
                //do light attack
            }
        }
        #endregion

        #region Attack Actions
        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventoryManager.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                {
                    return;
                }
                if (playerManager.canDoCombo)
                {
                    return;
                }
                playerAnimatorManager.anim.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventoryManager.rightWeapon);
            }
        }

        private void PerformRTMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventoryManager.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else
            {
                if (playerManager.isInteracting)
                {
                    return;
                }
                if (playerManager.canDoCombo)
                {
                    return;
                }
                playerAnimatorManager.anim.SetBool("isUsingRightHand", true);
                HandleHeavyAttack(playerInventoryManager.rightWeapon);
            }
        }

        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (playerManager.isInteracting)
            {
                return;
            }

            if (weapon.isFaithCaster)
            {
                if(playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isFaithSpell)
                {
                    if(playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                    {
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
                    }
                    else
                    {
                        //stun animation
                    }
                }
            }
            else if (weapon.isPyroCaster)
            {
                if (playerInventoryManager.currentSpell != null && playerInventoryManager.currentSpell.isPyroSpell)
                {
                    if (playerStatsManager.currentFocusPoints >= playerInventoryManager.currentSpell.focusPointCost)
                    {
                        playerInventoryManager.currentSpell.AttemptToCastSpell(playerAnimatorManager, playerStatsManager, playerWeaponSlotManager);
                    }
                    else
                    {
                        //stun animation
                    }
                }
            }
        }

        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting) return;

            if (isTwoHanding)
            {
                //if we are 2handing perform weapon art for the right weapon
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation(playerInventoryManager.leftWeapon.weapon_Art, true);
            }
        }

        private void SuccessfullyCastSpell()
        {
            playerInventoryManager.currentSpell.SuccessfullyCastSpell(playerAnimatorManager, playerStatsManager, cameraHandler, playerWeaponSlotManager);
            playerAnimatorManager.anim.SetBool("isFiringSpell", true);
        }
        #endregion

        #region Defense Actions
        private void PerformLBBlockingAction()
        {
            if (playerManager.isInteracting)
            {
                return;
            }

            if (playerManager.isBlocking) return;

            playerAnimatorManager.PlayTargetAnimation("Block_Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }
        #endregion

        public void AttemptBackStabOrRiposte()
        {
            if (playerStatsManager.currentStamina <= 0)
            {
                return;
            }

            RaycastHit hit;

            if(Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                //Check for team ID
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

                if(enemyCharacterManager != null)
                {
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamagerStandPosition.position;
                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimatorManager.PlayTargetAnimation("Back Stab", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimationWithRootRotation("Back Stabbed", true);
                }
            }
            else if (Physics.Raycast(inputHandler.criticalAttackRayCastStartPoint.position, transform.TransformDirection(Vector3.forward), out hit, 1f, riposteLayer))
            {
                //Check for team ID
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = playerWeaponSlotManager.rightHandDamageCollider;

                if(enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
                {
                    playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamagerStandPosition.position;

                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimatorManager.PlayTargetAnimation("Riposte", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposte_Stabbed", true);
                }
            }
        }
    }
}
