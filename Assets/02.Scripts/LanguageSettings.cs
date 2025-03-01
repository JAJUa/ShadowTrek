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
        curLanguageNum = DataManager.Inst.Data.localizationNum;
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
        DataManager.Inst.ChangeLocalization(curLanguageNum);
    }

    IEnumerator waitLocalization()
    {
        yield return new WaitForSeconds(0.5f);
        int localizedIndex = DataManager.Inst.Data.localizationNum > 0 ? DataManager.Inst.Data.localizationNum - 1 : DataManager.Inst.Data.localizationNum;

        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localizedIndex];
    }
}
