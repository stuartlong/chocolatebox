using UnityEngine;
using System.Collections;

public class DecorationAttachment : MonoBehaviour 
{
	public Vector2 maxSize;

	public DecorationType type;

	public float frequency;

	public enum DecorationType
	{
		OnGround,
		Hanging,
		BelowGround,
		InCeiling,
		Floating
	}
}
