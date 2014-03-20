using UnityEngine;
using System.Collections;

public class Goomba : Character
{

    public override void Start()
    {
        base.Start();
        walkVel = 1f;
        characterType = Character.CharacterType.Goomba;
        currentInputState = inputState.WalkLeft;
        facingDir = facing.Left;

    }

    public void Update()
    {
        
    }

    public void FixedUpdate()
    {
        _transform.rotation = new Quaternion(0, 0, 0,0);
        Debug.Log("LOGGING");
        //If something is on our left
        if (Physics2D.Raycast(new Vector2(_transform.position.x, _transform.position.y), -Vector2.right, 0.48f, groundMask)){
            currentInputState = inputState.WalkRight;
            facingDir = facing.Right;

            Debug.Log("SOMETHING ON LEFT!");

        }
        //If something is on our right
        if(Physics2D.Raycast(new Vector2(_transform.position.x, _transform.position.y), Vector2.right, 0.48f, groundMask))
        {
            currentInputState = inputState.WalkLeft;
            facingDir = facing.Left;
            Debug.Log("SOMETHING ON RIGHT!");
        }

        UpdatePhysics();

    }
}
