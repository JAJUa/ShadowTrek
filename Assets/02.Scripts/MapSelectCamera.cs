using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelectCamera : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x,transform.position.y,transform.position.z);
    }
}
