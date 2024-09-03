using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="CutSceneSO",menuName ="ScriptableObject/CutSceneText")]
public class CutSceneSO : ScriptableObject
{
    [TextArea]public string[] cutSceneText;
}
