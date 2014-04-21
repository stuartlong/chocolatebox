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
    private int leftBound;
    private int rightBound;
    private int upperBound;
    private int lowerBound;

	public EnemySection()
	{
	}
}
