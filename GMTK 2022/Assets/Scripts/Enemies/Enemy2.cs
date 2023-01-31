using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy2 : Enemy
{
    
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private ParticleSystem deadParticleEffect; 
    [SerializeField] private Animator enemy2Animator;
    
    [SerializeField] private Animator enemyTakeDamageAnimator;
    [SerializeField] private TMP_Text damageText;
    
    private SpriteRenderer rend;

    private void Start()
    {
        enemyName = "Minatour";
        maxHealth = health;
        turnToPlay = 1;
        isBoundToTurn = true;
        rend = GetComponent<SpriteRenderer>();

        damage = 1;
    }

    public override void MoveEnemy()
    {

        ArrayList freeTiles = new ArrayList();
        int px = Player.Instance.PlayerPos.x;
        int py = (int)Player.Instance.PlayerPos.y;

        freeTiles.Add(Grid.Instance.getTileAtPosition(px, EnemyPos.y));
        freeTiles.Add(Grid.Instance.getTileAtPosition(EnemyPos.x, py));


        // Remove occupied tiles
        removeOccupiedTiles(freeTiles);

        if (freeTiles.Count == 0) { return; }


        Tile targetTile = null;

        if (freeTiles.Count != 0)
        {
            if (freeTiles.Count == 1)
            {
                targetTile = (Tile)freeTiles[0];
            }
            else
            {
                targetTile = (Tile)freeTiles[Random.Range(0, 2)];
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
        Tile targetVector = null;
        if (EnemyPos.x == Player.Instance.PlayerPos.x)
        {
            if (EnemyPos.y > Player.Instance.PlayerPos.y)
            {
                targetVector = Grid.Instance.getTileAtPosition(Player.Instance.PlayerPos.x, (Player.Instance.PlayerPos.y + 1));
            }
            else
            {
                targetVector = Grid.Instance.getTileAtPosition(Player.Instance.PlayerPos.x, (Player.Instance.PlayerPos.y - 1));
            }
        }
        else if (EnemyPos.y == Player.Instance.PlayerPos.y)
        {
            if (EnemyPos.x > Player.Instance.PlayerPos.x)
            {
                targetVector = Grid.Instance.getTileAtPosition(Player.Instance.PlayerPos.x + 1, (Player.Instance.PlayerPos.y));
            }
            else
            {
                targetVector = Grid.Instance.getTileAtPosition(Player.Instance.PlayerPos.x - 1, (Player.Instance.PlayerPos.y));
            }
        }

        foreach (KeyValuePair<Tile, Enemy> entry in Grid.Instance.enemyLocations)
        {
            if (targetVector == entry.Key && !entry.Value.EnemyPos.Equals(EnemyPos))
            {
                return;
            }
        }

        Grid.Instance.enemyLocations.Remove(EnemyPos);
        Grid.Instance.enemyLocations[targetVector] = this;

        Vector2 jumpVector = new Vector2((targetVector.x + TileHolder.Instance.x) * TileHolder.Instance.s,
                            (targetVector.y + TileHolder.Instance.y) * TileHolder.Instance.s);
        _characterMovement.Jump(transform, jumpVector, "JumpVoice");
       // pos.localPosition = targetVector.tilePos;
        EnemyPos = targetVector;

        Player.Instance.TakeDamage(damage);
        enemy2Animator.SetTrigger("Attack");
    }

    public override void TakeTurn()
    {
        if (EnemyPos.x == Player.Instance.PlayerPos.x || EnemyPos.y == Player.Instance.PlayerPos.y)
        {
            state = STATE.ATTACK;
        }
        else
        {
            state = STATE.CHASE;
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
            StartCoroutine(DeathAnimation(rend, deadParticleEffect, enemy2Animator));
            Grid.Instance.enemyLocations.Remove(EnemyPos);
            Grid.Instance.NumberOfEnemies--;

            if (Grid.Instance.NumberOfEnemies <= 0)
            {
                base.AllEnemyDied();
            }
        }
    }
}
