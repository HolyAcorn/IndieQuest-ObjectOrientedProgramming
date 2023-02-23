using Codice.Client.Common.GameUI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace MonsterQuest
{
    public static class MonsterTypeImporter
    {

        public static string[] monsterIndexNames
        {
            get { return LoadMonsterNames(); }
            set { value = LoadMonsterNames(); }
        }
        private static MonsterIndexEntry[] monsterIndexEntries;

        private static string[] LoadMonsterNames()
        {
            string responseJson = GetResponseJson();
            MonsterIndexResponse monsterIndexResponse = JsonConvert.DeserializeObject<MonsterIndexResponse>(responseJson);
            monsterIndexEntries = monsterIndexResponse.results;
            string[] names = new string[monsterIndexEntries.Length];
            for (int i = 0; i < monsterIndexEntries.Length; i++)
            {
                MonsterIndexEntry monsterIndexEntry = monsterIndexEntries[i];
                names[i] = monsterIndexEntry.name;
            }
            return names;
        }

        private static string GetResponseJson(string index = "")
        {
            HttpClient httpClient = new HttpClient();
            string responseJson = httpClient.GetStringAsync("https://www.dnd5eapi.co/api/monsters" + index).Result;
            return responseJson;
        }

        public static void ImportData(string name, MonsterType monsterType)
        {
            string index = name.Replace(' ', '-');
            index = index.ToLower();
            string responseJson = GetResponseJson("/" + index);
            JObject monsterData = JObject.Parse(responseJson);
            monsterType.displayName = (string)monsterData["name"];
            monsterType.armorClass = (int)monsterData["armor_class"][0]["value"];
            monsterType.hitPointsRoll = (string)monsterData["hit_dice"];
            monsterType.abilityScores.wisdom.score = (int)monsterData["wisdom"];
            monsterType.abilityScores.charisma.score = (int)monsterData["charisma"];
            monsterType.abilityScores.constitution.score = (int)monsterData["constitution"];
            monsterType.abilityScores.dexterity.score = (int)monsterData["dexterity"];
            monsterType.abilityScores.intelligence.score = (int)monsterData["intelligence"];
            monsterType.abilityScores.strength.score = (int)monsterData["strength"];
            monsterType.alignment = (string)monsterData["alignment"];
            monsterType.sizeCategory = SizeCategory.Parse<SizeCategory>((string)monsterData["size"]);
        }
        
    }

    public class MonsterIndexEntry
    {
        public string index;
        public string name;
    }

    public class MonsterIndexResponse
    {
        int count;
        public MonsterIndexEntry[] results;
    }

}
