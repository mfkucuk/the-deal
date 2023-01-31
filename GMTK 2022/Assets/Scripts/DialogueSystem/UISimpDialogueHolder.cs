using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UISimpDialogueDefVal))]
public class UISimpDialogueHolder : DialogueHolder
{
    
    [SerializeField] private Animator dialogueAnimator;
    [SerializeField] private UISimpDialogueDefVal uiSimpDialogueDefVal;

    private Color color = Color.white;
    private int diffColorWordIndex = -1;

    private int indexCounter = 0;
    
    private RealUISimpDialogue realUISimpDialogue;
    private UISimpDialogue uiSimpDialogue;
    

    public void Update()
    {
        if (textEffect != ETextEffects.None)
        {
            if (DialogueStopGame)
                UnScaledTextEffectController.Instance.DoTextEffect(dialogueHolderText, textEffect);
            else
                TextEffectsController.Instance.DoTextEffect(dialogueHolderText, textEffect);
        }

        if (diffColorWordIndex != -1)
        {
            if (DialogueStopGame)
                UnScaledTextEffectController.Instance.ChangeWordColor(dialogueHolderText, diffColorWordIndex, color);
            else
                TextEffectsController.Instance.ChangeWordColor(dialogueHolderText, diffColorWordIndex, color);
        }

    }

    public override void OnStartDialogueActions(Dialogue dialogue)
    {
        if (DialogueStopGame)
        {
            DialogueManager.Instance.DialogueStopGame = true;
            Time.timeScale = 0f;
        }
        
        dialogueAnimator.enabled = true;
        
        this.gameObject.SetActive(true);
        
        HolderOnStartDialogueActions?.Invoke();
            
        realUISimpDialogue = new RealUISimpDialogue();
        
        realUISimpDialogue.Init(dialogue);
        
        uiSimpDialogue = dialogue as UISimpDialogue;

        for (int index = 0; index < dialogue.sentences.Length; index++)
            SetDefaultValues(index);
    }
    
    private void SetDefaultValues(int index)
    {
        realUISimpDialogue.SetText(index, uiSimpDialogue.sentences[index]); //Set Texts
        
        realUISimpDialogue.SetCustomTextWriteSpeed(index, uiSimpDialogueDefVal.textWriteSpeeds[uiSimpDialogue.characterCounts[index]]);
        realUISimpDialogue.SetCustomTextAudio(index, uiSimpDialogueDefVal.textAudios[uiSimpDialogue.characterCounts[index]]);
        realUISimpDialogue.SetCustomTextEffect(index, uiSimpDialogueDefVal.textEffects[uiSimpDialogue.characterCounts[index]]);
        realUISimpDialogue.SetCustomOverWrite(index);
        realUISimpDialogue.SetCustomAnimatorStateName(index, uiSimpDialogueDefVal.animatorStateNames[uiSimpDialogue.characterCounts[index]]);
        realUISimpDialogue.SetCustomDiffColorWordIndex(index, uiSimpDialogueDefVal.diffColorWordIndex[uiSimpDialogue.characterCounts[index]]);
        realUISimpDialogue.SetCustomDiffColor(index, uiSimpDialogueDefVal.diffColor[uiSimpDialogue.characterCounts[index]]);
    }

    private void ControlCustomValues(int index)
    {
        if (index < uiSimpDialogue.textWriteSpeeds.Count)  
            realUISimpDialogue.SetCustomTextWriteSpeed(uiSimpDialogue.textWriteSpeeds[index].id, uiSimpDialogue.textWriteSpeeds[index].textWriteSpeed);
        if (index < uiSimpDialogue.textAudios.Count)  
            realUISimpDialogue.SetCustomTextAudio(uiSimpDialogue.textAudios[index].id, uiSimpDialogue.textAudios[index].textAudio);
        if (index < uiSimpDialogue.textEffects.Count)  
            realUISimpDialogue.SetCustomTextEffect(uiSimpDialogue.textEffects[index].id, uiSimpDialogue.textEffects[index].textEffect);
        if (index < uiSimpDialogue.overWrites.Count)  
            realUISimpDialogue.SetCustomOverWrite(uiSimpDialogue.overWrites[index].id, uiSimpDialogue.overWrites[index].overWrite);
        if (index < uiSimpDialogue.diffColorWordIndex.Count)  
            realUISimpDialogue.SetCustomDiffColorWordIndex(uiSimpDialogue.diffColorWordIndex[index].id, uiSimpDialogue.diffColorWordIndex[index].diffColorWordIndex);
        if (index < uiSimpDialogue.diffColor.Count)  
            realUISimpDialogue.SetCustomDiffColor(uiSimpDialogue.diffColor[index].id, uiSimpDialogue.diffColor[index].diffColor);
        if (index < uiSimpDialogue.animatorStateNames.Count)  
            realUISimpDialogue.SetCustomAnimatorStateName(uiSimpDialogue.animatorStateNames[index].id, uiSimpDialogue.animatorStateNames[index].animationStateName);
    }

    public override RealDialogue OnCustomDialogueActions(int index)
    {
        ControlCustomValues(index);
        
        HolderOnCustomDialogueActions?.Invoke(realUISimpDialogue, index);
        
        diffColorWordIndex = realUISimpDialogue.diffColorWordIndex[index];
        color = realUISimpDialogue.diffColor[index];
        
        dialogueAnimator.runtimeAnimatorController = uiSimpDialogue.animators[uiSimpDialogue.characterCounts[index]];
        
        dialogueAnimator.Play(realUISimpDialogue.animatorStateNames[index]);
        
        SetEtextEffects(realUISimpDialogue.textEffects[index]);

        return realUISimpDialogue;
    }

    public override void OnOneDialogueEndActions()
    {
        HolderOnOneDialogueEndActions?.Invoke();
    }

    public override void OnEndDialogueActions()
    {
        if (DialogueStopGame)
        {
            DialogueManager.Instance.DialogueStopGame = false;
            Time.timeScale = 0f;
        }
        
        if (dialogueEnd == EDialogueEnd.None)
        {
            gameObject.SetActive(false);
        }
        else if (dialogueEnd == EDialogueEnd.NextDialogue)
        {
            DialogueTrigger.Instance.TriggerDialogue();
            gameObject.SetActive(false);
        }
        else if (dialogueEnd == EDialogueEnd.LoadScene)
        {
            LevelController.Instance.LoadNextScene();
        }
        
        HolderOnEndDialogueActions?.Invoke();

    }

}
