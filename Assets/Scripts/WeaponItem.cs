using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DarkSouls
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool isUnarmed;

        [Header("One Handed Attack Animations")]
        public string OH_LIGHT_ATTACK_1;
        public string OH_HEAVY_ATTACK_1;
    }
}