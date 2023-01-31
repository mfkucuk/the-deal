using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turn_Controller : MonoBehaviour
{
    
    Queue<Enemy> enemyTurns;
    private static Turn_Controller _instance;
    public static Turn_Controller Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Start()
    {
        Player.Instance.OnPlayerDeath += OnPlayerDeath;
        
        TurnState = TURN_STATE.NONE;
        enemyTurns = new Queue<Enemy>();
    }

    private void OnDestroy()
    {
        Player.Instance.OnPlayerDeath -= OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        StopAllCoroutines();
    }


    public TURN_STATE TurnState;
    public enum TURN_STATE
    {
        PLAYER_TURN = 0,
        ENEMY_TURN = 1,
        NONE = 2
    }

    public void CarryOutEnemyTurns()
    {
        foreach (KeyValuePair<Tile, Enemy> en in Grid.Instance.enemyLocations)
        {
            if (en.Value.isBoundToTurn)
                enemyTurns.Enqueue(en.Value);
        }

        StartCoroutine(Turn());
    }

    IEnumerator Turn()
    {
        float f = 1f / (Grid.Instance.NumberOfEnemies + 1);
        WaitForSeconds wfsrt = new WaitForSeconds(f);

        while (enemyTurns.Count != 0)
        {
            yield return wfsrt;
            Enemy e = null;
            e = enemyTurns.Dequeue();
            if (e != null) e.TakeTurn();
        }

        TurnState = TURN_STATE.NONE;
    }
}
