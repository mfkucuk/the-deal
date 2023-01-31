using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Abilities
{
    protected int level = 1;
    public abstract bool DoAbility(Enemy enemy);
    public abstract void LevelUp(Skill skill);
    public abstract void ApplyLevel(Skill skill);

    public abstract int GetInd();
    public abstract string GetName();
    public abstract string GetInfo(Skill skill);
}
