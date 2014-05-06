using UnityEngine;
using System.Collections;

public class BusterShot : MonoBehaviour {
    [HideInInspector]
    public Megaman.mmFacing direction;
    private float initialX;

    public BusterExplosion explosion;

    /// <summary>
    /// Time between shots in seconds
    /// </summary>
    public float threshold;

    public Vector2 velocity;
	// Use this for initialization
	void Start () {
        initialX = transform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
        if (Mathf.Abs(initialX - transform.position.x) > 20)
        {
            Destroy(this.gameObject);
        }
        else
        {
            if (direction == Megaman.mmFacing.Left)
            {
                rigidbody2D.velocity = -this.velocity;
            }
            else
            {
                rigidbody2D.velocity = this.velocity;
            }
        }
	}

    public void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Entering collision");
        if (col.gameObject.tag == "Enemy")
        {
            Enemy enemy = col.gameObject.GetComponent<Enemy>();
            enemy.health--;
            this.Explode();
        }
    }




    public void Explode()
    {
        //Launch Explosion Animationd
        Destroy(this.gameObject);
        Instantiate(explosion.gameObject, this.gameObject.transform.position, new Quaternion());
    }
}
