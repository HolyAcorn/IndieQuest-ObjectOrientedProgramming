using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class Monster : Creature
    {
        private static readonly bool[] _deathSavingThrows = Array.Empty<bool>();

        public int savingThrowDC { get; private set; }
        public MonsterType type { get; private set; }

        public override IEnumerable<bool> deathSavingThrows => _deathSavingThrows;

        public Monster(MonsterType type) : base(type.displayName, type.bodySprite, type.sizeCategory)
        {
            this.type = type;
            base.hitPointsMaximum = DiceHelper.Roll(type.hitPointsRoll);
            Initialize();
        }
        
        
    }
}
