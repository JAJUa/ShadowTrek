
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR  
using UnityEditor;
#endif
using UnityEngine;

[CustomPropertyDrawer(typeof(ShowIfEnumAttribute))]
public class ShowIfEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute showIfEnum = (ShowIfEnumAttribute)attribute;
        SerializedProperty enumProperty = property.serializedObject.FindProperty(showIfEnum.EnumFieldName);

        if (enumProperty != null && enumProperty.enumValueIndex == showIfEnum.EnumValue)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfEnumAttribute showIfEnum = (ShowIfEnumAttribute)attribute;
        SerializedProperty enumProperty = property.serializedObject.FindProperty(showIfEnum.EnumFieldName);

        if (enumProperty != null && enumProperty.enumValueIndex == showIfEnum.EnumValue)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        return -EditorGUIUtility.standardVerticalSpacing;
    }
}
