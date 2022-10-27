using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;

namespace MonsterQuest
{
    public class GameManager : MonoBehaviour
    {
        List<string> pcNames = new List<string> { "Jazlyn", "Theron", "Dayana", "Rolando" };
        List<string> enemyTypes = new List<string> { "Orc", "Mage", "Troll" };
        List<int> enemyHPs = new List<int> { 15, 40, 84 };
        List<int> dcValue = new List<int> { 12, 20, 18 };
        List<Monster> monsters = new List<Monster>();

        Random random = new Random();
        private BattleManager combatManager;

        private GameState gameState;

        private void Awake()
        {
            Transform battleTransform = transform.Find("Combat");
            combatManager = battleTransform.GetComponent<BattleManager>();
        }

        private void Start()
        {
            NewGame();
            Simulate();

        }

        private void NewGame()
        {
            List<Character> initialCharacterList = new List<Character>();
            foreach (string name in pcNames)
            {
                Character character = new Character(name);
                initialCharacterList.Add(character);
            }
            Party initialParty = new Party(initialCharacterList);
            gameState = new GameState(initialParty);


            for (int i = 0; i < enemyTypes.Count; i++)
            {
                Monster monster = new Monster(enemyTypes[i], enemyHPs[i], dcValue[i]);
                monsters.Add(monster);
            }

        }

        private void Simulate()
        {
            Console.Write("A party of warriors " + StringHelper.JoinWithAnd(pcNames));
            Console.Write(" descends into the dungeon.");
            Console.WriteLine("");
            while(monsters.Count > 0 && gameState.party.characters.Count > 0)
            {
                gameState.EnterCombatWithMonster(monsters[0]);
                combatManager.SimulateBattle(gameState);
                if (gameState.combat.monster.hitPoints <= 0)
                {
                    Console.WriteLine($"The {gameState.combat.monster.displayName} collapses and the heroes celebrate their victory! :D");
                    monsters.RemoveAt(0);
                    Console.WriteLine("");
                }

            }
            if (gameState.party.characters.Count > 0)
            {
                Console.WriteLine("The players leave the dungeon, the surviving members are " + StringHelper.JoinWithAnd(pcNames));
            }
            else
            {
                Console.WriteLine("The heroes all die in the dungeon.");
            }
        }
       
        
    }
}
