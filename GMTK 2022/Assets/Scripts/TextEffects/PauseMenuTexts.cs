using System;
using UnityEngine;
using TMPro;

public class PauseMenuTexts : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private Color color;
    
    [SerializeField] private PuaseMenu puaseMenu;

    [SerializeField] private PauseMenuTexts2 pauseMenuTexts2;
    
    private void Start()
    {
        puaseMenu.OnResumed += OnResumed;
    }

    private void OnDestroy()
    {
        puaseMenu.OnResumed -= OnResumed;
    }

    private void OnResumed() => OnMouseExit();
    
    private void OnMouseEnter()
    {
        if (pauseMenuTexts2.ReadyToClick)
        {
            AudioManager.Instance.Play("MenuSelect");
            text.color = color;
        }

    }

    private void OnMouseExit()
    {
        text.color = Color.white;
    }
    
    private void OnMouseDown()
    {
        if (pauseMenuTexts2.ReadyToClick)
        {
            puaseMenu.Resume();
        }

    }
}
