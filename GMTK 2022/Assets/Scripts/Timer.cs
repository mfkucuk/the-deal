using UnityEngine;

using TMPro;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Timer : MonoBehaviour
{

    [SerializeField] private float timerCount = 1f;
    [SerializeField] private TMP_Text countDownText;
    
    [SerializeField] private Vector3 lastScale;
    [SerializeField] private Vector3 defScale;
    [SerializeField] private float duration;
        
    [SerializeField] private Color color;
    
    [SerializeField] private Shake shake;
    [SerializeField] private Transform cameraPos;

    [SerializeField] private SpriteRenderer spriteRendere;
    
    private int timeLeft;
    
    private bool noTimer;
    private bool timeCanInfinty;
    
    private int textVal;        
    
    private bool stopTimer = false;

    private void Start()
    {
        Enemy.OnAllEnemyDied += OnAllEnemyDied;
        
        timeCanInfinty = FightSceneManager.Instance.currLevelDifficulty.timerCanInfinty;

        if (timeCanInfinty)
        {
            int val = Random.Range(0, 4);

            if (val == 0)
                noTimer = true;
            else 
                noTimer = false;
        }
        else
            noTimer = false;

        if (!noTimer)
        {
            timeLeft = FightSceneManager.Instance.currLevelDifficulty.timer;
            countDownText.text = timeLeft.ToString();
        }
      
    }

    private void OnDestroy()
    {
        Enemy.OnAllEnemyDied -= OnAllEnemyDied;
    }

    private void OnAllEnemyDied()
    {
        stopTimer = true;
    }
    
    void Update()
    {
        if (!stopTimer)
        {
            if (noTimer)
            {
                spriteRendere.enabled = true;
            }
            else if (Grid.Instance.GameStarted)
            {
                timerCount -= Time.deltaTime;
        
                if(timerCount < 0)
                {
                    transform.DOScale(lastScale, duration);
            
                    timerCount = 1f;

                    textVal = int.Parse(countDownText.text) - 1;
                    countDownText.text = textVal.ToString();

                    if (textVal == 0)
                    {
                        StartCoroutine(Player.Instance.PlayerDeath());
                    }
                    else if (textVal == 10)
                        countDownText.color = color;
                    else if (textVal < 10)
                    {
                        if(textVal % 2 == 0)
                            AudioManager.Instance.Play("Clock1");
                        else
                            AudioManager.Instance.Play("Clock2");
                
                        shake.DoShake(cameraPos);
                    }
            
                    transform.DOScale(defScale, duration);
                }
            } 
        }


    }
    
    
    
}
