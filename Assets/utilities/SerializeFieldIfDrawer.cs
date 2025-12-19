#if UNITY_EDITOR
using System;


using UnityEditor;
using UnityEngine;


namespace EditorUtilities
{
    public abstract class SerializeFieldIfDrawerBase : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!ShouldBeSerialized(property)) 
                return;

            EditorGUI.PropertyField(position, property, label, true);
        }

        /// <summary>
        /// Necessary when there are other fields to serialize because otherwise it would skip the space need for the serializeation of this variable before serializing the next field even when this one is not serialized (this probably makes no sense but idc im solo so far
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        /// <returns></returns>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => ShouldBeSerialized(property) ? EditorGUI.GetPropertyHeight(property, label) : 0f; // doesn t account for the padding between each variable

        protected abstract bool ShouldBeSerialized(SerializedProperty property);
        
        protected Type GetSerializedPropertyTargetFieldType(SerializedProperty targetProperty)
            => targetProperty.serializedObject.targetObject.GetType().GetField(targetProperty.propertyPath).FieldType;

        protected object GetRelevantValue(SerializedProperty conditionField) // boxing goes brrr
        {
            return conditionField.propertyType switch
            {
                SerializedPropertyType.Integer => conditionField.intValue,
                SerializedPropertyType.Boolean => conditionField.boolValue,
                SerializedPropertyType.Float => conditionField.floatValue,
                SerializedPropertyType.String => conditionField.stringValue,
                SerializedPropertyType.Color => conditionField.colorValue,
                SerializedPropertyType.ObjectReference => conditionField.objectReferenceValue,
                SerializedPropertyType.Enum => conditionField.enumValueIndex,
                SerializedPropertyType.Vector2 => conditionField.vector2Value,
                SerializedPropertyType.Vector3 => conditionField.vector3Value,
                SerializedPropertyType.Vector4 => conditionField.vector4Value,
                SerializedPropertyType.Rect => conditionField.rectValue,
                SerializedPropertyType.Bounds => conditionField.boundsValue,
                SerializedPropertyType.Quaternion => conditionField.quaternionValue,
                SerializedPropertyType.AnimationCurve => conditionField.animationCurveValue,
                SerializedPropertyType.LayerMask => conditionField.intValue,  // LayerMask is stored as int
                SerializedPropertyType.ArraySize => conditionField.intValue,  // Array size is stored as int
                SerializedPropertyType.Character => (char)conditionField.intValue,
                SerializedPropertyType.ExposedReference => conditionField.exposedReferenceValue,
                SerializedPropertyType.RectInt => conditionField.rectIntValue,
                SerializedPropertyType.BoundsInt => conditionField.boundsIntValue,
                SerializedPropertyType.Vector2Int => conditionField.vector2IntValue,
                SerializedPropertyType.Vector3Int => conditionField.vector3IntValue,
                _ => throw new System.Exception($"Unsupported SerializedPropertyType: {conditionField.propertyType}"),
            };
        }

    }
}



#endif