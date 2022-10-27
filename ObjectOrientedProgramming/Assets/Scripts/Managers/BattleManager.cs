using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MonsterQuest
{
    public class BattleManager : MonoBehaviour
    {
        public void SimulateBattle(GameState gameState)
        {
            int conSave = 0;

            int hitTarget = 0;

            int greatsword = 0;

            Party party = gameState.party;
            List<Character> characterList = party.characters;
            Monster enemy = gameState.combat.monster;

            Console.WriteLine($"A {enemy.displayName} with {enemy.hitPoints} HP appears!");

            while (enemy.hitPoints > 0)
            {
                foreach (Character character in characterList)
                {
                    greatsword = DiceHelper.Roll("2d6");
                    enemy.ReactToDamage(greatsword);
                    if (enemy.hitPoints < 0 || enemy.hitPoints == 0)
                    {
                        Console.WriteLine($"{character.displayName} hits the {enemy.displayName} for {greatsword}. The {enemy.displayName} has {enemy.hitPoints} HP left.");
                        break;
                    }
                    Console.WriteLine($"{character.displayName} hits the {enemy.displayName} for {greatsword}. The {enemy.displayName} has {enemy.hitPoints} HP left.");

                }
                hitTarget = Random.Range(0, characterList.Count);
                conSave = DiceHelper.Roll("1d20+5");
                Console.WriteLine($"The {enemy.displayName} attacks {characterList[hitTarget].displayName}. They roll a constituion save with DC {enemy.savingThrowDC} and rolls {conSave}");
                if (conSave < enemy.savingThrowDC)
                {
                    Console.WriteLine($"{characterList[hitTarget].displayName} fails their check and is killed. :c");
                    Console.WriteLine("");
                    characterList.RemoveAt(hitTarget);
                    if (characterList.Count == 0)
                    {
                        Console.WriteLine("Game over. :c");
                        break;
                    }
                }
                else
                {
                    Console.WriteLine($"{characterList[hitTarget].displayName} succeeds their saving throw.");
                    Console.WriteLine("");
                }
            }
        }
    }
}
