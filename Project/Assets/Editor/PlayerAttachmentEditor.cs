using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(PlayerAttachment))]
public class PlayerAttachmentEditor : Editor {

	public override void OnInspectorGUI()
	{
		PlayerAttachment player = (PlayerAttachment) target;
		Vector2 maxSize = EditorGUILayout.Vector2Field("Maximum Size", player.maxPlayerSize);
		if (maxSize == null) {
			Sprite sprite = ((SpriteRenderer) player.gameObject.GetComponent(typeof(SpriteRenderer))).sprite;
			maxSize = new Vector2(sprite.bounds.size.x, sprite.bounds.size.y);
		}
		
		if (maxSize.x <= 0) {
			Sprite sprite = ((SpriteRenderer) player.gameObject.GetComponent(typeof(SpriteRenderer))).sprite;
			maxSize = new Vector2(sprite.bounds.size.x, maxSize.y);
		}
		
		if (maxSize.y <= 0) {
			Sprite sprite = ((SpriteRenderer) player.gameObject.GetComponent(typeof(SpriteRenderer))).sprite;
			maxSize = new Vector2(maxSize.x, sprite.bounds.size.y);
		}
		player.maxPlayerSize = maxSize;

		player.maxJumpDistance = EditorGUILayout.Vector2Field("Maximum Jump Distance", player.maxJumpDistance);
	}
}
	
