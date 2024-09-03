using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowRot : MonoBehaviour
{
    float angle = 0;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        angle = Input.GetAxisRaw("Horizontal") * Time.deltaTime * speed;
        Vector3 curRot = transform.eulerAngles;
        curRot.y += angle;
        transform.eulerAngles= curRot;
    }
}
