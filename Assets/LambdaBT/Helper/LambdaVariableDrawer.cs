using UnityEditor;
using UnityEngine;

namespace LambdaBT.Helper
{
    [CustomPropertyDrawer(typeof(LambdaVariable))]
    public class LambdaVariableDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // Draw the variable name
            var variableNameRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            var variableNameProperty = property.FindPropertyRelative("variableName");
            EditorGUI.PropertyField(variableNameRect, variableNameProperty);

            // Draw the variable type
            var variableTypeRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2,
                position.width, EditorGUIUtility.singleLineHeight);
            var variableTypeProperty = property.FindPropertyRelative("value.variableType");
            EditorGUI.PropertyField(variableTypeRect, variableTypeProperty);

            // Draw the value based on the variable type
            var valueRect = new Rect(position.x, position.y + 2 * (EditorGUIUtility.singleLineHeight + 2),
                position.width, EditorGUIUtility.singleLineHeight);
            var variableType = (VariableType)variableTypeProperty.enumValueIndex;

            switch (variableType)
            {
                case VariableType.Float:
                    var floatValueProperty = property.FindPropertyRelative("value.floatValue");
                    EditorGUI.PropertyField(valueRect, floatValueProperty, new GUIContent("Value"));
                    break;
                case VariableType.Int:
                    var intValueProperty = property.FindPropertyRelative("value.intValue");
                    EditorGUI.PropertyField(valueRect, intValueProperty, new GUIContent("Value"));
                    break;
                case VariableType.String:
                    var stringValueProperty = property.FindPropertyRelative("value.stringValue");
                    EditorGUI.PropertyField(valueRect, stringValueProperty, new GUIContent("Value"));
                    break;
                case VariableType.Bool:
                    var boolValueProperty = property.FindPropertyRelative("value.boolValue");
                    EditorGUI.PropertyField(valueRect, boolValueProperty, new GUIContent("Value"));
                    break;
                case VariableType.GameObject:
                    var gameObjectValueProperty = property.FindPropertyRelative("value.gameObjectValue");
                    EditorGUI.PropertyField(valueRect, gameObjectValueProperty, new GUIContent("Value"));
                    break;
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 3 * (EditorGUIUtility.singleLineHeight + 2);
        }
    }
}