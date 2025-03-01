using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using VInspector;


public class MapDataManager : Singleton<MapDataManager>
{
    [Serializable]
    public class MapData
    {
        public int id = 0;
        public List<SpawnCharacter> spawnCharacters = new List<SpawnCharacter>();
        public string mapPrefab;
        public Vector3Int minMapSize, maxMapSize;
        public List<Vector3Int> seraPath;

        public MapData() {}
        public MapData( MapDataSheet.Data _data)
        {
            id = _data.id;
            mapPrefab = _data.prefabName;
            minMapSize = Vector3Int.RoundToInt(_data.minMapSize);
            maxMapSize = Vector3Int.RoundToInt(_data.maxMapSize);

            string[] c = _data.Character.Split(",");
            if (c.Length > 0)
            { 
                for (int i = 0; i < c.Length; i++)
                {
                    spawnCharacters.Add(new SpawnCharacter(c[i]));
                }
            }
           

        }
    }
    
    [Serializable]
    public class SpawnCharacter
    {
        public CharacterRole characterRole;
        public Vector3Int spawnPos;

        public SpawnCharacter(string characterName)
        {
            switch (characterName)
            {
                case "Sera":
                    characterRole = CharacterRole.Sera;
                    break;
                case "Papa":
                    characterRole = CharacterRole.Papa;
                    break;
            }

            spawnPos = Vector3Int.zero;
        }
    }

    [Serializable]
    public class LevelData
    {
        public List<MapData> mapData = new List<MapData>();
    }
    
    public LevelData Data = new LevelData();
    public int testMapIndex;
    
    private string _DataFilePath;

    protected void Awake()
    {
        MapDataSheet.Data.Load();
        
        Application.targetFrameRate = 60; // 프레임 정상화 
        _DataFilePath = Path.Combine(Application.persistentDataPath, "MapData.json");
        if (File.Exists(_DataFilePath))
        {
            var levelDataJson = File.ReadAllText(_DataFilePath);
            var levelData = JsonConvert.DeserializeObject<LevelData>(levelDataJson);
            if (levelData == null)
            {
                Debug.LogError("Json 변환 실패! levelData가 NULL입니다.");
            }
            else
            {
                Debug.Log($"Json 변환 성공! mapData 개수: {levelData.mapData.Count}");
            }
            Data = levelData;
            

        }
        else
        {
            var levelData = new LevelData();
            Data = levelData;

        }

     
    }

    private void Start()
    {
        var sheetDataList = MapDataSheet.Data.DataList;
        Debug.Log(Data.mapData.Count+" , "+sheetDataList.Count);
        if (Data.mapData.Count < sheetDataList.Count)
        {
            foreach (var _mapData in sheetDataList)
            {
                if (Data.mapData.FirstOrDefault(d => d.id == _mapData.id)==null)
                {
                    Data.mapData.Add(new MapData(_mapData));
                }
            }
            Save();
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
        Data = new LevelData();
        Save();
        
    }

    private void OnApplicationQuit()
    {
        Save();
    }
    

    [Button]
    private void DeleteJson()
    {
        File.Delete(_DataFilePath);
    }
    
    
    
    public void MapSpawn()
    {
        //Instantiate(MapDataManager.Inst. mapData[MapDataManager.Inst.testMapIndex].mapPrefab, Vector3.zero, quaternion.identity);
    }

    public void NextMap()
    {
        if (++testMapIndex >= Data.mapData.Count)
            testMapIndex = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    

    
}
