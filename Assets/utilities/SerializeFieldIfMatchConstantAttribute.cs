#if UNITY_EDITOR
using System;

using UnityEditor;


namespace EditorUtilities
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SerializeFieldIfMatchConstantAttribute : ConditionalSerializationAttribute
    {
        public object ConstantValue { get; protected set; }
        public SerializeFieldIfMatchConstantAttribute(string conditionField, object constantValue, bool invertCondition = false)
        {
            ConditionField = conditionField;
            ConstantValue = constantValue;
            InvertCondition = invertCondition;
        }
    }


    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class IfMatchConstantAttribute : SerializeFieldIfMatchConstantAttribute
    {
        public IfMatchConstantAttribute(string conditionField, object constantValue, bool invertCondition = false)
            : base(conditionField, constantValue, invertCondition) { }
    }

    [
        CustomPropertyDrawer(typeof(SerializeFieldIfMatchConstantAttribute)),
        CustomPropertyDrawer(typeof(IfMatchConstantAttribute)),
    ]
    public sealed class SerializeFieldIfMatchConstantDrawer : SerializeFieldIfDrawerBase
    {
        protected override bool ShouldBeSerialized(SerializedProperty property)
        {
            SerializeFieldIfMatchConstantAttribute ifAttribute = (SerializeFieldIfMatchConstantAttribute)attribute; // retrieve the relevant attribute

            SerializedProperty firstConditionField = property.serializedObject.FindProperty(ifAttribute.ConditionField)
                ?? throw new Exception("The condition field doesn t exist");

            if (firstConditionField.propertyType == SerializedPropertyType.Enum)  // if is enum field then it will decay to int when being serialized
                if (GetSerializedPropertyTargetFieldType(firstConditionField) == ifAttribute.ConstantValue.GetType()) // check if enum type match
                    return ifAttribute.InvertCondition ?
                        firstConditionField.enumValueIndex != Convert.ToInt32(ifAttribute.ConstantValue) : // if they do check wether the actual value match
                        firstConditionField.enumValueIndex == Convert.ToInt32(ifAttribute.ConstantValue)
                        ;
                else
                    return false;
            else
                return ifAttribute.InvertCondition ?
                    !ifAttribute.ConstantValue.Equals(GetRelevantValue(firstConditionField)) :
                    ifAttribute.ConstantValue.Equals(GetRelevantValue(firstConditionField))
                    ;
        }
    }
}

#endif