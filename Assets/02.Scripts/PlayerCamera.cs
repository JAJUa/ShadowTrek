using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float sensitivity;
    private float eulerAngleY;
    private float eulerAngleX;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        eulerAngleY += mouseX * sensitivity * Time.deltaTime;
        eulerAngleX-= mouseY * sensitivity * Time.deltaTime;
        eulerAngleX = Mathf.Clamp(eulerAngleX, -80, 50);
        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }
}
