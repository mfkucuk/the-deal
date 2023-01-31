using UnityEngine;

public class DestroySave : MonoBehaviour
{
    [SerializeField] private GameObject mainObject;
    
    public void DoAnimEnd()
    {
        Destroy(mainObject);
    }
}
