using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Inst;

    public int clearLevel;
    public int lastClearStage;
    public int localizationNum = 1;
    public int skinNum = 10 ;
    public List<List<bool>> relicsBool = new List<List<bool>>();
    public List<int> relicsMaxCount = new List<int> { 14, 12, 8, 4 };
    public List<int> relicsCurCount = new List<int> { 0,0,0,0};
    public float bgmVolume, soundEffectVolume;



    private IEnumerator Start()
    {
        
        Inst= this;

        yield return new WaitUntil(() => SaveSystem.Inst);

        if (SaveSystem.Inst.IsNewFile())
        {
            for (int i = 0; i < relicsMaxCount.Count; i++)
            {
                relicsBool.Add(new List<bool>());
            }
            for (int i = 0; i < relicsMaxCount.Count; i++)
            {
                for (int j = 0; j < relicsMaxCount[i]; j++)
                {
                    relicsBool[i].Add(false);
                }
            }
            Debug.Log(relicsMaxCount.Count);

            Debug.Log("relicsBool 초기화 완료");
        }


     
      // Debug.Log("relicsBool[0] 리스트 크기: " + relicsBool[0].Count);
      //  Debug.Log("relicsBool[0][0] 값: " + relicsBool[0][0]);


    }

    public void LevelClear(int level)
    {
        clearLevel= level;
        SaveSystem.Inst.SaveData();
    }

    public void SetSkin(int index)
    {
        skinNum = index;
        SaveSystem.Inst.SaveData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            /*
            for (int i = 0; i < relicsCurCount.Count; i++)
            {
                relicsBool.Add(new List<bool>());
                for (int j = 0; j < relicsMaxCount[i]; j++)
                {
                    relicsBool[i].Add(false);
                }
            }*/

            SaveSystem.Inst.SaveData();
        }

        if (Input.GetKeyDown(KeyCode.F11))
        {
            ResetData();
        }

    }

    public void ResetData()
    {
        clearLevel=0;
        lastClearStage=0;
        localizationNum = 2;
        skinNum = 0;
        relicsBool.Clear();
        relicsCurCount.Clear();
        SaveSystem.Inst.SaveData();
    }

    public void GetRelic(int relicType,int relicNumber)  //relicType 은 호리병/호박/로봇 등 종류, relicNumber는 그 종류 중에서 몇번째 유물인지
    {
        relicsBool[relicType][relicNumber] = true;
        relicsCurCount[relicType]++;
        SaveSystem.Inst.SaveData();
    }

    public void StageClear(int lastLevel)
    {
        lastClearStage= lastLevel;
        if(clearLevel >= lastLevel)
        {
          
                
        }
        else
        {
            clearLevel= lastLevel;
        }
        
       
        SaveSystem.Inst.SaveData();
    }

    public void ChangeLocalization(int index)
    {
        localizationNum= index;
        SaveSystem.Inst.SaveData();
    }
}
