using System;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
        
    [SerializeField] private Transition transition;

    private void Awake()
    {
        SceneData.Instance.SetCurrSceneName("Inventory");
    }

    void Start()
    {

        StartCoroutine(transition.StartTransition());
        
        AudioManager.Instance.Play("InventoryTheme");
    }


}
