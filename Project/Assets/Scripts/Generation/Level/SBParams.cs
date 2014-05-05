using UnityEngine;
using System.Collections;

/// <summary>
/// A simple class for centralizing the parameters used in <see cref="SectionBuilder"/>.
/// Primary Author - Stuart Long
/// </summary>
public class SBParams 
{
	public readonly static float SMALLEST_PARAM = 0.01f;

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
	public SectionAttributes sprites;

	/// <summary>
	/// Whether or not pits will be created in this section.
	/// </summary>
	public bool allowPits = true;

	/// <summary>
	/// Whether or not platforms will be generated in this section.
	/// </summary>
	public bool allowPlatforms = true;

	/// <summary>
	/// Indicates that this will be where the current level ends.
	/// </summary>
	public bool lastSection = false;

	/// <summary>
	/// The initial height of the ceiling.
	/// </summary>
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
			pittiness = value < SMALLEST_PARAM ? 0f : Mathf.Min(1.0f, value);
		}
	}


	/// <summary>
	/// Gets or sets the platforminess which determines how often platforms appear.
	/// </summary>
	/// <value>The platforminess.</value>
	public float Platforminess
	{
		get
		{
			return platforminess;
		}
		set
		{
			platforminess = value < 0 ? SMALLEST_PARAM : Mathf.Min(1.0f, value);
		}
	}

	/// <summary>
	/// Gets or sets the caviness, which determines how far apart the ground and ceiling should be.
	/// </summary>
	/// <value>The caviness.</value>
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

	private float platforminess;
	private float hilliness;
	private float pittiness;
	private float caviness;
}
