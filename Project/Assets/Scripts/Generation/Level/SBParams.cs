using UnityEngine;
using System.Collections;

/// <summary>
/// A simple class for centralizing the parameters used in <see cref="SectionBuilder"/>.
/// </summary>
public class SBParams 
{
	private static float SMALLEST_PARAM = 0.0000001f;

	/// <summary>
	/// The size of the section. The units here are Unity units, not the desired number of blocks.
	/// </summary>
	public Vector2 size;

	/// <summary>
	/// The starting entrance positions to the section. These will be left open/empty.
	/// </summary>
	public EntrancePositions entrancePositions;

	/// <summary>
	/// Gets or sets the hilliness which determines how often the height of the ground changes.
	/// </summary>
	public float Hilliness
	{
		get
		{
			return hilliness;
		}
		set
		{
			hilliness = value < 0 ? SMALLEST_PARAM : Mathf.Min(1.0f, value);
		}
	}

	/// <summary>
	/// Gets or sets the pittiness which determines how often and wide pits appear.
	/// </summary>
	public float Pittiness
	{
		get
		{
			return pittiness;
		}
		set
		{
			pittiness = value < 0 ? SMALLEST_PARAM : Mathf.Min(1.0f, value);
		}
	}

	private float hilliness;
	private float pittiness;
}
