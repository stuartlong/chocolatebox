using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    public enum CharacterType
    {
        Null, 
        Player,
        Goomba,
        Koopa
    }

    public CharacterType characterType;

    
    public enum inputState
    {
        None,
        WalkLeft,
        WalkRight,
        Jump,
        Attack
    }

    [HideInInspector]
    public inputState currentInputState;

    [HideInInspector]
    public enum facing { Right, Left }
    [HideInInspector]
    public facing facingDir;

    [HideInInspector]
    public bool alive = true;
    [HideInInspector]
    public Vector3 spawnPos;

    protected Transform _transform;
    protected Rigidbody2D _rigidbody;

    // edit these to tune character movement	
    protected float walkVel = 4; 	// walk speed
    protected float jumpVel = 6f; 	// jump velocity
    protected float jump2Vel = 2f; 	// double jump velocity
    protected float fallVel = 1f;		// fall velocity, gravity

    public float moveVel;
  
    private int jumps = 0;
    private int maxJumps = 2; 		// set to 2 for double jump

    // raycast stuff
    private RaycastHit2D hit;
    private Vector2 physVel = new Vector2();
   
    public bool grounded = false;
    protected int groundMask = 1 << 8; // Ground layer mask

    public virtual void Awake()
    {
        _transform = transform;
        _rigidbody = rigidbody2D;
    }

    // Use this for initialization
    public virtual void Start()
    {
        moveVel = this.walkVel;
    }

     // ============================== FIXEDUPDATE ============================== 

    public virtual void UpdatePhysics()
    {
        if (alive == false) return;

        physVel = Vector2.zero;

        if (currentInputState == inputState.WalkLeft)
        {
            physVel.x = -moveVel;
        }

        // move right
        if (currentInputState == inputState.WalkRight)
        {
            physVel.x = moveVel;
        }

        // jump
        if (currentInputState == inputState.Jump)
        {
            if (jumps < maxJumps)
            {
                jumps += 1;
                if (jumps == 1)
                {
                    _rigidbody.velocity = new Vector2(physVel.x, jumpVel);
                }
                else if (jumps == 2)
                {
                    _rigidbody.velocity = new Vector2(physVel.x, jump2Vel);
                }
            }
        }

        // use raycasts to determine if the player is standing on the ground or not
        if (Physics2D.Raycast(new Vector2(_transform.position.x - 0.1f, _transform.position.y), -Vector2.up, .26f, groundMask)
            || Physics2D.Raycast(new Vector2(_transform.position.x + 0.1f, _transform.position.y), -Vector2.up, .26f, groundMask))
        {
            grounded = true;
            jumps = 0;
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

