using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSteal : Abilities
{
    private int lifeStealRatio = 25;
    private int lifeStealAmount = 1;

    public int ind = 3;

    public override bool DoAbility(Enemy enemy)
    {
        int res = Random.Range(0, 101);

        if (res < lifeStealRatio)
        {
            Player.Instance.LifeSteal(lifeStealAmount);
            return true;
        }

        return false;
    }

    public override void LevelUp(Skill skill)
    {
        SkillData.Instance.SetAbilityData(3, level + 1, skill.skillIndex);
        level = SkillData.Instance.GetAbilityData(3, skill.skillIndex);

        lifeStealRatio = 20 + level * 5;
        lifeStealAmount = ((level + 1) / 2);
    }

    public override void ApplyLevel(Skill skill)
    {
        level = SkillData.Instance.GetAbilityData(3, skill.skillIndex);

        lifeStealRatio = 20 + level * 5;
        lifeStealAmount = ((level + 1) / 2);
    }
    public override int GetInd()
    {
        return ind;
    }

    public override string GetName()
    {
        return "Lifesteal";
    }

    public override string GetInfo(Skill skill)
    {
        level = SkillData.Instance.GetAbilityData(3, skill.skillIndex) + 1;

        if (level == 1)
        {
            return "Lifesteal Chance: " + (20 + level * 5) + "%\n" + "Lifesteal Amount: " + level;
        }
        else
        {
            return "Lifesteal Chance: " + (20 + (level-1) * 5) + "% -> " + (20 + level * 5) + "%\n" + "Lifesteal Amount: " + (((level) / 2)) + " -> " + ((level + 1) / 2);
        }
    }
}
