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
}
