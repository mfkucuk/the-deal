using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move1Skill : Skill
{
    public override void Start()
    {
        base.Start();

        skillName = "Move 1";
        skillIndex = 0;
        skillType = TYPE.MOVE;
        skillDesc = "Move 1 tile.";
        skillImage = GetComponent<SpriteRenderer>().sprite;
        attackPattern = Player.ATTACK_PATTERNS.NONE;
        movePattern = Player.MOVEMENT.MOVE1;

        rarity = 1;
    }

    public override void ShowPattern(int x, int y, Shake shaker)
    {

    }

    public override void HidePattern(int x, int y)
    {

    }
}
