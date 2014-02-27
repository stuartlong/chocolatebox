using UnityEngine;
using System.Collections;

public class CharacterAnims : MonoBehaviour 
{
	private Transform _transform;
	private Animator _animator;
	private Character character;

	public enum anim 
	{ 
		None,
		WalkLeft,
		WalkRight,
		StandLeft,
		StandRight,
		FallLeft,
		FallRight
	}

	private anim currentAnim;

	// hash the animation state string to save performance
	private int _playerAnimState = Animator.StringToHash("animstate");
	private int _animState;

	void Awake()
	{
		// cache components to save on performance
		_transform = transform;
		_animator = this.GetComponent<Animator>();
		character = this.GetComponent<Character>();

		_animState = _playerAnimState;

	}
	
	void Update() 
	{
		// run left
		if(character.currentInputState == Character.inputState.WalkLeft && character.grounded == true && currentAnim != anim.WalkLeft)
		{
			currentAnim = anim.WalkLeft;
			_animator.SetInteger(_animState, 1);
			_transform.localScale = new Vector3(1,1,1);
		}

		// stand left
		if(character.currentInputState != Character.inputState.WalkLeft && character.grounded == true && currentAnim != anim.StandLeft && character.facingDir == Character.facing.Left)
		{
			currentAnim = anim.StandLeft;
			_animator.SetInteger(_animState, 0);
			_transform.localScale = new Vector3(1,1,1);
		}
		
		// run right
		if(character.currentInputState == Character.inputState.WalkRight && character.grounded == true && currentAnim != anim.WalkRight)
		{
			currentAnim = anim.WalkRight;
			_animator.SetInteger(_animState, 1);
			_transform.localScale = new Vector3(-1,1,1);
		}

		// stand right
		if(character.currentInputState != Character.inputState.WalkRight && character.grounded == true && currentAnim != anim.StandRight && character.facingDir == Character.facing.Right)
		{
			currentAnim = anim.StandRight;
			_animator.SetInteger(_animState, 0);
			_transform.localScale = new Vector3(-1,1,1);
		}
		
		// fall or jump left
		if(character.grounded == false && currentAnim != anim.FallLeft && character.facingDir == Character.facing.Left)
		{
			currentAnim = anim.FallLeft;
			_animator.SetInteger(_animState, 2);
			_transform.localScale = new Vector3(1,1,1);
		}

		// fall or jump right
		if(character.grounded == false && currentAnim != anim.FallRight && character.facingDir == Character.facing.Right)
		{
			currentAnim = anim.FallRight;
			_animator.SetInteger(_animState, 2);
			_transform.localScale = new Vector3(-1,1,1);
		}
	}
}
