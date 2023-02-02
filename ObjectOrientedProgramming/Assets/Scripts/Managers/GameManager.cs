using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using UnityEngine;
using System.Linq;

namespace MonsterQuest
{
    public class GameManager : MonoBehaviour
    {
        List<string> pcNames = new List<string> { "Jazlyn", "Theron", "Dayana", "Rolando" };
        [SerializeField] MonsterType[] monsterTypes;
        Random random = new Random();
        private BattleManager combatManager;
        private CombatPresenter combatPresenter;

        private GameState gameState;

        [SerializeField] private Sprite[] characterSprites;

        private void Awake()
        {
            Transform battleTransform = transform.Find("Combat");
            combatManager = battleTransform.GetComponent<BattleManager>();
            combatPresenter = battleTransform.GetComponent<CombatPresenter>();
        }

        private IEnumerator Start()
        {
            yield return Database.Initialize();
            NewGame();
            StartCoroutine(Simulate());

        }

        private void NewGame()
        {
            List<Character> initialCharacterList = new List<Character>();
            List<WeaponType> items = new List<WeaponType>();
            List<ItemType> tempItems = Database.itemTypes.ToList<ItemType>();

            for (int i = 0; i < tempItems.Count; i++)
            {
                if (tempItems[i].GetType() != typeof(ArmorType)) if (tempItems[i].weight > 0) items.Add(Database.GetItemType<WeaponType>(tempItems[i].displayName));
                
                
            }

            for (int i = 0; i < pcNames.Count; i++)
            {
                WeaponType randomWeapon = items[random.Next(0, items.Count)];
                Character character = new Character(pcNames[i], characterSprites[i], 10, SizeCategory.Medium, randomWeapon, Database.GetItemType<ArmorType>("Studded Leather"));

                initialCharacterList.Add(character);
            }
            Party initialParty = new Party(initialCharacterList);
            gameState = new GameState(initialParty);




        }

        private IEnumerator Simulate()
        {
            Console.Write("A party of warriors " + StringHelper.JoinWithAnd(pcNames));
            yield return combatPresenter.InitializeParty(gameState);
            Console.Write(" descends into the dungeon.");
            Console.WriteLine("");

            for (int i = 0; i < monsterTypes.Length; i++)
            {
                Monster monster = new Monster(monsterTypes[i]);
                gameState.EnterCombatWithMonster(monster);
                yield return combatPresenter.InitializeMonster(gameState);
                yield return combatManager.SimulateBattle(gameState);
            }


            if (gameState.party.aliveCount > 0)
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
