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
    private EnemySection enemySections;

	public int[,] Grid
	{
		get
		{
			return grid;
		}
	}

	public SectionAttributes Sprites
	{
		get
		{
			return sprites;
		}
	}

	public Section(int[,] sectionGrid, SectionAttributes sectionSprites)
	{
		grid = sectionGrid;
		sprites = sectionSprites;
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
