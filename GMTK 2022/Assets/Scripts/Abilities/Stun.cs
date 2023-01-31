using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : Abilities
{
    public int stunChance = 25;
    private int stunTurnAmount = 1;

    public int ind = 1;
    public override bool DoAbility(Enemy enemy)
    {
        int res = Random.Range(0, 101);

        if (res < stunChance)
        {
            enemy.turnToPlay += stunTurnAmount;
            return true;
        }

        return false;
    }

    public override void LevelUp(Skill skill)
    {
        SkillData.Instance.SetAbilityData(1, level + 1, skill.skillIndex);
        level = SkillData.Instance.GetAbilityData(1, skill.skillIndex);

        stunChance = 20 + level * 5;
        stunTurnAmount = level;
    }
    public override void ApplyLevel(Skill skill)
    {
        level = SkillData.Instance.GetAbilityData(1, skill.skillIndex);

        stunChance = 20 + level * 5;
        stunTurnAmount = ((level + 1) / 2);
    }

    public override int GetInd()
    {
        return ind;
    }

    public override string GetName()
    {
        return "Stun Turn";
    }

    public override string GetInfo(Skill skill)
    {
        level = SkillData.Instance.GetAbilityData(1, skill.skillIndex) + 1;

        if (level == 1)
        {
            return "Stun Chance: " + (20 + level * 5) + "%\n" + "Stun Turn Amount: " + level;
        }
        else
        {
            return "Stun Chance: " + (20 + (level - 1) * 5) + "% -> " + (20 + level * 5) + "%\n" + "Stun Turn Amount: " + (((level) / 2)) + " -> " + ((level + 1) / 2);
        }
    }
}
