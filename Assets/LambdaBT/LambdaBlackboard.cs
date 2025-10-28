using System;
using System.Collections.Generic;
using UnityEngine;

namespace LambdaBT
{
    [CreateAssetMenu(fileName = "Blackboard", menuName = "Lambda BT/Blackbaord")] [Serializable]
    public class LambdaBlackboard : ScriptableObject
    {
        public List<LambdaVariable> variables;

        // DEBUGGING!
        public void PrintBlackboardToConsole()
        {
            if (variables.Count > 0)
            {
                foreach (var variable in variables)
                {
                    Debug.Log($"Name: {variable.variableName} \n Value: {variable.GetValue()}");
                }
            }
        }

        public object GetValueByName(string varName)
        {
            if (variables.Count > 0)
            {
                foreach (var variable in variables)
                {
                    if (string.Equals(variable.variableName, varName, StringComparison.CurrentCultureIgnoreCase))
                        return variable.GetValue();
                }
            }
            
            Debug.LogError($"Variable '{varName}' not found in the blackboard.");
            return null;
        }

        public void SetVariableByName(string varName, object value)
        {
            if (variables.Count > 0)
            {
                foreach (var variable in variables)
                {
                    if (string.Equals(variable.variableName, varName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        variable.SetVariable(value);
                        return;
                    }
                }
            }

            Debug.LogError($"Variable '{varName}' not found in the blackboard.");
        }
    }

    [Serializable]
    public struct LambdaVariable
    {
        public string variableName;
        public LambdaValue value;

        /// <summary>
        /// Retrieves the value of the variable based on its type.
        /// </summary>
        /// <returns>
        /// The value of the variable, which can be a float, int, string, bool, or GameObject,
        /// depending on the variable's type.
        /// </returns>
        public object GetValue()
        {
            return value.variableType switch
            {
                VariableType.Float => value.floatValue,
                VariableType.Int => value.intValue,
                VariableType.String => value.stringValue,
                VariableType.Bool => value.boolValue,
                VariableType.GameObject => value.gameObjectValue,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public void SetVariable(object newValue)
        {
            switch (newValue)
            {
                case float floatValue:
                    value.floatValue = floatValue;
                    break;
                case int intValue:
                    value.intValue = intValue;
                    break;
                case string stringValue:
                    value.stringValue = stringValue;
                    break;
                case bool boolValue:
                    value.boolValue = boolValue;
                    break;
                case GameObject gameObject:
                    value.gameObjectValue = gameObject;
                    break;
                default:
                    Debug.LogError($"Unsupported type for newValue: {newValue.ToString()}! Not a VariableType.");
                    break;
            }
        }
    }

    [Serializable]
    public enum VariableType
    {
        Float,
        Int,
        String,
        Bool,
        GameObject
    }

    [Serializable]
    public struct LambdaValue
    {
        public VariableType variableType;
        public float floatValue;
        public int intValue;
        public string stringValue;
        public bool boolValue;
        public GameObject gameObjectValue;
    }
}
