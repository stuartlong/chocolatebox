using UnityEngine;
using System.Collections;

public class PlayerAttachment : MonoBehaviour 
{
	public Vector2 maxJumpDistance;
	public Vector2	maxPlayerSize;

	[HideInInspector] public bool repeatingLevel;

	public void OnLevelEnd() { }
	public void OnLevelLoad() { }

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponent(typeof(LevelEndAttachment)) != null)
		{
			Debug.Log("COLLIDE");
			OnLevelEnd();
			if (repeatingLevel) 
			{
				Application.LoadLevel(Application.loadedLevelName);
			}
		}
	}
}
