using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

namespace MonsterQuest
{
    [CustomEditor(typeof(MonsterType))]
    public class MonsterTypeEditor : Editor
    {
        public VisualTreeAsset monsterTypeXML;

        public override VisualElement CreateInspectorGUI()
        {
            VisualElement monsterVisual = new VisualElement();


            DropdownField dropdown = new DropdownField();
            string[] monsters = MonsterTypeImporter.monsterIndexNames;
            dropdown.choices = monsters.ToList();
            monsterVisual.Add(dropdown);

            monsterTypeXML.CloneTree(monsterVisual);

            dropdown.RegisterValueChangedCallback(OnMonsterDataChangeEvent);
            

            return monsterVisual;



        }

        private void OnMonsterDataChangeEvent(ChangeEvent<string> evt)
        {
            MonsterTypeImporter.ImportData(evt.newValue, (MonsterType)serializedObject.targetObject);
        }
    }
}
