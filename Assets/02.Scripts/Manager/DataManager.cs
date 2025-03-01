using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using VInspector;

[Serializable]
public class PlayerData
{
    public int lastClearStage = 2;
    public int localizationNum = 2;
    public int skinNum = 0 ;
    public List<List<bool>> relicsBool = new List<List<bool>>();
    public List<int> relicsMaxCount = new List<int> { 14, 12, 8, 4 };
    public List<int> relicsCurCount = new List<int> { 0,0,0,0};
    public float bgmVolume, soundEffectVolume;
    public bool selectionTuto1 = false,selectionTuto2 = false;

}


public class DataManager : SingletonDontDestroyOnLoad<DataManager>
{
    private string _DataFilePath;
    public PlayerData Data = new PlayerData();

    protected void Awake()
    {
        Application.targetFrameRate = 60; // 프레임 정상화 
        _DataFilePath = Path.Combine(Application.persistentDataPath, "unitData.json");
        if (File.Exists(_DataFilePath))
        {
            var playerDataJson = File.ReadAllText(_DataFilePath);
            var playerData = JsonConvert.DeserializeObject<PlayerData>(playerDataJson);
            Data = playerData;
            if(Data.relicsBool.Count ==0)
                ResetRelicsBool();
        }
        else
        {
            var playerData = new PlayerData();
            Data = playerData;
            ResetRelicsBool();
        }
    }

    private void ResetRelicsBool()
    {
        Data.relicsBool.Clear();
        for (int i = 0; i < Data.relicsMaxCount.Count;i++)
        {
            List<bool> l = new List<bool>();
            for (int j = 0; j < Data.relicsMaxCount[i]; j++)
            {
                l.Add(false);
            }
            Data.relicsBool.Add(l);
        }
    }
    
    [Button]
    public void Save()
    {
        if (Data == null) return;
        
        var json = JsonConvert.SerializeObject(Data, Formatting.Indented, 
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

        File.WriteAllText(_DataFilePath, json);
    }

    [Button]
    public void ResetData()
    {
        Data = new PlayerData();
        Save();
        
    }

    private void OnApplicationQuit()
    {
        Save();
    }
    
    public void GetRelic(int relicType,int relicNumber)  //relicType 은 호리병/호박/로봇 등 종류, relicNumber는 그 종류 중에서 몇번째 유물인지
    {
        Data.relicsBool[relicType][relicNumber] = true;
        Data.relicsCurCount[relicType]++;
    }
    
    public void SetSkin(int index)
    {
        Data.skinNum = index;
    }

    [Button]
    private void DeleteJson()
    {
        File.Delete(_DataFilePath);
    }

    public void ChangeLocalization(int index)
    {
        Data.localizationNum = index;
    }

    public void StageClear(int stageIndex)
    {
        if (Data.lastClearStage < stageIndex)
        {
            Data.lastClearStage = stageIndex;
        }
    }
}
