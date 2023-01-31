using UnityEngine;


public class SkullHeadMovement : MonoBehaviour
{
    [SerializeField] private Vector2 force;
    private Rigidbody2D skullHeadRigid;

    void Start()
    {
        skullHeadRigid = GetComponent<Rigidbody2D>();
        skullHeadRigid.AddRelativeForce(force);
    }
    
}
