using UnityEngine;
using System.Collections;

public class Megaman : MonoBehaviour
{
    public bool jumping = false;
    public bool running = false;
    public bool shooting = false;
    public bool hit = false;
    public bool pressedJump = false;

    public bool grounded = true;
    protected int groundMask = 1 << 8; // Ground layer mask
    public bool jumped = false;
    
    public enum mmFacing { Right, Left }
    public mmFacing currentFacing;

    [HideInInspector]
    public bool alive = true;
    [HideInInspector]
    public Vector3 spawnPos;

    protected Transform _transform;
    protected Rigidbody2D _rigidbody;

    private float moveVel = 4.0f;
    private float jumpVel = 6.0f;
    private float fallVel = 1.0f;
    
    private Vector2 physVec;


    public void Awake()
    {
        _transform = transform;
        _rigidbody = rigidbody2D;
    }

    public void Start()
    {
        spawnPos = _transform.position;
    }

    public void Update()
    {
        // Reset level TEST ONLY
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel(0);
        }

    }

    public void FixedUpdate()
    {

        // move left
        if (Input.GetKey(KeyCode.A))
        {
            currentFacing = mmFacing.Left;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            currentFacing = mmFacing.Right;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            running = true;
        }
        else
        {
            running = false;
        }

        // jump
        if (Input.GetKey(KeyCode.W))
        {
            pressedJump = true;
        }

        // shoot
        if (Input.GetKey(KeyCode.Space))
        {
            shooting = true;
        }
        else
        {
            shooting = false;
        }


        UpdatePhysics();

    }

    public void UpdatePhysics()
    {
        if (alive == false) return;

        physVec = Vector2.zero;

        // move left
        if (running && currentFacing == mmFacing.Left)
        {
            physVec.x = -moveVel;
        }

        // move right
        if (running && currentFacing == mmFacing.Right)
        {
            physVec.x = moveVel;
        }

        if (pressedJump)
        {
            jumping = true;
        }

        // jump
        if (jumping)
        {
            if (pressedJump)
            {
                if (!jumped)
                {
                    _rigidbody.velocity = new Vector2(physVec.x, jumpVel);
                    jumped = true;
                }
            }
        }

        // use raycasts to determine if the player is standing on the ground or not
        if (Physics2D.Raycast(new Vector2(_transform.position.x - 0.20f, _transform.position.y), -Vector2.up, .40f, groundMask)
            || Physics2D.Raycast(new Vector2(_transform.position.x + 0.20f, _transform.position.y), -Vector2.up, .40f, groundMask))
        {
            grounded = true;
            jumped = false;
            jumping = false;
            pressedJump = false;
        }
        else
        {
            grounded = false;
            jumping = true;
            _rigidbody.AddForce(-Vector3.up * fallVel);
        }

        // actually move the player
        _rigidbody.velocity = new Vector2(physVec.x, _rigidbody.velocity.y);
    }
}
