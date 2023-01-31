using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private Vector2 targetPos;
    private Camera mainCamera;
    void Start()
    {
        Cursor.visible = false;
        mainCamera = Camera.main;
    }

    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        targetPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        transform.position = targetPos;
        
    }
}
