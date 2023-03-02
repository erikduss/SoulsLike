using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class EnemyAnimatorManager : AnimatorManager
    {
        EnemyManager enemyManager;
        EnemyStats enemyStats;
        EnemyBossManager enemyBossManager;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
            enemyBossManager = GetComponentInParent<EnemyBossManager>();
            enemyStats = GetComponentInParent<EnemyStats>();
        }

        public override void TakeCriticalDamageAnimationEvent()
        {
            enemyStats.TakeDamage(enemyManager.pendingCriticalDamage,false);
            enemyManager.pendingCriticalDamage = 0;
        }

        public void CanRotate()
        {
            anim.SetBool("canRotate", true);
        }

        public void StopRotation()
        {
            anim.SetBool("canRotate", false);
        }

        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            anim.SetBool("canDoCombo", false);
        }

        public void EnableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", true);
        }

        public void DisableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", false);
        }

        public void EnableIsParrying()
        {
            enemyManager.isParrying = true;
        }

        public void DisableIsParrying()
        {
            enemyManager.isParrying = false;
        }

        public void EnableCanbeRiposted()
        {
            enemyManager.canBeRiposted = true;
        }

        public void DisableCanbeRiposted()
        {
            enemyManager.canBeRiposted = false;
        }

        public void AwardSoulsOnDeath()
        {
            //scan for every player in the scene and award them souls
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            if (playerStats != null)
            {
                playerStats.AddSouls(enemyStats.soulsAwardedOnDeath);

                if (soulCountBar != null)
                {
                    soulCountBar.SetSoulCountText(playerStats.soulCount);
                }
            }
        }

        public void InstantiateBossParticleFX()
        {
            BossFxTransform bossFxTransform = GetComponentInChildren<BossFxTransform>();

            GameObject phaseFX = Instantiate(enemyBossManager.particleFX, bossFxTransform.transform);
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.enemyRigidbody.drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemyManager.enemyRigidbody.velocity = velocity;

            if (enemyManager.isRotatingWithRootMotion)
            {
                enemyManager.transform.rotation *= anim.deltaRotation;
            }
        }
    }
}
