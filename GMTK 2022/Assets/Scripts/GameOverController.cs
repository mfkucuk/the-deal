using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverController : MonoBehaviour
{
    
    void Start()
    {
        AudioManager.Instance.Play("GameOver");
    }


}
