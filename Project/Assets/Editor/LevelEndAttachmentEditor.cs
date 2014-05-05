using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Custom LevelEndAttachment Unity editor.
/// Primary Author - Stuart Long
/// </summary>
[CustomEditor(typeof(LevelEndAttachment))]
public class LevelEndAttachmentEditor : Editor {
	public override void OnInspectorGUI()
	{
		LevelEndAttachment levelEnd = (LevelEndAttachment) target;
		Vector2 maxSize = EditorGUILayout.Vector2Field("Maximum Size", levelEnd.maxSize);
		if (maxSize == null) {
			Sprite sprite = ((SpriteRenderer) levelEnd.gameObject.GetComponent(typeof(SpriteRenderer))).sprite;
			maxSize = new Vector2(sprite.bounds.size.x, sprite.bounds.size.y);
		}
		
		if (maxSize.x <= 0) {
			Sprite sprite = ((SpriteRenderer) levelEnd.gameObject.GetComponent(typeof(SpriteRenderer))).sprite;
			maxSize = new Vector2(sprite.bounds.size.x, maxSize.y);
		}
		
		if (maxSize.y <= 0) {
			Sprite sprite = ((SpriteRenderer) levelEnd.gameObject.GetComponent(typeof(SpriteRenderer))).sprite;
			maxSize = new Vector2(maxSize.x, sprite.bounds.size.y);
		}
		levelEnd.maxSize = maxSize;
	}
}

