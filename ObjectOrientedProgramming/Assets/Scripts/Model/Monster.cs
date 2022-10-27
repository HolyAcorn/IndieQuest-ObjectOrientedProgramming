using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class Monster
    {


        public string displayName { get; private set; }

        public int hitPoints { get; private set; }

        public int savingThrowDC { get; private set; }

        public Monster(string displayName, int hitPoints, int savingThrowDC)
        {
            this.displayName = displayName;
            this.hitPoints = hitPoints;
            this.savingThrowDC = savingThrowDC;
        }
        
        public void ReactToDamage(int damageAmount)
        {
            hitPoints = Math.Max(0, hitPoints - damageAmount);
        }
    }
}
