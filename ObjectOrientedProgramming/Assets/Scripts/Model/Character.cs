using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class Character
    {
        public string displayName { get; private set; }
        
        public Character(string displayName)
        {
            this.displayName = displayName;
        }
    }
}
