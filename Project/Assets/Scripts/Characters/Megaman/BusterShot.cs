using UnityEngine;
using System.Collections;

public class BusterShot : MonoBehaviour {
    [HideInInspector]
    public Megaman.mmFacing direction;
    private float initialX;

    /// <summary>
    /// Time between shots in seconds
    /// </summary>
    public float threshold;

    public Vector2 velocity;
	// Use this for initialization
	void Start () {
        Debug.Log(direction);
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
}
