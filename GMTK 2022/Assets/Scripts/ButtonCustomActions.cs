using UnityEngine;

public class ButtonCustomActions : MonoBehaviour
{

    [SerializeField] private AudioSource hoverSound;

    private void OnMouseEnter()
    {
        hoverSound.Play();
    }

}
