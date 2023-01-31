using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public enum STATE
    {
        CHASE = 0,
        ATTACK = 1
    }

    public string enemyName;
    [HideInInspector] public Tile EnemyPos;
    public int health;
    public int maxHealth;
    public int damage;
    public int turnToPlay;
    public STATE state;
    public bool isBoundToTurn;

    public Animator[] _abilityAnims;

    public static Action OnAllEnemyDied;

    public int bleedingCount = 0;

    private CharacterMovement _cm;
    public void removeOccupiedTiles(ArrayList freeTiles)
    {
        // Occupied by another enemy
        foreach (KeyValuePair<Tile, Enemy> entry in Grid.Instance.enemyLocations)
        {
            if (freeTiles.Contains(entry.Key))
            {
                freeTiles.Remove(entry.Key);
            }
        }

        // Occupied by the player.
        if (freeTiles.Contains(Player.Instance.PlayerPos))
        {
            freeTiles.Remove(Player.Instance.PlayerPos);
        }
    }

    public abstract void MoveEnemy();
    public abstract void AttackPlayer();
    public abstract void TakeTurn();
    public abstract void TakeDamage(int damage);
    
    public virtual void Recoil(int recoilSquareAmount)
    {
        _cm = FindObjectOfType<CharacterMovement>();

        Tile recoilTile = null;
        float maxDistance = float.MinValue;

        ArrayList tiles = new ArrayList();

        for (int i = EnemyPos.x - recoilSquareAmount; i <= EnemyPos.x + recoilSquareAmount; i++)
        {
            for (int j = EnemyPos.y - recoilSquareAmount; j <= EnemyPos.y + recoilSquareAmount; j++)
            {
                if (i >= 0 && i < 8 && j >= 0 && j < 8)
                {
                    tiles.Add(Grid.Instance.getTileAtPosition(i, j));
                }
            }
        }

        removeOccupiedTiles(tiles);

        foreach (Tile entry in tiles)
        {
            if (Mathf.Pow(entry.x - Player.Instance.transform.position.x, 2) + Mathf.Pow(entry.y - Player.Instance.transform.position.y, 2) >= maxDistance)
            {
                maxDistance = Mathf.Pow(entry.x - Player.Instance.transform.position.x, 2) + Mathf.Pow(entry.y - Player.Instance.transform.position.y, 2);
                recoilTile = entry;
            }
        }

        if (recoilTile == null)
        {
            return;
        }
        else
        {
            Grid.Instance.enemyLocations.Remove(EnemyPos);
            Grid.Instance.enemyLocations[recoilTile] = this;

            Vector2 jumpVector = new Vector2((recoilTile.x + TileHolder.Instance.x) * TileHolder.Instance.s,
                            (recoilTile.y + TileHolder.Instance.y) * TileHolder.Instance.s);

            _cm.Jump(transform, jumpVector, "JumpVoice");
            EnemyPos = recoilTile;
        }
    }

    public virtual void AllEnemyDied()
    {
        OnAllEnemyDied?.Invoke();
    }

    public virtual IEnumerator DeathAnimation(SpriteRenderer rend, ParticleSystem ps, Animator a, GameObject countDown = null)
    {
        AudioManager.Instance.Play("EnemyDead");

        if (countDown != null)
            countDown.SetActive(false);
        
        a.SetTrigger("Dead");
        ps.Play();
        
        yield return new WaitForSeconds(1f);
        
        rend.enabled = false;

        yield return new WaitForSeconds(1f);

        Destroy(this.gameObject);
    }

    public IEnumerator PlayAbilityAnimation(Animator a)
    {
        if (a != null) a.gameObject.SetActive(true);

        if (a != null) a.Play("Abilities");

        yield return new WaitForSeconds(1.5f);

        if (a != null) a.gameObject.SetActive(false);
    }

    public void RemoveEnemy()
    {
        Grid.Instance.enemyLocations.Remove(EnemyPos);
        Grid.Instance.NumberOfEnemies--;
    }
}
