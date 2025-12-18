using System;

using UnityEditor;


namespace EditorUtilities
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class SerializeFieldIfFieldsAreEqualAttribute : ConditionalSerializationAttribute
    {
        public string ConditionField_ { get; protected set; }
        public SerializeFieldIfFieldsAreEqualAttribute(string conditionField, string conditionField_, bool invertCondition = false)
        {
            ConditionField = conditionField;
            ConditionField_ = conditionField_;
            InvertCondition = invertCondition;
        }
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class IfFieldsAreEqualAttribute : SerializeFieldIfFieldsAreEqualAttribute
    {
        public IfFieldsAreEqualAttribute(string conditionField, string conditionField_, bool invertCondition = false)
            : base(conditionField, conditionField_, invertCondition) { }
    }

    [
       CustomPropertyDrawer(typeof(SerializeFieldIfFieldsAreEqualAttribute)),
       CustomPropertyDrawer(typeof(IfFieldsAreEqualAttribute)),
    ]
    public sealed class SerializeFieldIFieldsAreEqualDrawer : SerializeFieldIfDrawerBase
    {
        protected override bool ShouldBeSerialized(SerializedProperty property)
        {
            SerializeFieldIfFieldsAreEqualAttribute ifAttribute = (SerializeFieldIfFieldsAreEqualAttribute)attribute; // retrieve the relevant attribute

            SerializedProperty firstConditionField = property.serializedObject.FindProperty(ifAttribute.ConditionField)
                ?? throw new Exception("The condition field doesn t exist");


            SerializedProperty secondConditionField = property.serializedObject.FindProperty(ifAttribute.ConditionField_)
                ?? throw new Exception("The second condition field doesn t exist");

            if (firstConditionField.propertyType == SerializedPropertyType.Enum) // if is enum field then it will decay to int when being serialized
                if (GetSerializedPropertyTargetFieldType(firstConditionField) == GetSerializedPropertyTargetFieldType(secondConditionField)) // so we check wether the actual original type matches
                    return ifAttribute.InvertCondition ?
                        firstConditionField.enumValueIndex != secondConditionField.enumValueIndex :
                        firstConditionField.enumValueIndex == secondConditionField.enumValueIndex
                    ;
                else
                    return false; // not same enum type
            else
                return ifAttribute.InvertCondition ?
                    !GetRelevantValue(firstConditionField).Equals(GetRelevantValue(secondConditionField)) :
                    GetRelevantValue(firstConditionField).Equals(GetRelevantValue(secondConditionField))
                    ;
        }
    }
}