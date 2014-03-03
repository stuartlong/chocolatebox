using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public enum enemyState
    {
        None,
        WalkLeft,
        WalkRight,
        Jump,
        Attack
    }

    public enemyState currentEnemyState;

    [HideInInspector]
    public enum facing { Right, Left }
    [HideInInspector]
    public facing facingDir;

    [HideInInspector]
    public bool alive = true;
    
    protected Transform _transform;
    protected Rigidbody2D _rigidbody;

    // edit these to tune character movement	
    private float walkVel = 2f; 	// walk speed
    private float fallVel = 1f;		// fall velocity, gravity

    private float moveVel;

    // raycast stuff
    private RaycastHit2D hit;
    private Vector2 physVel = new Vector2();

    public bool grounded = false;
    private int groundMask = 1 << 8; // Ground layer mask

    public virtual void Awake()
    {
        _transform = transform;
        _rigidbody = rigidbody2D;
    }

    // Use this for initialization
    public virtual void Start()
    {
        moveVel = walkVel;
    }

    // ============================== FIXEDUPDATE ============================== 

    public virtual void UpdatePhysics()
    {
        if (alive == false) return;

        physVel = Vector2.zero;

        if (currentEnemyState == enemyState.WalkLeft)
        {
            physVel.x = -moveVel;
        }

        // move right
        if (currentEnemyState == enemyState.WalkRight)
        {
            physVel.x = moveVel;
        }

        // use raycasts to determine if the player is standing on the ground or not
        if (Physics2D.Raycast(new Vector2(_transform.position.x - 0.1f, _transform.position.y), -Vector2.up, .26f, groundMask)
            || Physics2D.Raycast(new Vector2(_transform.position.x + 0.1f, _transform.position.y), -Vector2.up, .26f, groundMask))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
            _rigidbody.AddForce(-Vector3.up * fallVel);
        }

        // actually move the player
        _rigidbody.velocity = new Vector2(physVel.x, _rigidbody.velocity.y);
    }
}

