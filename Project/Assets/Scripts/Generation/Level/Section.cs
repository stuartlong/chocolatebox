using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A high level abstraction of a section
/// Primary Author - Stuart Long
/// </summary>
public class Section
{
	public RangeTree<EnemyAttachment> enemyTree;

	private int[,] grid;
	private int[,] decorationGrid;
	private SectionAttributes attributes;
  	private List<EnemySection> enemySections;

	private int[] ceilingHeights;
	private int[] groundHeights;
	private List<int> groundDecorationIndeces;
	private List<int> pitIndeces;

	public Section(int[,] sectionGrid, SectionAttributes sectionSprites, int[] ceilings, int[] grounds, List<int> pits, List<EnemySection> es)
	{
		grid = sectionGrid;
		decorationGrid = new int[sectionGrid.GetLength(0), sectionGrid.GetLength(1)];
		attributes = sectionSprites;
		enemySections = es;
		enemyTree = new RangeTree<EnemyAttachment>();
		ceilingHeights = ceilings;
		groundHeights = grounds;
		groundDecorationIndeces = new List<int>();
		pitIndeces = pits;
	}
	
	/// <summary>
	/// Gets the pit indeces represeneted as a list storing
	/// the columns in which pits appear.
	/// </summary>
	/// <value>The pit indeces.</value>
	public List<int> PitIndeces
	{
		get
		{
			return pitIndeces;
		}
	}

	/// <summary>
	/// Gets the decoration grid, an entire
	/// map of the section with a "1" appearing
	/// where decorations are.
	/// </summary>
	/// <value>The decoration grid.</value>
	public int[,] DecorationGrid
	{
		get
		{
			return decorationGrid;
		}
	}

	/// <summary>
	/// Gets the grid, the map of the section
	/// detailing where the terrain is. The values
	/// are mapped to different types via
	/// LevelGenerator.AssetTypeKey
	/// </summary>
	/// <value>The grid.</value>
	public int[,] Grid
	{
		get
		{
			return grid;
		}
	}

	/// <summary>
	/// Gets the ground heights represented as a 
	/// 1D array storing the ground height for 
	/// every column.
	/// </summary>
	/// <value>The ground heights.</value>
	public int[] GroundHeights
	{
		get
		{
			return groundHeights;
		}
	}

	/// <summary>
	/// Gets the ceiling heights represented as a 
	/// 1D array storing the ceiling height for
	/// every column.
	/// </summary>
	/// <value>The ceiling heights.</value>
	public int[] CeilingHeights
	{
		get
		{
			return ceilingHeights;
		}
	}

	/// <summary>
	/// Gets this section's attributes.
	/// </summary>
	/// <value>The attributes.</value>
	public SectionAttributes Attributes
	{
		get
		{
			return attributes;
		}
	}

    public List<EnemySection> EnemySections
    {
        get
        {
            return enemySections;
        }
    }

	/// <summary>
	/// A help method that gets the width of this
	/// section's grid.
	/// </summary>
	/// <returns>The width.</returns>
    public int getWidth()
    {
        return grid.GetLength(0);
    }

	/// <summary>
	/// A help method that gets the height of this
	/// section's grid.
	/// </summary>
	/// <returns>The height.</returns>
    public int getHeight()
    {
        return grid.GetLength(1);
    }

    public int get(int x, int y)
    {
        return grid[x, y];
    }


    public void GenerateEnemyRangeTree()
    {
        foreach (EnemyAttachment enemy in attributes.enemies)
        {
            enemyTree.Add(enemy.probability, enemy);
        }

        enemyTree.Index();
    }
}
