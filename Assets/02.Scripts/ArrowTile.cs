using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTile : MonoBehaviour
{
    Transform transform;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerControl"))
        {
            
            other.GetComponentInChildren<Player>().CheckReplay();
        }
    }
}
