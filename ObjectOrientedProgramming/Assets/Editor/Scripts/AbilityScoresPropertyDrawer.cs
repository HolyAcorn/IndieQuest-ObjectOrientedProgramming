using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace MonsterQuest
{

    [CustomPropertyDrawer(typeof(AbilityScores))]
    public class AbilityScoresPropertyDrawer : PropertyDrawer
    {

        Dictionary<Ability, Label> titleLable = new Dictionary<Ability, Label>();

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement abilityScoreElement = new VisualElement();

            var popup = new UnityEngine.UIElements.PopupWindow();

            foreach (Ability ability in Enum.GetValues(typeof(Ability)))
            {
                if (ability == Ability.None) continue;
                string name = ability.ToString().ToLowerInvariant();
                Label modifierLabel = new Label();
                modifierLabel.style.fontSize = 14;
                popup.Add(modifierLabel);
                titleLable[ability] = modifierLabel;
                popup.Add(new PropertyField(property.FindPropertyRelative($"<{name}>k__BackingField"), $"{name}"));



            }

         

           abilityScoreElement.TrackSerializedObjectValue(property.serializedObject, UpdateModifiers);
            UpdateModifiers(property.serializedObject);

            abilityScoreElement.Add(popup);

            return abilityScoreElement;
        }




        private void UpdateModifiers(SerializedObject serializedObject)
        {
            if (serializedObject.targetObject is not MonsterType monsterType) return;

            foreach (Ability ability in Enum.GetValues(typeof(Ability)))
            {
                if (ability == Ability.None) continue;
                string name = ability.ToString().ToLowerInvariant().ToUpperFirst();

                titleLable[ability].text = $"{name} ({monsterType.abilityScores[ability].modifier:+#;-#;+0})";
            }
        }

    }
}
