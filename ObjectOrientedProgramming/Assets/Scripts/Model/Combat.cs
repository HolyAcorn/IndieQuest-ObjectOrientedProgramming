using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class Combat
    {
        public Monster monster { get; private set; }

        public Combat(Monster monster)
        {
            this.monster = monster;
        }
    }
}
