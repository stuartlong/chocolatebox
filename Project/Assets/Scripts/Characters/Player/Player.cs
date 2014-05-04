using UnityEngine;
using System.Collections;

public class Player : Character
{
    private bool running; 

    public override void Start()
    {
        base.Start();
        characterType = Character.CharacterType.Player;
        this.moveVel = 3.0f;

        spawnPos = _transform.position;
    }

    public void Update()
    {
        // Reset level
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.T))
        {
            Application.LoadLevel(0);
        }
        
        //Movement
        
    }

    public void FixedUpdate()
    {
        currentInputState = inputState.None;

        // running
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            this.running = true;
        }
        else
        {
            this.running = false;
        }

        // move left
        if (Input.GetKey(KeyCode.A))
        {
            currentInputState = inputState.WalkLeft;
            facingDir = facing.Left;
        }

        // move right

        if (Input.GetKey(KeyCode.D) && currentInputState != inputState.WalkLeft)
        {
            currentInputState = inputState.WalkRight;
            facingDir = facing.Right;
        }

        // jump
        if (Input.GetKeyDown(KeyCode.W))
        {
            currentInputState = inputState.Jump;
        }

        UpdatePhysics();

    }

    public override void UpdatePhysics()
    {
        if (running)
        {
            moveVel = runVel;
        }
        else
        {
            moveVel = walkVel;
        }

        base.UpdatePhysics();
    }
}
