﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// SectionBuilder's are used to generate square, individual
/// sections of a level. They are characterzied both by a passed in
/// SBParams object and the LevelGenerator
/// associated with this SectionBuilder.
/// </summary>
public class SectionBuilder {
	//TODO should be based on player
	private static int MAX_JUMP = 3;
	private static int MAX_PITS = 3;
	private static int MAX_PIT_LENGTH = 10;
	private static int MIN_PIT_LENGTH = 3;

	/// <summary>
	/// The final entrance positions after building.
	/// </summary>
	public EntrancePositions finalEntrancePositions;

	private Vector2 size;
	private LevelGenerator generator;
	private EntrancePositions givenEntrancePos;
	private int numberBlocksX;
	private int numberBlocksY;
	private int[,] section;
	private int groundHeight;
	private int blocksSinceLastChange;
	private float hilly;
	private List<int> pits;

	/// <summary>
	/// Initializes a new instance of the <see cref="SectionBuilder"/> class.
	/// </summary>
	/// <param name="mainGenerator">The <see cref="LevelGenerator"/> using this SectionBuilder. </param>
	/// <param name="sbParams">The various parameters characterizing this <see cref="SectionBuilder"/></param>
	public SectionBuilder(LevelGenerator mainGenerator, SBParams sbParams)
	{
		size = sbParams.size;
		givenEntrancePos = sbParams.entrancePositions;
		generator = mainGenerator;
		numberBlocksX = (int) (size.x / (generator.groundBlock.sprite.bounds.extents.x  * 2));
		numberBlocksY = (int) (size.y / (generator.groundBlock.sprite.bounds.extents.y * 2));
		section = new int[numberBlocksX,numberBlocksY];
		groundHeight = givenEntrancePos.westEntrance - 1;
		blocksSinceLastChange = 0;
		finalEntrancePositions = new EntrancePositions(givenEntrancePos);
		hilly = sbParams.hilliness;
		pits = new List<int>();
	}

	/// <returns>
	/// A 2D int array representing the built section.
	/// Uses <see cref="LevelGenerator.AssetTypeKey"/> as keys.
	/// </returns>
	public int[,] Build()
	{
		CreateEntrances();
		DeterminePits();

		for (int x = 0; x < numberBlocksX; x++) 
		{
			bool makingPit = pits.Contains(x);

			if (!makingPit && ShouldChangeGroundHeight(x)) 
			{
				ChangeGroundHeightIfAble(x);
			}

			if (x == numberBlocksX-1)
			{
				section[x,groundHeight+1] = (int) LevelGenerator.AssetTypeKey.Entrance;
				section[x,groundHeight+2] = (int) LevelGenerator.AssetTypeKey.Entrance;
				finalEntrancePositions.eastEntrance = groundHeight + 1;
			}


			for (int y = 0; y < numberBlocksY; y++)
			{
				//CreateExits(x, y);
				if (makingPit && y <= groundHeight) 
				{
					section[x,y] = (int) LevelGenerator.AssetTypeKey.Pit;
				} 
				else if (section[x,y] == (int) LevelGenerator.AssetTypeKey.None &&
				    (y <= groundHeight || ((generator.sectionsY > 1 || !generator.openLevel) && (x == 0 || x == numberBlocksX-1 || y == numberBlocksY-1))))
				{
					section[x,y] = (int) LevelGenerator.AssetTypeKey.GroundBlock;
					if (y == groundHeight)
					{
						blocksSinceLastChange++;
					}
				}
			}
		}

		return section;
	}

	private void DeterminePits()
	{
		bool makingPit = false;
		int numberPits = 0;
		int pitLength = 0;
		int firstPit = Random.Range(2,(int)size.x);
		int currentMaxPitLength = 0;

		for (int x = 0; x < numberBlocksX; x++) 
		{
			if (x < firstPit || numberPits >= MAX_PITS)
			{
				continue;
			}

			if (pitLength < currentMaxPitLength)
			{
				pits.Add(x);
				pitLength++;
				continue;
			}

			int lastPit = pits.Count == 0 ? -1 : pits.Last<int>();

			if (ShouldMakePit(x, lastPit))
			{
				currentMaxPitLength = Random.Range(MIN_PIT_LENGTH, Mathf.Min(MAX_PIT_LENGTH+1, numberBlocksX - 1 - x));
				pitLength = 0;
			}
		}
	}

	private bool ShouldMakePit(int currentX, int lastPitX)
	{
		if (currentX >= numberBlocksX - 2 || currentX - lastPitX < 2)
		{
			return false;
		}

		bool shouldPit = Random.Range(0f,1f) > .1;
		if (lastPitX != -1)
		{
			shouldPit &= Random.Range(0f,1f) > .5 && Beta(PercentChangeFromOne(lastPitX)) > .7;
		}

		return shouldPit;
	}

	private bool ShouldChangeGroundHeight(int currentX) 
	{
		//temporary fix so entrances line up correctly
		if (currentX == 0 || currentX >= numberBlocksX - 2)
		{
			return false;
		}

		return Beta((float) (PercentChangeFromOne(blocksSinceLastChange))) > Random.Range(0f,1f)/hilly;
	}

	private float PercentChangeFromOne(float x)
	{
		return ((x - 1) / x);
	}

	private float Beta(float x)
	{
		float alpha = 3;
		float beta = 1;
		return Mathf.Pow(x, alpha - 1)*Mathf.Pow(1-x,beta-1);
	}

	private void ChangeGroundHeightIfAble(int currentX)
	{
		float r = Random.Range(0f,1f);
		bool goUp = r >= .5;

		int difference = (MAX_JUMP+1 - (int) Mathf.Sqrt(Random.Range(1,(MAX_JUMP+1)*(MAX_JUMP+1))));

		if (goUp)
		{
			groundHeight = Mathf.Min(numberBlocksY-1, groundHeight + difference);
		}
		else
		{
			groundHeight = Mathf.Max(0, groundHeight - difference);
		}
		blocksSinceLastChange = 0;
	}

	private void CreateEntrances()
	{
		if (givenEntrancePos.westEntrance >= 0)
		{
			section[0,givenEntrancePos.westEntrance] = (int) LevelGenerator.AssetTypeKey.Entrance;
			section[0,givenEntrancePos.westEntrance + 1] = (int) LevelGenerator.AssetTypeKey.Entrance;
		}

		if (givenEntrancePos.eastEntrance >= 0)
		{
			section[numberBlocksX - 1,givenEntrancePos.eastEntrance] = (int) LevelGenerator.AssetTypeKey.Entrance;
			section[numberBlocksX - 1,givenEntrancePos.eastEntrance + 1] = (int) LevelGenerator.AssetTypeKey.Entrance;
		}

		if (givenEntrancePos.southEntrance >= 0)
		{
			section[givenEntrancePos.southEntrance, 0] = (int) LevelGenerator.AssetTypeKey.Entrance;;
			section[givenEntrancePos.southEntrance + 1, 0] = (int) LevelGenerator.AssetTypeKey.Entrance;;
		}

		if (givenEntrancePos.northEntrance >= 0)
		{
			section[givenEntrancePos.northEntrance, numberBlocksY - 1] = (int) LevelGenerator.AssetTypeKey.Entrance;;
			section[givenEntrancePos.northEntrance + 1, numberBlocksY - 1] = (int) LevelGenerator.AssetTypeKey.Entrance;;
		}
	}
}