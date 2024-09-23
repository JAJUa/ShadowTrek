using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class LanguageSettings : MonoBehaviour
{
    private int curLanguageNum;

    private void Start()
    {
        StartCoroutine(waitLocalization());
        curLanguageNum = GameData.Inst.localizationNum;
    }

    public void ChangeLanguage(bool right)
    {
        if (right)
        {
            if (curLanguageNum == 3)
                curLanguageNum = 1;
            else
                curLanguageNum++;
        }
        else
        {
            if (curLanguageNum == 1)
                curLanguageNum = 3;
            else
                curLanguageNum--;
        }
        UserLcoalization();
    }

    public void UserLcoalization()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[curLanguageNum - 1];
        GameData.Inst.ChangeLocalization(curLanguageNum);
    }

    IEnumerator waitLocalization()
    {
        yield return new WaitForSeconds(0.5f);
        int localizedIndex = GameData.Inst.localizationNum > 0 ? GameData.Inst.localizationNum - 1 : GameData.Inst.localizationNum;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localizedIndex];
    }
}
