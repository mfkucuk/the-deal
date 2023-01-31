using UnityEngine;
using DG.Tweening;
using System;

public class CharacterMovement : MonoBehaviour
{

    [SerializeField] private float duration = 0.8f;
    [SerializeField] private float jumpPower = 1;
    
    [SerializeField] private float shakeDuration = 3;
    [SerializeField] private Vector3 shakePower;

    public Action OnJumpFinished;

    public void Jump(Transform characterTransform, Vector3 targetPosition, string jumpVoiceName, bool checkFinish = false)
    {

        AudioManager.Instance.Play(jumpVoiceName);

        characterTransform.DOShakeRotation(shakeDuration, shakePower).OnComplete(() => { characterTransform.eulerAngles = Vector3.zero; });

        characterTransform.DOJump(targetPosition, jumpPower, 1, duration, false).OnComplete(() => { if(checkFinish) JumpFinished(); });
        
    }

    private void JumpFinished()
    {
        OnJumpFinished?.Invoke();
    }

}
