using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTile : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Active()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        CutSceneManager.Inst.StartCutScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
