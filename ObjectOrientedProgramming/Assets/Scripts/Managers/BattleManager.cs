using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MonsterQuest
{
    public class BattleManager : MonoBehaviour
    {
        public IEnumerator SimulateBattle(GameState gameState)
        {


            bool combatResolved = false;

            Party party = gameState.party;
            List<Character> characterList = party.characters;
            Monster enemy = gameState.combat.monster;

            List<Creature> creatureInOrderOfInitiative = new List<Creature>(party.characters);
            creatureInOrderOfInitiative.Add(enemy);
            creatureInOrderOfInitiative.Shuffle();

            Console.WriteLine($"A {enemy.displayName.ToUpperFirst()} with {enemy.hitPoints} HP appears!");

            do
            {
                foreach (Creature creature in creatureInOrderOfInitiative)
                {
                    if (creature.lifeStatus == LifeStatus.Dead) continue;


                    IAction action = creature.TakeTurn(gameState);
                    yield return action.Execute();

                    combatResolved = enemy.lifeStatus == LifeStatus.Dead || party.aliveCount == 0;
                    if (combatResolved) break;
                }


            } while (!combatResolved);

            if(enemy.lifeStatus == LifeStatus.Dead)
            {
                Console.WriteLine($"The {enemy.displayName.ToUpperFirst()} collapses and the heroes celebrate their victory! :D");
                Console.WriteLine("");
            }
            else
            {

                Console.WriteLine($"The party has failed and  {enemy.displayName.ToUpperFirst()} lives!");
                Console.WriteLine("");
            }
         
            
        }

    }
}
