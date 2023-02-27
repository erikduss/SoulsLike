using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class EnemyStats : CharacterStats
    {
        EnemyAnimatorManager enemyAnimatorManager;

        public UIEnemyHealthBar enemyHealthBar;

        public int soulsAwardedOnDeath = 50;

        private void Awake()
        {
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            enemyHealthBar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            //skill level increase etc things that boost health
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public override void TakeDamage(int damage, bool playAnimation, string damageAnimation = "Take Damage")
        {
            if (isDead)
            {
                return;
            }
            currentHealth = currentHealth - damage;
            enemyHealthBar.SetHealth(currentHealth);

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
