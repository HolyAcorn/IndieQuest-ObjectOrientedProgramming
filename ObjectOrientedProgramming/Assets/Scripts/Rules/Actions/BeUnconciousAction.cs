using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterQuest
{
    public class BeUnconciousAction : IAction
    {
        private Character character;

        public BeUnconciousAction(Character character)
        {
            this.character = character;
        }


        public IEnumerator Execute()
        {
            if (character.lifeStatus == LifeStatus.UnstableUnconcious) yield return character.HandleUnconciousState();
            else if (character.lifeStatus == LifeStatus.StableUnconcious) yield break ;
        }
    }
}
