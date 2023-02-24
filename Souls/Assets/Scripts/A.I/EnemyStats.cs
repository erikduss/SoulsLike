using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class EnemyStats : CharacterStats
    {
        Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFromHealthLevel()
        {
            //skill level increase etc things that boost health
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage, bool playAnimation)
        {
            if (isDead)
            {
                return;
            }
            currentHealth = currentHealth - damage;

            if(playAnimation) animator.Play("Take Damage");

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                if (playAnimation) animator.Play("Death");
                isDead = true;
            }
        }
    }
}
