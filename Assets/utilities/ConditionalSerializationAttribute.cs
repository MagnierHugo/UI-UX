
using System;

using UnityEngine;


namespace EditorUtilities
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public abstract class ConditionalSerializationAttribute : PropertyAttribute
    {
        public string ConditionField { get; protected set; } = null;
        public bool InvertCondition { get; protected set; } = false;
    }
}
