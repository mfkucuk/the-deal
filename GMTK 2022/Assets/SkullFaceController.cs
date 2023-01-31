using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullFaceController : MonoBehaviour
{
    [SerializeField] private Animator skullFaceAnimator;
    
    [SerializeField] private float timerFirstValue;
    private float timer;

    [SerializeField] string[] animationNames;
    
    private void Start()
    {
        timer = timerFirstValue;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        
        if (timer < 0)
        {
            timer = timerFirstValue;

            int index = UnityEngine.Random.Range(0, animationNames.Length);

            skullFaceAnimator.Play(animationNames[index]);

        }
        
    }
}
