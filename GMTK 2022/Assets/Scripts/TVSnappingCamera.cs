using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVSnappingCamera : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;

    void Update()
    {
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, transform.position.z);
    }
}
