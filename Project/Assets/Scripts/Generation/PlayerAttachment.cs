using UnityEngine;
using System.Collections;

public class PlayerAttachment : MonoBehaviour 
{
	public Vector2 maxJumpDistance;
	public Vector2	maxPlayerSize;

	public void OnLevelEnd() { }
	public void OnLevelLoad() { }

	public void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponent(typeof(LevelEndAttachment)) != null)
		{
			Debug.Log("COLLIDE");
			OnLevelEnd();
			Application.LoadLevel(Application.loadedLevelName);
		}
	}
}
