using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu(fileName ="WeaponType", menuName = "Scriptable Objects/WeaponType")]
    public class WeaponType : ItemType
    {
        public string damageRoll;
        public bool isRanged;
        public bool isFinesse;
    }
}
