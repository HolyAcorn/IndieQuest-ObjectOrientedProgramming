using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [Serializable]
    public class AbilityScores
    {
        [field: SerializeField] public AbilityScore strength { get; private set; }
        [field: SerializeField] public AbilityScore dexterity { get; private set; }
        [field: SerializeField] public AbilityScore constitution { get; private set; }
        [field: SerializeField] public AbilityScore intelligence { get; private set; }
        [field: SerializeField] public AbilityScore wisdom { get; private set; }
        [field: SerializeField] public AbilityScore charisma { get; private set; }

        public AbilityScores()
        {
            strength = new AbilityScore();
            dexterity = new AbilityScore();
            constitution = new AbilityScore();
            intelligence = new AbilityScore();
            wisdom = new AbilityScore();
            charisma = new AbilityScore();
        }


        public AbilityScore this[Ability ability]
        {
            get
            {
                switch (ability)
                {
                    case Ability.Strength:
                        return strength;
                    case Ability.Dexterity:
                        return dexterity;
                    case Ability.Constitution:
                        return constitution;
                    case Ability.Intelligence:
                        return intelligence;
                    case Ability.Wisdom:
                        return wisdom;
                    case Ability.Charisma:
                        return charisma;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
