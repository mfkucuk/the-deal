using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;

public class TextTimer : MonoBehaviour
{
    public TMP_Text text;
    
    [SerializeField] private int startValue;
    [SerializeField] private Color color;
    
    [SerializeField] private Vector3 lastScale;
    [SerializeField] private float duration;

    public Action OnEnemyDo;

    private Vector3 defScale;
    private bool isZero;
    public int val;

    private bool doEnemy = false;

    void Start()
    {
        text.color = color;
        defScale = transform.localScale;
        val = startValue;
        text.text = startValue.ToString();
        
        if(Grid.Instance.GameStarted)
            StartCoroutine(CountDown());
        
        Grid.Instance.OnGameStarted += OnGameStarted;
    }

    private void OnDestroy()
    {
        Grid.Instance.OnGameStarted -= OnGameStarted;
    }
    public void OnGameStarted()
    {
        StartCoroutine(CountDown());
    }
    
    public void AddTime(int time)
    {
        val += time;
        text.text = val.ToString();
    }

    private IEnumerator CountDown()
    {
        WaitForSeconds wfs = new WaitForSeconds(1f);

        while (!isZero)
        {
            transform.DOScale(defScale, duration);
            yield return wfs;

            if (doEnemy)
            {
                doEnemy = false;
                OnEnemyDo?.Invoke();
            }

            transform.DOScale(lastScale, duration);
            text.text = (--val).ToString();

            if (val == 1)
            {
                doEnemy = true;
                val = startValue+1;
            }
            
        }
        
    }


}
