using UnityEngine;
using System.Collections;

public class Enemy : Character
{
    [HideInInspector]
    public static GameObject player;
    
    private static float hdist = 10.0f;
    public int health;

    private int weaponLayer;

    public bool activeEnemy = false;

    public override void Start()
    {
        base.Start();
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
    }

    public override void UpdatePhysics()
    {
        if(activeEnemy){
            base.UpdatePhysics();

            if (this.health <= 0)
            {
                this.DeathAnimation();
            }
        }
        else
        {
            if (Mathf.Abs(player.transform.position.x - this.transform.position.x) < hdist)
            {
                activeEnemy = true;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (this.facingDir == facing.Left)
            {
                this.facingDir = facing.Right;
                this.currentInputState = inputState.WalkRight;
                Vector3 oldPos = this.transform.position;
                this.transform.position = new Vector3(oldPos.x + 0.01f, oldPos.y, oldPos.z);
            }
            else
            {
                this.facingDir = facing.Left;
                this.currentInputState = inputState.WalkLeft;
                Vector3 oldPos = this.transform.position;
                this.transform.position = new Vector3(oldPos.x - 0.01f, oldPos.y, oldPos.z);
            }
        }
    }

    public void DeathAnimation()
    {
        Destroy(this.gameObject);
    }
}
