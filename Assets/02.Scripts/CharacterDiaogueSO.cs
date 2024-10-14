using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

[CreateAssetMenu(fileName = "CharacterDialogueSO", menuName = "ScriptableObject/CharacterDialogue")]
public class CharacterDiaogueSO : ScriptableObject
{
    public LocalizedString[] localizeName;
}
