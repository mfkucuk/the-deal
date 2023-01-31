using System.Collections;
using UnityEngine;
using System;
using Random = UnityEngine.Random;
using DG.Tweening;

public class Dice : MonoBehaviour {
    

    public Sprite[] diceSides;
    public SpriteRenderer rend;
    
    public int diceFaceAmount;
    [SerializeField] private float waitForEachDiceFace = 0.05f;
    
    [SerializeField] private float strength;

    [SerializeField] private bool isFightScene;

    public Action<int> OnDiceRolled;
    
    public Action OnDiceStartingRolled;

    private void Start()
    {
        if (isFightScene)
        {

        }       
        else
            diceFaceAmount = diceSides.Length;
    }

    public bool CanDice { get; set; } = true;
    public int Result { get;  set;}

    public IEnumerator RollTheDice()
    {
        OnDiceStartingRolled?.Invoke();
        
        CanDice = false;
        int randomDiceSide = 0;

        transform.DOShakeRotation(5, strength).OnComplete(() => { 
            transform.eulerAngles = Vector3.zero;});
        
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, diceFaceAmount);

            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(waitForEachDiceFace);

        }
        
        Result = randomDiceSide + 1;;

        CanDice = true;
        OnDiceRolled?.Invoke(Result);

    }


}
