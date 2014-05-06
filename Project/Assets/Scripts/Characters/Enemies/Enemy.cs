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

    public void OnCollision2DEnter(Collision2D col)
    {
        Debug.Log("Entering collision");
        if (col.gameObject.tag == "Weapon")
        {
            BusterShot shot = col.gameObject.GetComponent<BusterShot>();
            shot.Explode();

            this.health--;
        }
    }

    public void DeathAnimation()
    {
        Destroy(this.gameObject);
    }
}
