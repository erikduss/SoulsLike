using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class EnemyStats : CharacterStats
    {
        EnemyAnimatorManager enemyAnimatorManager;
        EnemyBossManager enemyBossManager;
        public UIEnemyHealthBar enemyHealthBar;
        public int soulsAwardedOnDeath = 50;

        public bool isBoss;

        private void Awake()
        {
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        void Start()
        {
            if (!isBoss)
            {
                enemyHealthBar.SetMaxHealth(maxHealth);
            }
        }

        private int SetMaxHealthFromHealthLevel()
        {
            //skill level increase etc things that boost health
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public override void TakeDamage(int damage, bool playAnimation, string damageAnimation = "Take Damage")
        {
            base.TakeDamage(damage, playAnimation, damageAnimation = "Take Damage");

            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if(isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth);
            }

            if(playAnimation) enemyAnimatorManager.PlayTargetAnimation(damageAnimation, true);

            if (currentHealth <= 0)
            {
                HandleDeath(playAnimation);
            }
        }

        private void HandleDeath(bool playDeathAnimation)
        {
            currentHealth = 0;
            if (playDeathAnimation) enemyAnimatorManager.PlayTargetAnimation("Death", true);
            isDead = true;
        }
    }
}
