using UnityEngine;
using System.Collections;

public class Enemy : Character
{
    [HideInInspector]
    public static GameObject player;
    
    private static float hdist = 10.0f;

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
        }
        else
        {
            if (Mathf.Abs(player.transform.position.x - this.transform.position.x) < hdist)
            {
                activeEnemy = true;
            }
        }
    }
}
