using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class DataManager : MonoBehaviour
{
    public static DataManager Inst;
    public int testMapIndex;
    
    [Serializable]
    public struct MapData
    {
        public List<SpawnCharacter> SpawnCharacters;
        public GameObject mapPrefab;
        public Vector3Int minMapSize, maxMapSize;
        
    }
    
    [Serializable]
    public struct SpawnCharacter
    {
        public CharacterRole characterRole;
        public Vector3Int spawnPos;
    }

    public List<MapData> mapData;

    private void Awake()
    {
        MapSpawn();
        Inst = this;
    }
    
    
    public void MapSpawn()
    {
        Instantiate(mapData[testMapIndex].mapPrefab, Vector3.zero, quaternion.identity);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
