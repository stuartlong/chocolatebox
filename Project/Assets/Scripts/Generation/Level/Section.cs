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
	private SectionAttributes sprites;
    private List<EnemySection> enemySections;

    [HideInInspector]
    public TreeMap<EnemyAttachment> enemyTree;


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
		sprites = sectionSprites;
        enemySections = es;
        enemyTree = new TreeMap<EnemyAttachment>();
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


    public void GenerateEnemyTreeMap()
    {
        foreach (EnemyAttachment enemy in sprites.enemies)
        {
            enemyTree.Add(enemy.probability, enemy);
        }

        enemyTree.Index();
    }
}
