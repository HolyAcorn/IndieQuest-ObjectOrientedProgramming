using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    [Serializable]
    public class AbilityScore
    {
        [field: SerializeField] public int score;
        public int modifier => Mathf.FloorToInt((score - 10) / 2f);

        public static implicit operator int(AbilityScore abilityScore) { return abilityScore.score; }
    }
}
