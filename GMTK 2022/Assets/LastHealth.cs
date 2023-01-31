using System;
using System.Collections;
using UnityEngine;
using TMPro;

public class LastHealth : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private TMP_Text lastHealthText;

    private bool check = false;

    private void Start()
    {
        Player.Instance.OnPlayerTakeDamage += OnPlayerTakeDamage;
        Grid.Instance.OnGameStarted += OnGameStarted;
    }

    private void OnDestroy()
    {
        Player.Instance.OnPlayerTakeDamage -= OnPlayerTakeDamage;
        Grid.Instance.OnGameStarted -= OnGameStarted;
    }

    private void OnGameStarted()
    {
        if (SkullData.Instance.GetHealtDatas() == 1)
        {
            check = true;
            StartCoroutine(BlinkText());
        }
        
        Grid.Instance.OnGameStarted -= OnGameStarted;
    }

    private void OnPlayerTakeDamage()
    {
        if (SkullData.Instance.GetHealtDatas() == 1 && !check)
        {
            check = true;
            StartCoroutine(BlinkText());
        }

        if (SkullData.Instance.GetHealtDatas() == 0)
        {
            this.gameObject.SetActive(false);
        }
    }

    private IEnumerator BlinkText()
    {
        WaitForSeconds wfs = new WaitForSeconds(duration);

        while (check)
        {
            lastHealthText.enabled = true;
            yield return wfs;
            lastHealthText.enabled = false;
            yield return wfs;

            if (SkullData.Instance.GetHealtDatas() != 1)
            {
                check = false;
                StopAllCoroutines();
            }
        }
    }
    
    
}
