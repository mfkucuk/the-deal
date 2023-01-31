using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileHolder : MonoBehaviour
{
    private static TileHolder _instance;
    public static TileHolder Instance { get { return _instance; } }

    [SerializeField] private Transform tileHolderPos;

    public float s;
    public float x;
    public float y;

    private void Start()
    {
        s = tileHolderPos.lossyScale.x;
        x = tileHolderPos.position.x + 0.1f;
        y = tileHolderPos.position.y + 0.3f;
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
}
