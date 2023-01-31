using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class EnemyInfo : MonoBehaviour
{
    private static EnemyInfo _instance;
    public static EnemyInfo Instance { get { return _instance; } }

    [SerializeField] private Tile[] infoTiles;
    [SerializeField] private GameObject[] enemyHearts;
    [SerializeField] private GameObject[] enemyDamages;
    [SerializeField] private GameObject[] turnLeft;
    [SerializeField] private GameObject _mimic;
    [SerializeField] private GameObject _wing;
    [SerializeField] private GameObject _hook;
    [SerializeField] private TMP_Text _enemyName;
    [SerializeField] private Shake shakeController;

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
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (!(i == 2 && j == 2))
                {
                    infoTiles[i * 5 + j].infoTilePos = infoTiles[i * 5 + j].transform.position;
                }
            }
        }
        gameObject.SetActive(false);
    }

    public void showInfoTiles(Enemy enemy)
    {
        if (enemy == null) return;

        gameObject.SetActive(true);
        _enemyName.gameObject.SetActive(true);
        _enemyName.SetText(enemy.enemyName);

        // Display hearts
        for (int i = 0; i < enemyHearts.Length; i++)
        {
            enemyHearts[i].SetActive(true);
        }

        for (int i = enemyHearts.Length - 1; i >= enemy.health; i--)
        {
            enemyHearts[i].SetActive(false);
        }

        // Display damage
        for (int i = 0; i < enemyDamages.Length; i++)
        {
            enemyDamages[i].SetActive(true);
        }

        for (int i = enemyDamages.Length - 1; i >= enemy.damage; i--)
        {
            enemyDamages[i].SetActive(false);
        }

        // Display how much turns left
        for (int i = 0; i < turnLeft.Length; i++)
        {
            turnLeft[i].SetActive(true);
        }

        for (int i = turnLeft.Length - 1; i >= enemy.turnToPlay; i--)
        {
            turnLeft[i].SetActive(false);
        }

        shakeController.mouseOn = false;

        // Display attack and movement patterns
        if (enemy is Enemy1)
        {
            // Attack
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (!(i == 2 && j == 2))
                    {
                        shakeController.StartMouseOnShaking(infoTiles[i * 5 + j].transform);
                        infoTiles[i * 5 + j].Hitbox.SetActive(true);
                    }
                }
            }

            // Movement
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (!(i == 2 && j == 2))
                    {
                        infoTiles[i * 5 + j].canTakeAction.SetActive(true);
                    }
                }
            }
        }

        if (enemy is Enemy2)
        {
            // Attack
            for (int i = 0; i < 5; i++)
            {
                if (i != 2)
                {
                    shakeController.StartMouseOnShaking(infoTiles[i * 5 + 2].transform);
                    infoTiles[i * 5 + 2].Hitbox.SetActive(true);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                if (i != 2)
                {
                    shakeController.StartMouseOnShaking(infoTiles[10 + i].transform);
                    infoTiles[10 + i].Hitbox.SetActive(true);
                }
            }

            // Movement
            infoTiles[2].canTakeAction.SetActive(true);
            infoTiles[2 * 5].canTakeAction.SetActive(true);
            infoTiles[2 * 5 + 4].canTakeAction.SetActive(true);
            infoTiles[4 * 5 + 2].canTakeAction.SetActive(true);
        }

        if (enemy is Enemy3)
        {
            infoTiles[3 * 5 + 2].canTakeAction.SetActive(true);
        }

        if (enemy is Enemy4)
        {
            // Attack
            int i = 0;
            int j = 0;
            for (; i < 5 ; i++, j++)
            {
                if (!(i == 2 && j == 2))
                {
                    shakeController.StartMouseOnShaking(infoTiles[i * 5 + j].transform);
                    infoTiles[i * 5 + j].Hitbox.SetActive(true);
                }
            }

            i = 4;
            j = 0;
            for (; i >= 0; i--, j++)
            {
                if (!(i == 2 && j == 2))
                {
                    shakeController.StartMouseOnShaking(infoTiles[i * 5 + j].transform);
                    infoTiles[i * 5 + j].Hitbox.SetActive(true);
                }
            }

            

            // Movement
            for (int x = 1; x <= 3; x++)
            {
                for (int y = 1; y <= 3; y++)
                {
                    if (!(x == 2 && y == 2))
                    {
                        infoTiles[x * 5 + y].canTakeAction.SetActive(true);
                    }
                }
            }
        }

        if (enemy is Enemy5)
        {
            // Attack
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (!(i == 2 && j == 2))
                    {
                        shakeController.StartMouseOnShaking(infoTiles[i * 5 + j].transform);
                        infoTiles[i * 5 + j].Hitbox.SetActive(true);
                    }
                }
            }

            // Movement
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (i == 0 || j == 0 || i == 4 || j == 4)
                    {
                        infoTiles[i * 5 + j].canTakeAction.SetActive(true);
                    }
                }
            }
        }

        if (enemy is Enemy6)
        {
            // Attack
            for (int x = 1; x <= 3; x++)
            {
                for (int y = 1; y <= 3; y++)
                {
                    if (!(x == 2 && y == 2))
                    {
                        shakeController.StartMouseOnShaking(infoTiles[x * 5 + y].transform);
                        infoTiles[x * 5 + y].Hitbox.SetActive(true);
                    }
                }
            }

            // Movement
            int i = 1;
            int j = 1;
            for (; i < 4; i++, j++)
            {
                if (!(i == 2 && j == 2))
                {
                    infoTiles[i * 5 + j].canTakeAction.SetActive(true);
                }
            }

            i = 3;
            j = 1;
            for (; i >= 1; i--, j++)
            {
                if (!(i == 2 && j == 2))
                {
                    infoTiles[i * 5 + j].canTakeAction.SetActive(true);
                }
            }
        }

        if (enemy is Enemy7)
        {
            // Movement
            infoTiles[1 * 5 + 2].canTakeAction.SetActive(true);
            infoTiles[2 * 5 + 1].canTakeAction.SetActive(true);
            infoTiles[2 * 5 + 3].canTakeAction.SetActive(true);
            infoTiles[3 * 5 + 2].canTakeAction.SetActive(true);
        }

        if (enemy is Enemy8)
        {
            // Attack
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (!(i == 2 && j == 2))
                    {
                        shakeController.StartMouseOnShaking(infoTiles[i * 5 + j].transform);
                        infoTiles[i * 5 + j].Hitbox.SetActive(true);
                    }
                }
            }

            // Movement
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (!(i == 2 && j == 2))
                    {
                        infoTiles[i * 5 + j].canTakeAction.SetActive(true);
                    }
                }
            }
        }

        if (enemy is Enemy9)
        {
            _hook.SetActive(true);

            // Attack
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (!(i == 2 && j == 2))
                    {
                        shakeController.StartMouseOnShaking(infoTiles[i * 5 + j].transform);
                        infoTiles[i * 5 + j].Hitbox.SetActive(true);
                    }
                }
            }

            // Movement
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (!(i == 2 && j == 2))
                    {
                        infoTiles[i * 5 + j].canTakeAction.SetActive(true);
                    }
                }
            }
        }

        if (enemy is Enemy10)
        {

        }

        if (enemy is Enemy11)
        {
            _wing.SetActive(true);

            // Movement
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (!(i == 2 && j == 2))
                    {
                        infoTiles[i * 5 + j].canTakeAction.SetActive(true);
                    }
                }
            }

            // Attack
            for (int i = 0; i < 5; i++)
            {
                if (i != 2)
                {
                    infoTiles[i * 5 + 2].danger.SetActive(true);
                }
            }

            for (int i = 0; i < 5; i++)
            {
                if (i != 2)
                {
                    infoTiles[10 + i].danger.SetActive(true);
                }
            }
        }

        if (enemy is Enemy12)
        {
            _mimic.SetActive(true);

            // Attack
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (!(i == 2 && j == 2))
                    {
                        shakeController.StartMouseOnShaking(infoTiles[i * 5 + j].transform);
                        infoTiles[i * 5 + j].Hitbox.SetActive(true);
                    }
                }
            }
        }

        if (enemy is DarkmageBoss)
        {
            var curHealth = 10;


            if (enemy.health > 20)
            {
                for (int i = enemyHearts.Length - 1; i >= (curHealth - (30-enemy.health)); i--)
                {
                    enemyHearts[i].SetActive(false);
                }

                // Attack
                for (int i = 0; i <= 4; i++)
                {
                    for (int j = 0; j <= 4; j++)
                    {
                        if (!(i == 2 && j == 2))
                        {
                            shakeController.StartMouseOnShaking(infoTiles[i * 5 + j].transform);
                            infoTiles[i * 5 + j].Hitbox.SetActive(true);
                        }
                    }
                }

                // Movement
                for (int i = 0; i <= 4; i++)
                {
                    for (int j = 0; j <= 4; j++)
                    {
                        if (!(i == 2 && j == 2))
                        {
                            infoTiles[i * 5 + j].canTakeAction.SetActive(true);
                        }
                    }
                }
            }
            else if (enemy.health > 10)
            {
                for (int i = enemyHearts.Length - 1; i >= (curHealth - (20 - enemy.health)); i--)
                {
                    enemyHearts[i].SetActive(false);
                }

                // Movement
                for (int i = 0; i <= 4; i++)
                {
                    for (int j = 0; j <= 4; j++)
                    {
                        if (i == 0 || i == 4 || j == 0 || j == 4)
                        {
                            infoTiles[i * 5 + j].canTakeAction.SetActive(true);
                        }
                    }
                }

                // Attack
                for (int i = 2; i <= 4; i++)
                {
                    for (int j = 2; j <= 4; j++)
                    {
                        infoTiles[i * 5 + j].danger.SetActive(true);
                    }
                }
            }
            else
            {
                for (int i = 0; i < enemyHearts.Length; i++)
                {
                    enemyHearts[i].SetActive(false);
                }

                // Attack
                for (int i = 1; i <= 3; i++)
                {
                    for (int j = 1; j <= 3; j++)
                    {
                        if (!(i == 2 && j == 2))
                        {
                            shakeController.StartMouseOnShaking(infoTiles[i * 5 + j].transform);
                            infoTiles[i * 5 + j].Hitbox.SetActive(true);
                        }
                    }
                }

                // Movement
                for (int i = 1; i <= 3; i++)
                {
                    for (int j = 1; j <= 3; j++)
                    {
                        if (!(i == 2 && j == 2))
                        {
                            infoTiles[i * 5 + j].canTakeAction.SetActive(true);
                        }
                    }
                }
            }
        }

        if (enemy is DarkmageBossClone)
        {
            for (int i = 0; i < enemyHearts.Length; i++)
            {
                enemyHearts[i].SetActive(false);
            }

            // Attack
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (!(i == 2 && j == 2))
                    {
                        shakeController.StartMouseOnShaking(infoTiles[i * 5 + j].transform);
                        infoTiles[i * 5 + j].Hitbox.SetActive(true);
                    }
                }
            }

            // Movement
            for (int i = 1; i <= 3; i++)
            {
                for (int j = 1; j <= 3; j++)
                {
                    if (!(i == 2 && j == 2))
                    {
                        infoTiles[i * 5 + j].canTakeAction.SetActive(true);
                    }
                }
            }
        }
    }

    public void hideInfoTiles()
    {
        gameObject.SetActive(true);
        _enemyName.gameObject.SetActive(false);
        shakeController.resetShaking();

        _hook.SetActive(false);
        _wing.SetActive(false);
        _mimic.SetActive(false);

        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (!(i == 2 && j == 2))
                {
                    infoTiles[i * 5 + j].canTakeAction.SetActive(false);
                    infoTiles[i * 5 + j].Hitbox.SetActive(false);
                    infoTiles[i * 5 + j].danger.SetActive(false);
                    infoTiles[i * 5 + j].transform.position = infoTiles[i * 5 + j].infoTilePos;
                }
            }
        }

        gameObject.SetActive(false);
    }
}
