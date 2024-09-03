using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    public GameObject player;
    public Material[] materials;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine( WaitForChangeSkin());
       
    }

    IEnumerator WaitForChangeSkin()
    {
        yield return new WaitForSeconds(0.05f);
        player.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material = materials[GameData.Inst.skinNum];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
