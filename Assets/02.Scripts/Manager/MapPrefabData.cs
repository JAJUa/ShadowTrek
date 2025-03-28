using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPrefabData : MonoBehaviour
{
    public static MapPrefabData Inst;
    public Transform interactionGimic, interactionLights, interactionBoth;
    public Transform prefabCam;
    [HideInInspector]public Vector3 camPos,camRot;

    private void Awake()
    {
        camPos = prefabCam.position;
        camRot = new Vector3(prefabCam.rotation.eulerAngles.x, prefabCam.rotation.eulerAngles.y, prefabCam.rotation.eulerAngles.z);
        Inst = this;
    }
}
