using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class PlayerStats : CharacterStats
    {
        PlayerManager playerManager;
        HealthBar healthBar;
        StaminaBar staminaBar;
        FocusPointBar focusPointBar;

        PlayerAnimatorManager animatorHandler;

        public float staminaRegenerationAmount = 1f;
        private float staminaRegenerationTimer = 0;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            healthBar = FindObjectOfType<HealthBar>();
            staminaBar = FindObjectOfType<StaminaBar>();
            focusPointBar = FindObjectOfType<FocusPointBar>();
            animatorHandler = GetComponentInChildren<PlayerAnimatorManager>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetCurrentHealth(currentHealth);

            maxStamina = SetMaxStaminaFromStaminaLevel();
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            staminaBar.SetCurrentStamina(currentStamina);

            maxFocusPoints = SetMaxFocusPointsFromFocusLevel();
            currentFocusPoints = maxFocusPoints;
            focusPointBar.SetMaxFocusPoints(maxFocusPoints);
            focusPointBar.SetCurrentFocusPoints(currentFocusPoints);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            //skill level increase etc things that boost health
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private float SetMaxStaminaFromStaminaLevel()
        {
            //skill level increase etc things that boost stamina
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        private float SetMaxFocusPointsFromFocusLevel()
        {
            maxFocusPoints = focusLevel * 10;
            return maxFocusPoints;
        }

        public void TakeDamage(int damage, bool playAnimation)
        {
            if (playerManager.isInvulnerable) return;
            if (isDead)
            {
                return;
            }

            currentHealth = currentHealth - damage;

            healthBar.SetCurrentHealth(currentHealth);

            if (playAnimation) animatorHandler.PlayTargetAnimation("Take Damage", true);

            if(currentHealth <= 0)
            {
                currentHealth = 0;
                if (playAnimation) animatorHandler.PlayTargetAnimation("Death", true);
                isDead = true;
                //HANDLE PLAYER DEATH
            }
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina = currentStamina - damage;
            staminaBar.SetCurrentStamina(currentStamina);
        }

        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenerationTimer = 0;
            }
            else
            {
                staminaRegenerationTimer += Time.deltaTime;
                if (currentStamina < maxStamina && staminaRegenerationTimer > 1f)
                {
                    currentStamina += staminaRegenerationAmount * Time.deltaTime;
                    staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
                }
            }
        }

        public void HealPlayer(int healAmount)
        {
            currentHealth = currentHealth + healAmount;

            if (currentHealth > maxHealth) currentHealth = maxHealth;

            healthBar.SetCurrentHealth(currentHealth);
        }

        public void DeductFocusPoints(int FP)
        {
            currentFocusPoints = currentFocusPoints - FP;

            if(currentFocusPoints < 0)
            {
                currentFocusPoints = 0;
            }

            focusPointBar.SetCurrentFocusPoints(currentFocusPoints);
        }

        public void AddSouls(int souls)
        {
            soulCount = soulCount + souls;
        }
    }
}
