using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;
using System.Linq;

namespace MonsterQuest
{
    public class Monster : Creature
    {
        private static readonly bool[] _deathSavingThrows = Array.Empty<bool>();

        public int savingThrowDC { get; private set; }
        public MonsterType type { get; private set; }

        public override IEnumerable<bool> deathSavingThrows => _deathSavingThrows;
        public override int armorClass => type.armorClass;

        public override AbilityScores abilityScores => type.abilityScores;

        public Monster(MonsterType type) : base(type.displayName, type.bodySprite, type.sizeCategory)
        {
            this.type = type;
            base.hitPointsMaximum = DiceHelper.Roll(type.hitPointsRoll);
            Initialize();
        }

        public override IAction TakeTurn(GameState gameState)
        {
            Party party = gameState.party;
            Character target;

            if (abilityScores.intelligence <= 7) target = party.aliveCharacters.Random();
            else target = party.GetCharacterWithLowestHP();
            WeaponType weaponType = type.weaponTypes.Random();

            return new AttackAction(this, target, weaponType );
        }

    }
}
