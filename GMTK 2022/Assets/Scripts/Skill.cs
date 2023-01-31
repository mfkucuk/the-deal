using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    // UI Related
    public Vector3 originalPos;
    public Vector3 dragOffset;
    public Vector3 scaleVector;
    public Vector3 originalScale;
    private bool isDrag;
    public GameObject[] slots;
    public GameObject[] ability;
    public bool clicked;
    public bool hovered;

    // Skill description
    public string skillName;
    public int skillIndex;
    public TYPE skillType;
    public Player.ATTACK_PATTERNS attackPattern;
    public Player.MOVEMENT movePattern;
    public string skillDesc;
    public Sprite skillImage;

    public int step;
    public int damage;

    public int rarity;

    public List<Abilities> abilities = new List<Abilities>();

    private Shake shakeController;

    private Transition transition;

    public static bool isInactive { get; set; } = false;

    public enum TYPE
    {
        MOVE = 0,
        ATTACK
    }

    public virtual void Start()
    {
        shakeController = FindObjectOfType<Shake>();
        transition = FindObjectOfType<Transition>();
        
        isDrag = false;
        originalPos = transform.position;
        originalScale = transform.lossyScale;
        clicked = false;
        hovered = false;
    }

    private void OnMouseEnter()
    {
        if (!transition.TransitionStarted)
        {
            shakeController.StartMouseOnShaking(transform);
            AudioManager.Instance.Play("InventoryHover");
            if (InventoryData.Instance.GetInventoryData() == 1)
            {
                skillDesc = GetSkillData();
            
                // Show the skill description.
                InventoryManager.Instance.ShowSkillInfo(this, skillImage, skillDesc);
            }
            else
            {
                if(!PuaseMenu.GameIsPaused)
                {
                    hovered = true;
                    FightSceneBackground.Instance.HoverSkill();
                }
            }
        }
    }

    private void OnMouseExit()
    {
        shakeController.resetShaking();
        if (InventoryData.Instance.GetInventoryData() == 1)
        {
            if (!isDrag) transform.position = originalPos;

            // Hide the skill description.
            InventoryManager.Instance.HideSkillInfo();
        }
        else
        {
            if (!PuaseMenu.GameIsPaused)
            {
                hovered = false;
                FightSceneBackground.Instance.UnhoverSkill();
            }
            
        }
    }

    private void OnMouseDown()
    {
        if (!transition.TransitionStarted && !isInactive)
        {
            shakeController.resetShaking();
            if (InventoryData.Instance.GetInventoryData() == 1)
            {
            
                dragOffset = transform.position - GetMousePosition();
                isDrag = true;

                // Hide the skill description.
                InventoryManager.Instance.HideSkillInfo();
            }
            else
            {
                if (!PuaseMenu.GameIsPaused)
                {
                    AudioManager.Instance.Play("SelectSkill");
                    clicked = true;
                    FightSceneBackground.Instance.SelectSkill();
                }

            }
        }

    }

    private void OnMouseDrag()
    {
        if (!transition.TransitionStarted)
        {
            if (InventoryData.Instance.GetInventoryData() == 1)
            {
                isDrag = true;
                shakeController.resetShaking();
                transform.position = GetMousePosition() + dragOffset;

                InventoryManager.Instance.HideSkillInfo();
            }
        }
        
    }

    private Vector3 GetMousePosition()
    {
        Vector3 move = new Vector3();
        transform.DOScale(scaleVector, .2f);

        move.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        move.y = Camera.main.ScreenToWorldPoint(Input.mousePosition).y;
        move.z = 0;

        return move;
    }

    private void OnMouseUp()
    {
        if (!transition.TransitionStarted)
        {
            if (InventoryData.Instance.GetInventoryData() == 1)
            {
                isDrag = false;
                transform.DOScale(originalScale, .2f);

                for (int i = 0; i < InventoryManager.Instance.moveDiceFaces.Length; i++)
                {
                    if (skillType == TYPE.MOVE)
                    {
                        if (!InventoryManager.Instance.moveDiceFaces[i].full && InventoryManager.Instance.moveDiceFaces[i].put && InventoryManager.Instance.moveSkills.Contains(this))
                        {
                            transform.position = InventoryManager.Instance.moveDiceFaces[i].transform.position;
                            originalPos = InventoryManager.Instance.moveDiceFaces[i].transform.position;
                            InventoryManager.Instance.moveDiceFaces[i].full = true;
                            InventoryManager.Instance.moveDiceFaces[i].activeSkill = this;
                            InventoryManager.Instance.OnSkillEquiped(this);
                            InventoryData.Instance.SetMoveSkillPlaced(i, skillIndex);
                        }
                        else if (InventoryManager.Instance.moveDiceFaces[i].full && !InventoryManager.Instance.moveDiceFaces[i].put && this == InventoryManager.Instance.moveDiceFaces[i].activeSkill)
                        {
                            InventoryManager.Instance.moveDiceFaces[i].full = false;
                            InventoryManager.Instance.moveDiceFaces[i].activeSkill = null;
                            InventoryManager.Instance.OnSkillRemoved(this);
                            InventoryData.Instance.SetMoveSkillPlaced(-1, skillIndex);
                        }
                        else if (InventoryManager.Instance.moveDiceFaces[i].full && InventoryManager.Instance.moveDiceFaces[i].put && this == InventoryManager.Instance.moveDiceFaces[i].activeSkill)
                        {
                            transform.position = originalPos;
                        }
                    }
                }

                for (int i = 0; i < InventoryManager.Instance.attackDiceFaces.Length; i++)
                {
                    if (skillType == TYPE.ATTACK)
                    {
                        if (!InventoryManager.Instance.attackDiceFaces[i].full && InventoryManager.Instance.attackDiceFaces[i].put && InventoryManager.Instance.attackSkills.Contains(this))
                        {
                            transform.position = InventoryManager.Instance.attackDiceFaces[i].transform.position;
                            originalPos = InventoryManager.Instance.attackDiceFaces[i].transform.position;
                            InventoryManager.Instance.attackDiceFaces[i].full = true;
                            InventoryManager.Instance.attackDiceFaces[i].activeSkill = this;
                            InventoryManager.Instance.OnSkillEquiped(this);
                            InventoryData.Instance.SetAttackSkillPlaced(i, skillIndex);
                        }
                        else if (InventoryManager.Instance.attackDiceFaces[i].full && !InventoryManager.Instance.attackDiceFaces[i].put && this == InventoryManager.Instance.attackDiceFaces[i].activeSkill)
                        {
                            InventoryManager.Instance.attackDiceFaces[i].full = false;
                            InventoryManager.Instance.attackDiceFaces[i].activeSkill = null;
                            InventoryManager.Instance.OnSkillRemoved(this);
                            InventoryData.Instance.SetAttackSkillPlaced(-1, skillIndex);
                        }
                        else if (InventoryManager.Instance.attackDiceFaces[i].full && InventoryManager.Instance.attackDiceFaces[i] && this == InventoryManager.Instance.attackDiceFaces[i].activeSkill)
                        {
                            transform.position = originalPos;
                        }
                    }
                }

                transform.DOMove(originalPos, 0.2f);
            }
        }
        
    }

    public string GetSkillData()
    {
        string result = "";

        // Rarity 

        // Damage
        if (skillType == TYPE.ATTACK)
        {
            result += "Damage: " + SkillData.Instance.GetAttackDamageData(skillIndex) + "\n";
        }

        // Abilities
        foreach (Abilities ability in abilities)
        {
            result += ability.GetName() + ": Level " + SkillData.Instance.GetAbilityData(ability.GetInd(), skillIndex) + "\n"; 
        }

        return result;
    }

    public abstract void ShowPattern(int x, int y, Shake shaker);

    public abstract void HidePattern(int x, int y);

    public virtual void ImproveStep(int byThisValue)
    {
        step += byThisValue;
    }
    public virtual void ImproveDamage(int byThisValue)
    {
        damage += byThisValue;
    }
}
