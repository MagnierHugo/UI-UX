using System;

using UnityEditor;


namespace EditorUtilities
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SerializeFieldIfAttribute : ConditionalSerializationAttribute
    {
        public SerializeFieldIfAttribute(string conditionField, bool invertCondition = false)
        {
            ConditionField = conditionField;
            InvertCondition = invertCondition;
        }
    }


    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class IfAttribute : SerializeFieldIfAttribute // shorter (use when paired with [SerializeField])
    {
        public IfAttribute(string conditionField, bool invertCondition = false)
            : base(conditionField, invertCondition) { }
    }

    [
        CustomPropertyDrawer(typeof(SerializeFieldIfAttribute)),
        CustomPropertyDrawer(typeof(IfAttribute)),
    ]
    public sealed class SerializeFieldIfDrawer : SerializeFieldIfDrawerBase
    {
        protected override bool ShouldBeSerialized(SerializedProperty property)
        {
            SerializeFieldIfAttribute ifAttribute = (SerializeFieldIfAttribute)attribute; // retrieve the relevant attribute

            SerializedProperty firstConditionField = property.serializedObject.FindProperty(ifAttribute.ConditionField)
                ?? throw new Exception("The condition field doesn t exist");


            if (firstConditionField.propertyType != SerializedPropertyType.Boolean)
                throw new Exception("The condition field is not a boolean");

            return ifAttribute.InvertCondition ? !firstConditionField.boolValue : firstConditionField.boolValue;
        }
    }
}