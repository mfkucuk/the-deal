using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudio : MonoBehaviour
{
    [SerializeField] private float timeLeft = 10f;

    private float lerpTime = 3f;
    private float i = 0;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            timeLeft = 10f;

            StopAllCoroutines();

            if(i++ % 2 == 0)
                StartCoroutine(UnLerpMusic());
            else
                StartCoroutine(LerpMusic());
        }
    }

    IEnumerator UnLerpMusic()
    {
        float time = lerpTime;

        while (time <= lerpTime)
        {
                audioSource.pitch = Mathf.Lerp(0.2f, 1, time / lerpTime);
                
                yield return null;
                time -= Time.deltaTime;
        }

    }

    IEnumerator LerpMusic()
    {
        float time = 0;

        while (time <= lerpTime)
        {
            audioSource.pitch = Mathf.Lerp(0.2f, 1, time / lerpTime);

            yield return null;
            time += Time.deltaTime;
        }

    }

}
