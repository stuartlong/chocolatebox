using UnityEngine;
using System.Collections;

/// <summary>
/// The attachment script for the main player.
/// Primary Author - Stuart Long
/// </summary>
public class PlayerAttachment : MonoBehaviour 
{
	/// <summary>
	/// The maximum distance this player can jump.
	/// </summary>
	public Vector2 maxJumpDistance;

	/// <summary>
	/// The maximum size of this player. Will default to the player's
	/// sprite's bounds if not set manually.
	/// </summary>
	public Vector2	maxPlayerSize;

	/// <summary>
	/// Called when the LevelEnd object is reached
	/// by the player.
	/// </summary>
	public void OnLevelEnd() { }

	/// <summary>
	/// Called when the level finishes generating.
	/// </summary>
	public void OnLevelLoad() { }

	[HideInInspector] public bool repeatingLevel;
	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponent(typeof(LevelEndAttachment)) != null)
		{
			Debug.Log("IT'S AN END");
			OnLevelEnd();
			if (repeatingLevel) 
			{
				Debug.Log ("LOAD");
				Application.LoadLevel(Application.loadedLevelName);
			}
		}
	}
}
