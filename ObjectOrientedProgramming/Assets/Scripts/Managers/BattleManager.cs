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
            int conSave = 0;

            Character hitTarget;

            int damage = 0;

            Party party = gameState.party;
            List<Character> characterList = party.characters;
            Monster enemy = gameState.combat.monster;

            List<Creature> creatureInOrderOfInitiative = new List<Creature>(party.characters);
            creatureInOrderOfInitiative.Add(enemy);


            Console.WriteLine($"A {enemy.displayName} with {enemy.hitPoints} HP appears!");

            while (enemy.hitPoints > 0 && gameState.party.characters.Count > 0)
            {
                foreach (Character character in characterList)
                {
                    if (character.lifeStatus == LifeStatus.Dead) continue;
                    if (character.lifeStatus != LifeStatus.Conscious) 
                    {
                        yield return character.HandleUnconciousState();
                        continue;
                    }
                    yield return character.presenter.Attack();
                    damage = DiceHelper.Roll(character.weaponType.damageRoll);
                    bool wasCriticalHit = damage <= -enemy.hitPointsMaximum;
                    yield return enemy.ReactToDamage(damage, wasCriticalHit);
                    Console.WriteLine($"{character.displayName} hits the {enemy.displayName} with their {character.weaponType.displayName} for {damage}. The {enemy.displayName} has {enemy.hitPoints} HP left.");
                    if (enemy.hitPoints <= 0) break;
                    

                }
                if(enemy.hitPoints > 0)
                {
                    yield return enemy.presenter.Attack();

                    hitTarget = characterList[Random.Range(0, characterList.Count)];
                    int weaponToUse = 0;
                    if (enemy.type.weaponTypes.Length > 1) weaponToUse = DiceHelper.Roll("1d" + (enemy.type.weaponTypes.Length-1));
                    damage = DiceHelper.Roll(enemy.type.weaponTypes[weaponToUse].damageRoll);
                    int remainingDamage = damage - hitTarget.hitPoints;


                    bool wasCriticalHit = remainingDamage >= hitTarget.hitPointsMaximum;

                    Console.WriteLine($"The {enemy.displayName} attacks {hitTarget.displayName} with its {enemy.type.weaponTypes[weaponToUse].displayName} & deals {damage} damage.");

                    yield return hitTarget.ReactToDamage(damage, wasCriticalHit);
                    if (hitTarget.lifeStatus == LifeStatus.Dead)
                    {
                        Console.WriteLine($"{hitTarget.displayName} is killed.");
                        characterList.Remove(hitTarget);
                    }
                    else
                    {
                        Console.WriteLine($"{hitTarget.displayName} has {hitTarget.hitPoints} HP left.");
                    }

                }
                
            }

            if(enemy.hitPoints == 0)
            {
                Console.WriteLine($"The {gameState.combat.monster.displayName} collapses and the heroes celebrate their victory! :D");
                Console.WriteLine("");
            }
            
        }
    }
}
