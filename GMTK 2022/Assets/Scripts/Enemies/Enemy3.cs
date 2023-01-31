using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy3 : Enemy
{
    
    [SerializeField] private Enemy1 enemy1Prefab;
    [SerializeField] private Enemy2 enemy2Prefab;
    [SerializeField] private Enemy4 enemy4Prefab;

    [SerializeField] private CharacterMovement _characterMovement;

    [SerializeField] private ParticleSystem deadParticleEffect;
    [SerializeField] private Animator enemy3Animator;
    
    [SerializeField] private Animator enemyTakeDamageAnimator;
    [SerializeField] private TMP_Text damageText;
    
    private SpriteRenderer rend;
    
    private Enemy[] prefabs;
    private void Start()
    {
        prefabs = new Enemy[3];
        prefabs[0] = enemy1Prefab;
        prefabs[1] = enemy2Prefab;
        prefabs[2] = enemy4Prefab;

        enemyName = "Body Pile";
        maxHealth = health;
        turnToPlay = 1;
        isBoundToTurn = true;
        rend = GetComponent<SpriteRenderer>();

        damage = 0;
    }

    public override void MoveEnemy()
    {
        Tile targetTile = null;

        targetTile = Grid.Instance.getTileAtPosition(EnemyPos.x, EnemyPos.y - 1);

        if (targetTile == null) return;

        foreach (KeyValuePair<Tile, Enemy> entry in Grid.Instance.enemyLocations)
        {
            if (targetTile == entry.Key)
            {
                return;
            }
        }

        if (targetTile == Player.Instance.PlayerPos)
        {
            return;
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
        /* This enemy does not attack the player. */
    }

    public override void TakeTurn()
    {
        turnToPlay--;

        if (turnToPlay == 0)
        {
            EnemyInfo.Instance.hideInfoTiles();
            MoveEnemy();
            turnToPlay = 1;
        }

        Transform();
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
            StartCoroutine(DeathAnimation(rend, deadParticleEffect, enemy3Animator));
            Grid.Instance.enemyLocations.Remove(EnemyPos);
            Grid.Instance.NumberOfEnemies--;

            if (Grid.Instance.NumberOfEnemies <= 0)
            {
                base.AllEnemyDied();
            }
        }
    }

    public void Transform()
    {
        if (EnemyPos == null || health == 0) return;

        if (EnemyPos.y == 0 && health > 0)
        {
            Enemy newEnemy = Instantiate(prefabs[Random.Range(0, 3)], new Vector2((EnemyPos.x + TileHolder.Instance.x) * TileHolder.Instance.s, (EnemyPos.y + TileHolder.Instance.y) * TileHolder.Instance.s), Quaternion.identity);
            newEnemy.name = $"Enemy {EnemyPos.transform.position.x} {EnemyPos.transform.position.y}";
            newEnemy.EnemyPos = EnemyPos;
            newEnemy.turnToPlay = 2;
            Grid.Instance.enemyLocations.Remove(EnemyPos);
            Grid.Instance.enemyLocations[EnemyPos] = newEnemy;
            Destroy(this.gameObject);
        }
    }
}
