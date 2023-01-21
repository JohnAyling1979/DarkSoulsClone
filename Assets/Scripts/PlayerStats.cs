using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class PlayerStats : MonoBehaviour
    {
        public int healthLevel = 10;
        public int healthFactor = 10;
        public int maxHealth;
        public int currentHealth;

        public HealthBar healthBar;

        void Start()
        {
            SetMaxHealthFromHealthLevel();

            currentHealth = maxHealth;

            healthBar.SetMaxHealth(maxHealth);
        }

        private void SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * healthFactor;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;

            healthBar.SetCurrentHealth(currentHealth);
        }
    }
}
