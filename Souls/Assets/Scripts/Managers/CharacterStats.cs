using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class CharacterStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 10;
        public float maxStamina;
        public float currentStamina;

        public int focusLevel = 10;
        public float maxFocusPoints;
        public float currentFocusPoints;

        public int soulCount = 0;

        [Header("Armor Absorptions")]
        public float physicalDamageAbsorptionHead;
        public float physicalDamageAbsorptionBody;
        public float physicalDamageAbsorptionLegs;
        public float physicalDamageAbsorptionHands;

        public bool isDead;

        public virtual void TakeDamage(int phyisicalDamage, bool playAnimation, string damageAnimation = "Take Damage")
        {
            if (isDead) return;

            float totalPhysicalDamageAbsorption = 1 -
                (1 - physicalDamageAbsorptionHead / 100) *
                (1 - physicalDamageAbsorptionBody / 100) *
                (1 - physicalDamageAbsorptionLegs / 100) *
                (1 - physicalDamageAbsorptionHands / 100);

            phyisicalDamage = Mathf.RoundToInt(phyisicalDamage - (phyisicalDamage * totalPhysicalDamageAbsorption));

            Debug.Log("Total Damage Absorption: " + totalPhysicalDamageAbsorption + "%");

            float finalDamage = phyisicalDamage; //+Fire damage, etc, etc.

            currentHealth = Mathf.RoundToInt(currentHealth - finalDamage);

            Debug.Log("Total Damage Dealt is: " + finalDamage);

            if(currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }
    }
}
