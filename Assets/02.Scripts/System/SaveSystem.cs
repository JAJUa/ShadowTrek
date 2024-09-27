using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using Newtonsoft.Json;
using System.Text;
using System.Collections;
using VInspector;

[System.Serializable]
public class PlayerDatas
{
    public int clearLevel;
    public List<List<bool>> relicsBool = new List<List<bool>>();
    public List<int> relicsMaxCount = new List<int> ();
    public List<int> relicsCurCount = new List<int> ();
    public int skinNum = 1;
    public int localizationNum=2;
    public int[] Item = { 0, 0, 0 };
    public float bgmVolume = 0.5f, soundEffectVolume;
    public bool selectionTuto1,selectionTuto2;
}




public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Inst;
    public PlayerDatas playerDatas = new();
    public bool dataSuccess;

    string path;
    string fileName = "/save";

    private void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Inst = this;
        }
    }

    private IEnumerator Start()
    {
        

        yield return new WaitUntil(() => GameData.Inst);

        path = Path.Combine(Application.persistentDataPath + fileName);
        yield return new WaitUntil(() => GameData.Inst.relicSaveSuccess);
        LoadData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            FileDelete();
        }
    


    }


    [Button]
    public void FileDelete()
    {
        File.Delete(path);
        Debug.Log("Save file deleted.");
   
  
    }

    public void SaveData()
    {
        PlayerDatas playerData = new PlayerDatas();

        playerData.skinNum = GameData.Inst.skinNum;
        playerData.clearLevel = GameData.Inst.clearLevel;
       
        playerData.relicsCurCount = GameData.Inst.relicsCurCount;
        playerData.relicsMaxCount = GameData.Inst.relicsMaxCount;
        playerData.localizationNum= GameData.Inst.localizationNum;
        playerData.bgmVolume = GameData.Inst.bgmVolume;
        playerData.selectionTuto1 = GameData.Inst.selectionTuto1;
        playerData.selectionTuto2 = GameData.Inst.selectionTuto2;
        playerData.soundEffectVolume= GameData.Inst.soundEffectVolume;  
        playerData.relicsBool = GameData.Inst.relicsBool;

        string json = JsonConvert.SerializeObject(playerData);
        System.IO.File.WriteAllText(path, json);
        Debug.Log($"JSON saved to {path}");
    }

    public void LoadData()
    {
        PlayerDatas playerData = new PlayerDatas();
        if (!File.Exists(path))
        {
            SaveData();
            Debug.Log(" 새파일");
        }
        else
        {
            Debug.Log("기존파일");
            //string Data = File.ReadAllText(path);
            string json = System.IO.File.ReadAllText(path);

            //playerData = JsonUtility.FromJson<PlayerDatas>(Data);
            playerData = JsonConvert.DeserializeObject<PlayerDatas>(json);

            if(playerData != null)
            {
                GameData.Inst.skinNum= playerData.skinNum;
                GameData.Inst.clearLevel = playerData.clearLevel;
                GameData.Inst.relicsBool= playerData.relicsBool;
                GameData.Inst.relicsCurCount= playerData.relicsCurCount;
                GameData.Inst.relicsMaxCount= playerData.relicsMaxCount;
                GameData.Inst.localizationNum= playerData.localizationNum;
                GameData.Inst.bgmVolume= playerData.bgmVolume;
                GameData.Inst.selectionTuto1 = playerData.selectionTuto1;
                GameData.Inst.selectionTuto2 = playerData.selectionTuto2;

                Debug.Log(GameData.Inst.relicsBool[0][0]);
                GameData.Inst.soundEffectVolume= playerData.soundEffectVolume;
            }
        }
        dataSuccess = true;
    }

    public bool IsNewFile()
    {
        if (!File.Exists(path))
            return true;
        else return false;
    }
}

/* 
SaveSystem.instance.playerDatas.level = 5;

SaveSystem.instance.SaveData();
*/

