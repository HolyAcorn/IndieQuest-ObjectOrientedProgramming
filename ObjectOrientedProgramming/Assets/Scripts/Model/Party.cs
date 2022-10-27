using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterQuest
{
    public class Party
    {
        public List<Character> characters { get; private set; }

        public Party(IEnumerable<Character> initialCharacters)
        {
            characters = initialCharacters.ToList();
        }

        public string ToString()
        {
            return StringHelper.JoinWithAnd(characters.Select(character => character.displayName));
        }
    }
}
