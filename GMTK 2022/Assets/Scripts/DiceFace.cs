using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceFace : MonoBehaviour
{
    public bool put = false;
    public bool full = false;
    public Skill activeSkill = null;

    private Vector3 origPos;

    private void Start()
    {
        origPos = transform.position;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!full)
        {
            put = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        put = false;
    }
}
