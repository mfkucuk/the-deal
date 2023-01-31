using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Enemy10 : Enemy
{
    [SerializeField] private CharacterMovement _characterMovement;
    [SerializeField] private ParticleSystem deadParticleEffect;
    [SerializeField] private Animator enemy10Animator;

    [SerializeField] private Animator enemyTakeDamageAnimator;
    [SerializeField] private TMP_Text damageText;

    private SpriteRenderer rend;
    private void Start()
    {
        Grid.Instance.OnGameStarted += OnGameStarted;
        enemyName = "Totem";
        health = 3;
        maxHealth = health;
        turnToPlay = 0;
        isBoundToTurn = false;
        rend = GetComponent<SpriteRenderer>();
        damage = 0;
    }

    private void OnDestroy()
    {
        Grid.Instance.OnGameStarted -= OnGameStarted;
    }
    public override void AttackPlayer()
    {
        // Enemy doesn't attack
    }

    public override void MoveEnemy()
    {
        // Enemy doesn't move
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
            foreach (KeyValuePair<Tile, Enemy> entry in Grid.Instance.enemyLocations)
            {
                entry.Value.damage--;
            }

            EnemyInfo.Instance.hideInfoTiles();
            StartCoroutine(DeathAnimation(rend, deadParticleEffect, enemy10Animator));
            Grid.Instance.enemyLocations.Remove(EnemyPos);
            Grid.Instance.NumberOfEnemies--;

            if (Grid.Instance.NumberOfEnemies <= 0)
            {
                base.AllEnemyDied();
            }
        }
    }

    public override void TakeTurn()
    {

    }

    private void OnGameStarted()
    {
        StartCoroutine(WaitForDamage());
    }
    
    IEnumerator WaitForDamage()
    {
        yield return new WaitForSeconds(2f);


        foreach (KeyValuePair<Tile, Enemy> entry in Grid.Instance.enemyLocations)
        {
            if (!(entry.Value is Enemy10))
                entry.Value.damage++;

        }

        damage = 0;
    }
}
