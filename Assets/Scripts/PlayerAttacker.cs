using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    public class PlayerAttacker : MonoBehaviour
    {
        AnimateHandler animateHandler;

        void Start()
        {
            animateHandler = GetComponentInChildren<AnimateHandler>();
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            animateHandler.PlayTargetAnimation(weapon.OH_LIGHT_ATTACK_1, true);
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            animateHandler.PlayTargetAnimation(weapon.OH_HEAVY_ATTACK_1, true);
        }
    }
}
