using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeapons : MonoBehaviour
{
    private void Update() 
    {
        if(!PuaseMenu.GameIsPaused)
            AimToMousePosition();
    }
    public void AimToMousePosition()
    {
        Vector3 aimDirection = (GetMousePosition() - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
    public Vector3 GetMousePosition()
    {
        Vector3 vec = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec.z = 0f;
        return vec;
    }
}
