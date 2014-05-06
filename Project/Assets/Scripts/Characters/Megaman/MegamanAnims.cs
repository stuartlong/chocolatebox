using UnityEngine;
using System.Collections;

public class MegamanAnims : MonoBehaviour 
{
	private Transform _transform;
	private Animator _animator;
    private BoxCollider2D _collider;
	private Megaman mm;
    private Vector3 initialScale;


    private int jumpId = Animator.StringToHash("MEGAMAN_JUMPING");
    private int runId = Animator.StringToHash("MEGAMAN_RUNNING");
    private int shootId = Animator.StringToHash("MEGAMAN_SHOOTING");
    private int hitId = Animator.StringToHash("MEGAMAN_HIT");

  	void Awake()
	{
		// cache components to save on performance
		_transform = transform;
		_animator = this.GetComponent<Animator>();
        _collider = this.GetComponent<BoxCollider2D>();
		mm = this.GetComponent<Megaman>();
        initialScale = _transform.localScale;
	}
	
	void Update() 
	{

        //face left

        if (mm.currentFacing == Megaman.mmFacing.Left)
        {
            _transform.localScale = new Vector3(initialScale.x, initialScale.y, initialScale.z);
        }

        // face right

        if (mm.currentFacing == Megaman.mmFacing.Right)
        {
            _transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
        }

        _animator.SetBool(jumpId, mm.jumping);
        _animator.SetBool(runId, mm.running);
        _animator.SetBool(shootId, mm.shooting);
        _animator.SetBool(hitId, mm.hit);
    }
}
