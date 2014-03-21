using UnityEngine;
using System.Collections;

/// <summary>
/// A high level abstraction of a section
/// Primary Author - Stuart Long
/// </summary>
public class Section
{
	private int[,] grid;
	private SectionSprites sprites;

	public int[,] Grid
	{
		get
		{
			return grid;
		}
	}

	public SectionSprites Sprites
	{
		get
		{
			return sprites;
		}
	}

	public Section(int[,] sectionGrid, SectionSprites sectionSprites)
	{
		grid = sectionGrid;
		sprites = sectionSprites;
	}
}
