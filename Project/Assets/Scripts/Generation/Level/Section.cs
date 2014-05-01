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
  	private List<EnemySection> enemySections;

  	[HideInInspector]
	private int[] ceilingHeights;
	private int[] groundHeights;
	private List<int> groundDecorationIndeces;
    	public RangeTree<EnemyAttachment> enemyTree;
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

    public List<EnemySection> EnemySections
    {
        get
        {
            return enemySections;
        }
    }


	public Section(int[,] sectionGrid, SectionAttributes sectionSprites, List<EnemySection> es)
	{
		grid = sectionGrid;
		decorationGrid = new int[sectionGrid.GetLength(0), sectionGrid.GetLength(1)];
		sprites = sectionSprites;
       		enemySections = es;
		enemyTree = new RangeTree<EnemyAttachment>();
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


    public void GenerateEnemyRangeTree()
    {
        foreach (EnemyAttachment enemy in sprites.enemies)
        {
            enemyTree.Add(enemy.probability, enemy);
        }

        enemyTree.Index();
    }
}
