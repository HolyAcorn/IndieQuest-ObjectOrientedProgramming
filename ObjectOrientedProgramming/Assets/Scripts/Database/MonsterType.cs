using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu(fileName ="MonsterType", menuName ="Scriptable Objects/MonsterType")]
    public class MonsterType : ScriptableObject
    {
        public string displayName;
        public string alignment;
        public string hitPointsRoll;
        public int armorClass;
        public ArmorType armorType;
        public WeaponType[] weaponTypes;
        public Sprite bodySprite;
        public SizeCategory sizeCategory;
    }
}
