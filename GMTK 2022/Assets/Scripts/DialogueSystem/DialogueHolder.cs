using UnityEngine;
using TMPro;
using System;

public abstract class DialogueHolder : MonoBehaviour
{

    public TMP_Text dialogueHolderText;
    public AudioSource audioSource;
    public EDialogueEnd dialogueEnd;

    protected ETextEffects textEffect = ETextEffects.None;
    
    public Action HolderOnStartDialogueActions;
    public Action<RealDialogue, int> HolderOnCustomDialogueActions;
    public Action HolderOnOneDialogueEndActions;
    public Action HolderOnEndDialogueActions;
    
    public bool DialogueStopGame;

    public void SubsActions()
    {
        DialogueManager.Instance.OnStartDialogueActions += OnStartDialogueActions;
        DialogueManager.Instance.OnCustomDialogueActions += OnCustomDialogueActions;
        DialogueManager.Instance.OnEndDialogueActions += OnEndDialogueActions;
        DialogueManager.Instance.OnOneDialogueEndActions += OnOneDialogueEndActions;
    }

    public void UnSubsActions()
    {
        DialogueManager.Instance.OnStartDialogueActions -= OnStartDialogueActions;
        DialogueManager.Instance.OnCustomDialogueActions -= OnCustomDialogueActions;
        DialogueManager.Instance.OnEndDialogueActions -= OnEndDialogueActions;
        DialogueManager.Instance.OnOneDialogueEndActions += OnOneDialogueEndActions;
    }

    private void OnDestroy()
    {
        DialogueManager.Instance.OnStartDialogueActions -= OnStartDialogueActions;
        DialogueManager.Instance.OnCustomDialogueActions -= OnCustomDialogueActions;
        DialogueManager.Instance.OnEndDialogueActions -= OnEndDialogueActions;
        DialogueManager.Instance.OnOneDialogueEndActions += OnOneDialogueEndActions;
    }

    public void SetEtextEffects(ETextEffects textEffect) => this.textEffect = textEffect;

    public abstract void OnStartDialogueActions(Dialogue dialogue);
    public abstract RealDialogue OnCustomDialogueActions(int index);
    public abstract void OnOneDialogueEndActions();
    public abstract void OnEndDialogueActions();



}
