using UnityEngine;

public class CrosshairUI : MonoBehaviour
{
    private Vector3 targetPos;
    [SerializeField] private Camera camera;

    private RectTransform rectTransform;
    [SerializeField] private RectTransform basisObject;

    void Start()
    {
        Cursor.visible = false;
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        
        targetPos = camera.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = basisObject.position.z;
        rectTransform.position = targetPos;
        
    }
}
