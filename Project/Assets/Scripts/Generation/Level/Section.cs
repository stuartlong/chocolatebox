using UnityEngine;
using System.Collections;

/// <summary>
/// A high level abstraction of a section
/// Primary Author - Stuart Long
/// </summary>
public class Section
{
	private int[,] grid;

	public Section(int[,] sectionGrid)
	{
		grid = sectionGrid;
	}

	public int[,] getGrid()
	{
		return grid;
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
