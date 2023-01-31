using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedTeleportSkill : Skill
{
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        skillName = "Cursed Teleport";
        skillIndex = 7;
        skillType = TYPE.MOVE;
        skillDesc = "Teleport to one edge of the board.";
        skillImage = GetComponent<SpriteRenderer>().sprite;
        attackPattern = Player.ATTACK_PATTERNS.NONE;
        movePattern = Player.MOVEMENT.CURSED_TELEPORT;

        rarity = 3;
    }
    public override void HidePattern(int x, int y)
    {

    }

    public override void ShowPattern(int x, int y, Shake shaker)
    {

    }
}
