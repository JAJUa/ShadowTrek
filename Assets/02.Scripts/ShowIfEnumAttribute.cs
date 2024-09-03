using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowIfEnumAttribute : PropertyAttribute
{
    public string EnumFieldName { get; private set; }
    public int EnumValue { get; private set; }

    public ShowIfEnumAttribute(string enumFieldName, int enumValue)
    {
        EnumFieldName = enumFieldName;
        EnumValue = enumValue;
    }
}

