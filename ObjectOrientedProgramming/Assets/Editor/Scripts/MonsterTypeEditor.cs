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

            monsterTypeXML.CloneTree(monsterVisual);



            return monsterVisual;
        }
    }
}
