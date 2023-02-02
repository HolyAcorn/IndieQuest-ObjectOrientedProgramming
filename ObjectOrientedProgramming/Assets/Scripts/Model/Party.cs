using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public class Party
    {
        public List<Character> characters { get; private set; }
        public IEnumerable<Character> aliveCharacters => characters.Where(character => character.isAlive);
        public int aliveCount => characters.Count(character => character.isAlive);

        public Party(IEnumerable<Character> initialCharacters)
        {
            characters = initialCharacters.ToList();
        }

        public string ToString()
        {
            return StringHelper.JoinWithAnd(characters.Select(character => character.displayName));
        }

        public Character GetCharacterWithLowestHP()
        {
            Character character = characters[0];
            for (int i = 1; i < characters.Count; i++)
            {
                if(!character.isAlive || characters[i].hitPoints < character.hitPoints) character = characters[i];
            }
            return character;
        }

    }
}
