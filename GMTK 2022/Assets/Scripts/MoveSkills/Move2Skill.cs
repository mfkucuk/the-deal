using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move2Skill : Skill
{
    public override void Start()
    {
        base.Start();

        skillName = "Move 2";
        skillIndex = 1;
        skillType = TYPE.MOVE;
        skillDesc = "Move 2 tiles.";
        skillImage = GetComponent<SpriteRenderer>().sprite;
        attackPattern = Player.ATTACK_PATTERNS.NONE;
        movePattern = Player.MOVEMENT.MOVE2;

        rarity = 1;
    }

    public override void ShowPattern(int x, int y, Shake shaker)
    {

    }

    public override void HidePattern(int x, int y)
    {

    }
}
