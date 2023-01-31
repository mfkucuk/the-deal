using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;

public class Enemy5 : Enemy
{
    [SerializeField] private Enemy6 enemy6Prefab;
    
    [SerializeField] private CharacterMovement _characterMovement;

    [SerializeField] private ParticleSystem deadParticleEffect;
    [SerializeField] private Animator enemy5Animator;
    
    [SerializeField] private Animator enemyTakeDamageAnimator;
    [SerializeField] private TMP_Text damageText;

    private SpriteRenderer rend;

    private void Start()
    {
        enemyName = "Headless Horseman";
        maxHealth = health;
        turnToPlay = 1;
        isBoundToTurn = true;
        rend = GetComponent<SpriteRenderer>();
        damage = 1;

    }

    public override void MoveEnemy()
    {
        ArrayList freeTiles = new ArrayList();

        // Tiles enemy can go.
        for (int i = EnemyPos.x - 2; i <= EnemyPos.x + 2; i++)
        {
            for (int j = EnemyPos.y - 2; j <= EnemyPos.y + 2; j++)
            {
                if (i >= 0 && i < 8 && j >= 0 && j < 8)
                {
                    freeTiles.Add(Grid.Instance.getTileAtPosition(i, j));
                }
            }
        }

        // Remove occupied tiles.
        removeOccupiedTiles(freeTiles);

        if (freeTiles.Count == 0) { return; }

        ////////// Chase Logic
        Tile targetTile = null;
        float minDistance = float.MaxValue;

        foreach (Tile entry in freeTiles)
        {
            if (Mathf.Pow(entry.x - Player.Instance.PlayerPos.x, 2) + Mathf.Pow(entry.y - Player.Instance.PlayerPos.y, 2) <= minDistance)
            {
                minDistance = Mathf.Pow(entry.x - Player.Instance.PlayerPos.x, 2) + Mathf.Pow(entry.y - Player.Instance.PlayerPos.y, 2);
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
        Player.Instance.TakeDamage(damage);
        enemy5Animator.SetTrigger("Attack");
    }

    public override void TakeTurn()
    {
        // Check and change state depending on what happened.
        state = STATE.CHASE;

        for (int i = EnemyPos.x - 1; i <= EnemyPos.x + 1; i++)
        {
            for (int j = EnemyPos.y - 1; j <= EnemyPos.y + 1; j++)
            {
                if (i >= 0 && i < 8 && j >= 0 && j < 8)
                {
                    if (Grid.Instance.getTileAtPosition(i, j) == Player.Instance.PlayerPos)
                    {
                        state = STATE.ATTACK;
                        break;
                    }
                }
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
        
        if(randomDamage == 0)
            enemyTakeDamageAnimator.SetTrigger("TakeDamage1");
        else if(randomDamage == 1)
            enemyTakeDamageAnimator.SetTrigger("TakeDamage2");

        if (health <= 0)
        {
            EnemyInfo.Instance.hideInfoTiles();
            Transform();
            StartCoroutine(DeathAnimation(rend, deadParticleEffect, enemy5Animator));
            Grid.Instance.NumberOfEnemies--;

            if (Grid.Instance.NumberOfEnemies <= 0)
            {
                base.AllEnemyDied();
            }
        }
    }

    public void Transform()
    {
        Enemy newEnemy = Instantiate(enemy6Prefab, new Vector2((EnemyPos.x + TileHolder.Instance.x) * TileHolder.Instance.s, (EnemyPos.y + TileHolder.Instance.y) * TileHolder.Instance.s), Quaternion.identity);
        newEnemy.name = $"Enemy {EnemyPos.transform.position.x} {EnemyPos.transform.position.y}";
        newEnemy.EnemyPos = EnemyPos;
        newEnemy.turnToPlay = 2;
        Grid.Instance.enemyLocations.Remove(EnemyPos);
        Grid.Instance.enemyLocations[EnemyPos] = newEnemy;
        Grid.Instance.NumberOfEnemies++;
    }
}
