using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        Collider damageCollider;
        public bool enabledDamageColliderOnStartup = false;

        public int currentWeaponDamage = 25;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = enabledDamageColliderOnStartup;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider collision)
        {
            if(collision.tag == "Player")
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if(enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        //get parried if parryable
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                    else if (shield != null && enemyCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;
                        
                        if(playerStats != null)
                        {
                            playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), true, "Block_Guard");
                            return;
                        }
                    }
                }

                if(playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage,true);
                }
            }

            if(collision.tag == "Enemy")
            {
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();
                CharacterManager enemyCharacterManager = collision.GetComponent<CharacterManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (enemyCharacterManager != null)
                {
                    if (enemyCharacterManager.isParrying)
                    {
                        //get parried if parryable
                        characterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                        return;
                    }
                    else if (shield != null && enemyCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                        if (enemyStats != null)
                        {
                            enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), true, "Block_Guard");
                            return;
                        }
                    }
                }

                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeaponDamage,true);
                }
            }
        }
    }
}
