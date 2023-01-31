using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class Enemy9 : Enemy
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private ParticleSystem deadParticleEffect;
    [SerializeField] private Animator enemy9Animator;

    [SerializeField] private Animator enemyTakeDamageAnimator;
    [SerializeField] private TMP_Text damageText;

    [SerializeField] private GameObject[] hooks;

    [SerializeField] private Transform hook1Pos;
    [SerializeField] private Transform hook2Pos;

    private SpriteRenderer rend;

    private void Start()
    {
        enemyName = "Executor";
        maxHealth = health;
        health = 4;
        turnToPlay = 1;
        isBoundToTurn = true;
        rend = GetComponent<SpriteRenderer>();
        damage = 2;
    }

    public override void MoveEnemy()
    {

        ArrayList freeTiles = new ArrayList();

        // Tiles enemy can go.
        for (int i = EnemyPos.x - 1; i <= EnemyPos.x + 1; i++)
        {
            for (int j = EnemyPos.y - 1; j <= EnemyPos.y + 1; j++)
            {
                if (i >= 0 && i < 8 && j >= 0 && j < 8)
                {
                    freeTiles.Add(Grid.Instance.getTileAtPosition(i, j));
                }
            }
        }


        // Remove occupied tiles
        removeOccupiedTiles(freeTiles);

        if (freeTiles.Count == 0) { return; }

        // Chase Logic
        Tile targetSide = null;
        float minDistanceDiagonal = float.MaxValue;

        foreach (Tile entry in Player.Instance.sides)
        {
            if (Mathf.Pow(EnemyPos.x - entry.x, 2) + Mathf.Pow(EnemyPos.y - entry.y, 2) <= minDistanceDiagonal)
            {
                minDistanceDiagonal = Mathf.Pow(EnemyPos.x - entry.x, 2) + Mathf.Pow(EnemyPos.y - entry.y, 2);
                targetSide = entry;
            }
        }


        Tile targetTile = null;
        float minDistance = float.MaxValue;
        foreach (Tile entry in freeTiles)
        {
            if (Mathf.Pow(entry.x - targetSide.x, 2) + Mathf.Pow(entry.y - targetSide.y, 2) <= minDistance)
            {
                minDistance = Mathf.Pow(entry.x - targetSide.x, 2) + Mathf.Pow(entry.y - targetSide.y, 2);
                targetTile = entry;
            }
        }

        Grid.Instance.enemyLocations.Remove(EnemyPos);
        Grid.Instance.enemyLocations[targetTile] = this;

        Vector2 jumpVector = new Vector2((targetTile.x + TileHolder.Instance.x) * TileHolder.Instance.s,
                            (targetTile.y + TileHolder.Instance.y) * TileHolder.Instance.s);
        
        if (bleedingCount > 0)
        {
            AudioManager.Instance.Play("EnemyTakeDamage");

            damageText.text = "1";

            int randomDamage = Random.Range(0, 2);

            if (randomDamage == 0)
                enemyTakeDamageAnimator.SetTrigger("TakeDamage1");
            else if (randomDamage == 1)
                enemyTakeDamageAnimator.SetTrigger("TakeDamage2");

            TakeDamage(1);
            bleedingCount--;
        }
        
        _characterMovement.Jump(transform, jumpVector, "JumpVoice");
        //pos.localPosition = targetTile.tilePos;
        if (EnemyPos != null) EnemyPos = targetTile;
    }

    public override void AttackPlayer()
    {
        bool isNearPlayer = false;

        for (int i = EnemyPos.x - 1; i <= EnemyPos.x + 1; i++)
        {
            for (int j = EnemyPos.y - 1; j <= EnemyPos.y + 1; j++)
            {
                if (i >= 0 && i < 8 && j >= 0 && j < 8)
                {
                    if (Grid.Instance.getTileAtPosition(i, j) == Player.Instance.PlayerPos)
                    {
                        isNearPlayer = true;
                        break;
                    }
                }
            }
        }

        if (isNearPlayer)
        {
            Player.Instance.TakeDamage(damage - 1);
        }
        else
        {
            Tile targetTile = null;
            if (EnemyPos.x == Player.Instance.PlayerPos.x)
            {
                if (EnemyPos.y > Player.Instance.PlayerPos.y)
                {
                    targetTile = Grid.Instance.getTileAtPosition(EnemyPos.x, (EnemyPos.y - 1));
                }
                else
                {
                    targetTile = Grid.Instance.getTileAtPosition(EnemyPos.x, (EnemyPos.y + 1));
                }
            }
            else if (EnemyPos.y == Player.Instance.PlayerPos.y)
            {
                if (EnemyPos.x > Player.Instance.PlayerPos.x)
                {
                    targetTile = Grid.Instance.getTileAtPosition(EnemyPos.x - 1, (EnemyPos.y));
                }
                else
                {
                    targetTile = Grid.Instance.getTileAtPosition(EnemyPos.x + 1, (EnemyPos.y));
                }
            }

            foreach (KeyValuePair<Tile, Enemy> entry in Grid.Instance.enemyLocations)
            {
                if (targetTile == entry.Key && !entry.Value.EnemyPos.Equals(EnemyPos))
                {
                    return;
                }
            }

            StartCoroutine(Hook(targetTile));
            
        }
    }

    IEnumerator Hook(Tile targetTile)
    {
        if (Math.Abs(transform.position.x - Player.Instance.currentPos.x) < 0.15f)
        {
            if (transform.position.y > Player.Instance.transform.position.y)
            {
                hooks[0].transform.eulerAngles = new Vector3(0f, 0f, 90f);
                hooks[1].transform.eulerAngles = new Vector3(0f, 0f, 90f);
            }
            else
            {
                hooks[0].transform.eulerAngles = new Vector3(0f, 0f, 270f);
                hooks[1].transform.eulerAngles = new Vector3(0f, 0f, 270f);
            }
            
        }
        else
        {
            if (transform.position.x > Player.Instance.transform.position.x)
            {
                hooks[0].transform.eulerAngles = new Vector3(0f, 0f, 0f);
                hooks[1].transform.eulerAngles = new Vector3(0f, 0f, 0f);
            }
            else
            {
                hooks[0].transform.eulerAngles = new Vector3(0f, 0f, 180f);
                hooks[1].transform.eulerAngles = new Vector3(0f, 0f, 180f);
            }
        }

        enemy9Animator.SetBool("Hook", true);
        
        yield return new WaitForSeconds(0.3f);
        
        hooks[0].SetActive(true);
        hooks[1].SetActive(true);
        hooks[0].GetComponent<TrailRenderer>().enabled = true;
        hooks[1].GetComponent<TrailRenderer>().enabled = true;

        hooks[0].transform.DOMove(Player.Instance.transform.position, 0.6f);
        hooks[1].transform.DOMove(Player.Instance.transform.position, 0.6f);
        
        yield return new WaitForSeconds(0.6f);

        Vector2 jumpVector = new Vector2((targetTile.x + TileHolder.Instance.x) * TileHolder.Instance.s,
            (targetTile.y + TileHolder.Instance.y) * TileHolder.Instance.s);
        _characterMovement.Jump(Player.Instance.transform, jumpVector, "JumpVoice");

        Player.Instance.TakeDamage(damage);

        Player.Instance.PlayerPos = targetTile;
        Player.Instance.CalculateAllDiagonalsOfPlayer(targetTile.x, targetTile.y, 8);
        Player.Instance.CalculateAllSidesOfPlayer();

        hooks[0].GetComponent<TrailRenderer>().enabled = false;
        hooks[1].GetComponent<TrailRenderer>().enabled = false;

        hooks[0].transform.DOMove(hook1Pos.position, 0.5f);
        hooks[1].transform.DOMove(hook2Pos.position, 0.5f);
        
        yield return new WaitForSeconds(0.5f);
        
        hooks[0].SetActive(false);
        hooks[1].SetActive(false);
        enemy9Animator.SetBool("Hook", false);

    }

    public override void TakeTurn()
    {
        if (EnemyPos.x == Player.Instance.PlayerPos.x || EnemyPos.y == Player.Instance.PlayerPos.y)
        {
            state = STATE.ATTACK;
        }
        else
        {
            bool isNearPlayer = false;

            for (int i = EnemyPos.x - 1; i <= EnemyPos.x + 1; i++)
            {
                for (int j = EnemyPos.y - 1; j <= EnemyPos.y + 1; j++)
                {
                    if (i >= 0 && i < 8 && j >= 0 && j < 8)
                    {
                        if (Grid.Instance.getTileAtPosition(i, j) == Player.Instance.PlayerPos)
                        {
                            isNearPlayer = true;
                            break;
                        }
                    }
                }
            }

            if (isNearPlayer)
            {
                state = STATE.ATTACK;
            }
            else
            {
                state = STATE.CHASE;
            }
        }

        turnToPlay--;

        if (Grid.Instance.getTileAtPosition(EnemyPos.x, EnemyPos.y).highlight.activeSelf == true)
        {
            EnemyInfo.Instance.hideInfoTiles();
            EnemyInfo.Instance.showInfoTiles(this);
        }

        if (turnToPlay == 0)
        {
            if (state == STATE.CHASE)
            {
                EnemyInfo.Instance.hideInfoTiles();
                MoveEnemy();
            }
            else
            {
                AttackPlayer();
            }

            turnToPlay = 2;
        }
    }

    public override void TakeDamage(int damage)
    {
        if (health <= 0) return;
        health -= damage;

        AudioManager.Instance.Play("EnemyTakeDamage");

        damageText.text = damage.ToString();

        int randomDamage = Random.Range(0, 2);

        if (randomDamage == 0)
            enemyTakeDamageAnimator.SetTrigger("TakeDamage1");
        else if (randomDamage == 1)
            enemyTakeDamageAnimator.SetTrigger("TakeDamage2");

        if (health <= 0)
        {
            EnemyInfo.Instance.hideInfoTiles();
            StartCoroutine(DeathAnimation(rend, deadParticleEffect, enemy9Animator));
            Grid.Instance.enemyLocations.Remove(EnemyPos);
            Grid.Instance.NumberOfEnemies--;

            if (Grid.Instance.NumberOfEnemies <= 0)
            {
                base.AllEnemyDied();
            }
        }
    }
}
