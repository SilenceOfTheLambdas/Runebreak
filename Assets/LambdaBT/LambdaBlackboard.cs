using System;
using System.Collections.Generic;
using UnityEngine;

namespace LambdaBT
{
    [CreateAssetMenu(fileName = "Blackboard", menuName = "Lambda BT/Blackbaord")] [Serializable]
    public class LambdaBlackboard : ScriptableObject
    {
        public List<LambdaVariable> variables;

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
                    return string.Equals(variable.variableName, varName, StringComparison.CurrentCultureIgnoreCase) ? variable.GetValue() : null;
                }
            }

            return null;
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
