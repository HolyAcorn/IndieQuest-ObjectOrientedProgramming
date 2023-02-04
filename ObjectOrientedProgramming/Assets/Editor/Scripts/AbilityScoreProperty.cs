using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace MonsterQuest
{
    [CustomPropertyDrawer(typeof(AbilityScore))]
    public class AbilityScoreProperty : PropertyDrawer
    {

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement abilityScoreElement = new VisualElement();

            PropertyField scoreProperty = new PropertyField(property.FindPropertyRelative("score"), "");




            abilityScoreElement.Add(scoreProperty);



            return abilityScoreElement;
        }


    }
}
