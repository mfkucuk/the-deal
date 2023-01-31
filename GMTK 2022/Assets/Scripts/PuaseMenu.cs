using System;
using UnityEngine;

public class PuaseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    [SerializeField] private GameObject pauseMenuBackground;

    [SerializeField] private GameObject[] closeWhenPaused;
    private bool[] closeFirstValue;

    public bool PauseMenuInActive { get; set; } = false;
    
    public Action OnPaused;
    public Action OnResumed;

    private void Start()
    {
        closeFirstValue = new bool[closeWhenPaused.Length];
    }

    void Update()
    {
        if (!PauseMenuInActive)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (GameIsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        
    }

    public void Resume()
    {
        OnResumed?.Invoke();

        for (int i = 0; i < closeWhenPaused.Length; i++)
        {
            closeWhenPaused[i].SetActive(closeFirstValue[i]);
        }
        
        pauseMenuBackground.SetActive(false);
        
        if(!DialogueManager.Instance.DialogueStopGame)
            Time.timeScale = 1f;

        GameIsPaused = false;
    }

    public void Pause()
    {
        OnPaused?.Invoke();
        
        for (int i = 0; i < closeWhenPaused.Length; i++)
        {
            closeFirstValue[i] = closeWhenPaused[i].activeSelf;
            closeWhenPaused[i].SetActive(false);
        }

        pauseMenuBackground.SetActive(true);
        Time.timeScale = 0f;

        GameIsPaused = true;
    }
    
    
}
