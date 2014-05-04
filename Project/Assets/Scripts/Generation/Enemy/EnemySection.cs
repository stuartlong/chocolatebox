using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Enemy Sections are generated when the ground or the ceiling height of the level changes. 
/// In this way, depending on the size and location of the section, we can allocate enemies to be placed
/// 
/// Primary Author - James Fitzpatrick
/// </summary>


public class EnemySection
{
    public int leftBound;
    public int rightBound;
    public int upperBound;
    public int lowerBound;

	public EnemySection(int left, int right, int up, int down)
	{
        leftBound = left;
        rightBound = right;
        upperBound = up;
        lowerBound = down;
	}
}
