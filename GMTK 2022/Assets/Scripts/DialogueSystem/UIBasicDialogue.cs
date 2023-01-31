using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIDBasicialogueData", menuName = "ScriptableObjects/UIBasicDialogueDataScriptableObject", order = 1)]
public class UIBasicDialogue : Dialogue
{
    public List<DiffColorWordIndexDic> diffColorWordIndex;
    public List<DiffColorDic> diffColor;
}

public class RealUIBasicDialogue : RealDialogue
{
    public int[] diffColorWordIndex;
    public Color[] diffColor;

    public override void Init(Dialogue dialogue)
    {
        base.Init(dialogue);
        
        int arraysLength = dialogue.sentences.Length;
        
        UIDialogue uiDialogue = dialogue as UIDialogue;
        
        diffColorWordIndex = new int[arraysLength];
        diffColor = new Color[arraysLength];
    }
    
    public void SetCustomDiffColorWordIndex(int index, int customDiffColorWordIndex)
    {
        this.diffColorWordIndex[index] = customDiffColorWordIndex;
    }
    
    public void SetCustomDiffColor(int index, Color customDiffColor)
    {
        this.diffColor[index] = customDiffColor;
    }
    

}
