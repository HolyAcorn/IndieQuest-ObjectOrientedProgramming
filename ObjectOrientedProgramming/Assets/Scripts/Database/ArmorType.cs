using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [CreateAssetMenu(fileName ="ArmorType", menuName ="Scriptable Objects/ArmorType")]
    public class ArmorType : ItemType
    {
        
        public ArmorCategory category;
    }
}
