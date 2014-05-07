using UnityEngine;
using System.Collections;


public class Megaman : MonoBehaviour
{
    public bool jumping = false;
    public bool running = false;
    public bool shooting = false;
    public bool hit = false;
    public bool pressedJump = false;
    private bool fireShot;
    private bool invincible;

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

    public BusterShot weapon;
    

    private float moveVel = 5.0f;
    private float jumpVel = 7.0f;
    private float fallVel = 1.0f;
    
    private Vector2 physVec;

    private System.DateTime lastShotTime;
    private System.TimeSpan shotInterval;

    public float woundDuration;
    private System.DateTime lastHitTime;
    private System.TimeSpan hitDuration;

    public AudioClip fallSound;
    public AudioClip megaBusterSound;
    public AudioClip hurtSound;

    public void Awake()
    {
        _transform = transform;
        _rigidbody = rigidbody2D;
        lastShotTime = System.DateTime.Now;
        shotInterval = new System.TimeSpan((long) (weapon.threshold * 10000000));
        hitDuration = new System.TimeSpan((long)(woundDuration * 10000000));
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

        if (hit)
        {
            System.DateTime curHitTime = System.DateTime.Now;

            if (curHitTime - lastHitTime > hitDuration)
            {
                hit = false;
                invincible = true;
            }
        }
        if (invincible)
        {
            System.DateTime recoverTime = System.DateTime.Now;
            if (recoverTime - lastHitTime > hitDuration + hitDuration)
            {
                invincible = false;
                this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;

            }
            else
            {
                if (this.gameObject.GetComponent<SpriteRenderer>().color != Color.gray)
                {
                    this.gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
                } 
                else
                {
                    this.gameObject.GetComponent<SpriteRenderer>().color = Color.clear;
                }
            }
        }

    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        // If we are not already hit (Invincibility frames)
        if (!hit)
        {
            if (col.gameObject.tag == "Enemy" && !invincible)
            {
                hit = true;
                lastHitTime = System.DateTime.Now;
                AudioSource.PlayClipAtPoint(hurtSound, transform.position);

                if (col.gameObject.transform.position.x <= this.transform.position.x)
                {
                    this.currentFacing = mmFacing.Left;
                }
                else
                {
                    this.currentFacing = mmFacing.Right;
                }
            }
        }
        
    }

    public void FixedUpdate()
    {
        //Disable button commands when hit. 
        if (!hit)
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

                System.DateTime nextShotTime = System.DateTime.Now;

                if (nextShotTime - lastShotTime > shotInterval)
                {
                    if (!hit)
                    {
                        fireShot = true;
                        lastShotTime = nextShotTime;
                    }
                }
                else
                {
                    fireShot = false;
                }
            }
            else
            {
                shooting = false;
                fireShot = false;
            }


            if (fireShot)
            {
                // Make a new bustershot
                Vector3 shotPosition = new Vector3();
                if (currentFacing == mmFacing.Left)
                {
                    shotPosition.x = _transform.position.x - 0.4f;
                }
                else
                {
                    shotPosition.x = transform.position.x + 0.4f;
                }
                shotPosition.y = _transform.position.y + .02f;

                //Fire the gun, launch shot in other direction. 
                weapon.direction = currentFacing;
                BusterShot shot = weapon;
                shot.direction = currentFacing;
                Instantiate(shot.gameObject, shotPosition, new Quaternion());

                AudioSource.PlayClipAtPoint(megaBusterSound, this.transform.position);
            }
        }
        else //WE'VE BEEN HIT CAP'N STAGGER A BIT!
        {
            Vector3 staggerPosition = new Vector3();
            staggerPosition.y = this.transform.position.y;
            staggerPosition.z = this.transform.position.z;
            if(currentFacing == mmFacing.Left)
            {
                //We are facing backwards, so move forwards
                staggerPosition.x = this.transform.position.x + 0.075f;
            }
            else
            {
                staggerPosition.x = this.transform.position.x - 0.075f;
            }

            this.transform.position = staggerPosition;
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

            if (jumping && !Input.GetKey(KeyCode.W))
            {
                AudioSource.PlayClipAtPoint(fallSound, transform.position);
            }


            jumped = false;
            jumping = false;
            pressedJump = false;
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + .001f, 0);
        }
        else
        {
            grounded = false;
            jumping = true;
            _rigidbody.AddForce(-Vector3.up * fallVel);
        }

        if (hit)
        {
            physVec.x = -physVec.x/10;
        }

        // actually move the player
        _rigidbody.velocity = new Vector2(physVec.x, _rigidbody.velocity.y);
    }
}
