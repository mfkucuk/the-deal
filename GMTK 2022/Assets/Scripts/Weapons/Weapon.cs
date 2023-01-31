using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{  
    protected ERarity rarity;

    protected int damage;
    protected List<Abilities> abilities;
    
    public ERarity GetRarityLevel()
    { 
        return rarity;
    }
    
    public abstract void Init();

    public abstract void AttackEnemy();





}
