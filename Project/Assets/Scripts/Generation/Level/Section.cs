using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A high level abstraction of a section
/// Primary Author - Stuart Long
/// </summary>
public class Section
{
	private int[,] grid;
	private int[,] decorationGrid;
	private SectionAttributes sprites;
	private int[] ceilingHeights;
	private int[] groundHeights;
	private List<int> groundDecorationIndeces;
    private EnemySection enemySections;
	private List<int> pitIndeces;

	public List<int> PitIndeces
	{
		get
		{
			return pitIndeces;
		}
	}

	public List<int> GroundDecorationIndeces
	{
		get
		{
			return groundDecorationIndeces;
		}
	}

	public int[,] DecorationGrid
	{
		get
		{
			return decorationGrid;
		}
	}

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

	public Section(int[,] sectionGrid, SectionAttributes sectionSprites, int[] ceilings, int[] grounds, List<int> pits)
	{
		grid = sectionGrid;
		decorationGrid = new int[sectionGrid.GetLength(0), sectionGrid.GetLength(1)];
		sprites = sectionSprites;
		ceilingHeights = ceilings;
		groundHeights = grounds;
		groundDecorationIndeces = new List<int>();
		pitIndeces = pits;
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
