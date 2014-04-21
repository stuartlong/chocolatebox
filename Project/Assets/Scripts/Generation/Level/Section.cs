using UnityEngine;
using System.Collections;

/// <summary>
/// A high level abstraction of a section
/// Primary Author - Stuart Long
/// </summary>
public class Section
{
	private int[,] grid;
	private SectionAttributes sprites;
	private int[] ceilingHeights;
	private int[] groundHeights;
    private EnemySection enemySections;

	public int[,] Grid
	{
		get
		{
			return grid;
		}
	}

	public int[] GroundHeights
	{
		get
		{
			return groundHeights;
		}
	}

	public int[] CeilingHeights
	{
		get
		{
			return ceilingHeights;
		}
	}

	public SectionAttributes Sprites
	{
		get
		{
			return sprites;
		}
	}

	public Section(int[,] sectionGrid, SectionAttributes sectionSprites, int[] ceilings, int[] grounds)
	{
		grid = sectionGrid;
		sprites = sectionSprites;
		ceilingHeights = ceilings;
		groundHeights = grounds;
	}

    public int getWidth()
    {
        return grid.GetLength(0);
    }

    public int getHeight()
    {
        return grid.GetLength(1);
    }

    public int get(int x, int y)
    {
        return grid[x, y];
    }
}
