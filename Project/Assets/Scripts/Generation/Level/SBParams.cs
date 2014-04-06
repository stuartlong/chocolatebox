using UnityEngine;
using System.Collections;

/// <summary>
/// A simple class for centralizing the parameters used in <see cref="SectionBuilder"/>.
/// Primary Author - Stuart Long
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
	/// The various group of sprites that comprise this section.
	/// </summary>
	public SectionSprites sprites;

	/// <summary>
	/// Whether or not pits will be created in this section.
	/// </summary>
	public bool allowPits = true;

	/// <summary>
	/// Indicates that this will be where the current level ends.
	/// </summary>
	public bool lastSection = false;

	public int ceilingHeight = -1;

	
	/// <summary>
	/// A float to be used as a generic level of difficulty
	/// </summary>
	public float difficulty;

	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="Section"/> should be monotonically increasing.
	/// </summary>
	/// <value><c>true</c> if only goes up; otherwise, <c>false</c>.</value>
	public bool OnlyGoesUp
	{
		get
		{
			return onlyUp;
		}

		set
		{
			onlyUp = value;
			if (value) 
			{
				onlyDown = false;
			}
		}
	}


	/// <summary>
	/// Gets or sets a value indicating whether this <see cref="SBParams"/> only down.
	/// </summary>
	/// <value><c>true</c> if only goes down; otherwise, <c>false</c>.</value>
	public bool OnlyGoesDown
	{
		get
		{
			return onlyDown;
		}
		set
		{
			onlyDown = value;
			if (value)
			{
				onlyUp = false;
			}
		}
	}

	private bool onlyDown = false;
	private bool onlyUp = false;

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

	public float Caviness
	{
		get
		{
			return caviness;
		}
		set
		{
			caviness = value < 0 ? SMALLEST_PARAM : Mathf.Min(1.0f, value);
		}
	}

	private float hilliness;
	private float pittiness;
	private float caviness;
}
