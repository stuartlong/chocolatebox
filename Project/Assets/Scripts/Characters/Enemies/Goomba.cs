using UnityEngine;
using System.Collections;

public class Goomba : Enemy
{

    public override void Start()
    {
        base.Start();
        moveVel = 1.0f;
        characterType = Character.CharacterType.Goomba;
        currentInputState = inputState.WalkLeft;
        facingDir = facing.Left;
    }

    public void Update()
    {
        if (_transform.position.y <= -100)
        {
            Destroy(this.gameObject);
        }
    }


    public void FixedUpdate()
    {
        _transform.rotation = new Quaternion(0, 0, 0, 0);
        //If something is on our left
        if (Physics2D.Raycast(new Vector2(_transform.position.x, _transform.position.y), -Vector2.right, 0.48f, groundMask)){
            currentInputState = inputState.WalkRight;
            facingDir = facing.Right;
        }
        //If something is on our right
        if(Physics2D.Raycast(new Vector2(_transform.position.x, _transform.position.y), Vector2.right, 0.48f, groundMask))
        {
            currentInputState = inputState.WalkLeft;
            facingDir = facing.Left;
        }

        UpdatePhysics();

    }
}
