using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DecorationAttachment))]
public class DecorationAttachmentEditor : Editor {

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		DecorationAttachment dec = (DecorationAttachment) target;

		dec.type = (DecorationAttachment.DecorationType) EditorGUILayout.EnumMaskField("Decoration Type", dec.type);
		dec.frequency = EditorGUILayout.Slider("Frequency", dec.frequency, 0f, 1f);

		Vector2 maxSize = EditorGUILayout.Vector2Field("Maximum Size", dec.maxSize);
		if (maxSize == null) {
			Sprite decSprite = ((SpriteRenderer) dec.gameObject.GetComponent(typeof(SpriteRenderer))).sprite;
			maxSize = new Vector2(decSprite.bounds.size.x, decSprite.bounds.size.y);
		}

		if (maxSize.x <= 0) {
			Sprite decSprite = ((SpriteRenderer) dec.gameObject.GetComponent(typeof(SpriteRenderer))).sprite;
			maxSize = new Vector2(decSprite.bounds.size.x, maxSize.y);
		}

		if (maxSize.y <= 0) {
			Sprite decSprite = ((SpriteRenderer) dec.gameObject.GetComponent(typeof(SpriteRenderer))).sprite;
			maxSize = new Vector2(maxSize.x, decSprite.bounds.size.y);
		}
		dec.maxSize = maxSize;
	}
}
