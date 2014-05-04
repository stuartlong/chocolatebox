using UnityEngine;
using System.Collections;

/// <summary>
/// The attachment script for decorations.
/// Primary Author - Stuart Long
/// </summary>
public class DecorationAttachment : MonoBehaviour 
{
	/// <summary>
	/// The maximum size of this decoration. This
	/// will default to this sprite's bounds if
	/// not set manually.
	/// </summary>
	public Vector2 maxSize;

	/// <summary>
	/// The type of this decoration.
	/// </summary>
	public DecorationType type;

	/// <summary>
	/// How frequently, relative to other
	/// decorations, this decoration should
	/// appear.
	/// 
	/// Note, this frequency will be normalized
	/// with the other frequencies of decorations
	/// in this section.
	/// </summary>
	public float frequency;

	/// <summary>
	/// Indicates whether this decoration
	/// can overlap with other decoations.
	/// </summary>
	public bool allowOverlap;

	public enum DecorationType
	{
		OnGround,
		Hanging
	}
}
