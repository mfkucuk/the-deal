using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : Skill
{

    ArrayList _patternTiles;
    public override void Start()
    {
        base.Start();

        _patternTiles = new ArrayList();

        skillName = "Sword";
        skillIndex = 0;
        skillType = TYPE.ATTACK;
        skillDesc = "Attacks in 1 tile in a 1 tile radius.";
        skillImage = GetComponent<SpriteRenderer>().sprite;
        attackPattern = Player.ATTACK_PATTERNS.SINGLE_STRIKE;
        movePattern = Player.MOVEMENT.NONE;


        rarity = 1;

        step = SkillData.Instance.GetAttackStepData(skillIndex);
        damage = SkillData.Instance.GetAttackDamageData(skillIndex);

        if (SkillData.Instance.GetAbilityData(SkillData.Instance.STUN_TIME, skillIndex) != 0) abilities.Add(new StunTime());
        if (SkillData.Instance.GetAbilityData(SkillData.Instance.STUN_TURN, skillIndex) != 0) abilities.Add(new Stun());
        if (SkillData.Instance.GetAbilityData(SkillData.Instance.RECOIL, skillIndex) != 0) abilities.Add(new Recoil());
        if (SkillData.Instance.GetAbilityData(SkillData.Instance.LIFESTEAL, skillIndex) != 0) abilities.Add(new LifeSteal());
        if (SkillData.Instance.GetAbilityData(SkillData.Instance.BLEEDING, skillIndex) != 0) abilities.Add(new Bleeding());

        for (int i = 0; i < abilities.Count; i++)
        {
            abilities[i].ApplyLevel(this);
            ability[abilities[i].GetInd()].SetActive(true);
            ability[abilities[i].GetInd()].transform.position = slots[i].transform.position;
        }
    }

    public override void ShowPattern(int x, int y, Shake shaker)
    {
        var hitTiles = new ArrayList();

        for (int i = Player.Instance.PlayerPos.x - 1; i <= Player.Instance.PlayerPos.x + 1; i++)
        {
            for (int j = Player.Instance.PlayerPos.y - 1; j <= Player.Instance.PlayerPos.y + 1; j++)
            {
                if (i >= 0 && i < 8 && j >= 0 && j < 8)
                {
                    hitTiles.Add(Grid.Instance.getTileAtPosition(i, j));
                }
            }
        }

        hitTiles.Remove(Player.Instance.PlayerPos);

        // Get 3 closest tile to the mouse.
        float minDistance = float.MaxValue;
        Tile mouseTile = Grid.Instance.getTileAtPosition(x, y);
        Tile targetTile = null;

        foreach (Tile entry in hitTiles)
        {
            if (Mathf.Pow(entry.x - mouseTile.x, 2) + Mathf.Pow(entry.y - mouseTile.y, 2) <= minDistance)
            {
                minDistance = Mathf.Pow(entry.x - mouseTile.x, 2) + Mathf.Pow(entry.y - mouseTile.y, 2);
                targetTile = entry;
            }
        }

        _patternTiles.Add(targetTile);
        hitTiles.Remove(targetTile);

        minDistance = float.MaxValue;

        foreach (Tile entry in hitTiles)
        {
            if (Mathf.Pow(entry.x - mouseTile.x, 2) + Mathf.Pow(entry.y - mouseTile.y, 2) <= minDistance)
            {
                minDistance = Mathf.Pow(entry.x - mouseTile.x, 2) + Mathf.Pow(entry.y - mouseTile.y, 2);
                targetTile = entry;
            }
        }

        _patternTiles.Add(targetTile);
        hitTiles.Remove(targetTile);

        minDistance = float.MaxValue;

        foreach (Tile entry in hitTiles)
        {
            if (Mathf.Pow(entry.x - mouseTile.x, 2) + Mathf.Pow(entry.y - mouseTile.y, 2) <= minDistance)
            {
                minDistance = Mathf.Pow(entry.x - mouseTile.x, 2) + Mathf.Pow(entry.y - mouseTile.y, 2);
                targetTile = entry;
            }
        }

        _patternTiles.Add(targetTile);
        hitTiles.Remove(targetTile);

        foreach (Tile tile in _patternTiles)
        {
            tile.Hitbox.SetActive(true);
            shaker.StartMouseOnShaking(tile.transform);
        }
    }

    public override void HidePattern(int x, int y)
    {
        foreach (Tile tile in _patternTiles)
        {
            tile.Hitbox.SetActive(false);
            tile.transform.localPosition = tile.tilePos;
        }

        _patternTiles.Clear();
    }
    public override void ImproveStep(int byThisValue)
    {
        base.ImproveStep(byThisValue);
        SkillData.Instance.SetAttackStepData(step, skillIndex);
    }

    public override void ImproveDamage(int byThisValue)
    {
        base.ImproveDamage(byThisValue);
        SkillData.Instance.SetAttackDamageData(damage, skillIndex);
    }
}
