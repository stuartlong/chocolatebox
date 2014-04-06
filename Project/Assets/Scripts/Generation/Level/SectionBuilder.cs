﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// SectionBuilder's are used to generate square, individual
/// sections of a level. They are characterzied both by a passed in
/// SBParams object and the LevelGenerator
/// associated with this SectionBuilder.
/// 
/// Primary Author - Stuart Long
/// </summary>
public class SectionBuilder {
	//These are later changed ONCE in constructor to account for difficulty since appropriate scope is required
	private int MIN_PIT_LENGTH = 3;
	private int MAX_PITS = 3;

	/// <summary>
	/// The final entrance positions after building.
	/// </summary>
	public EntrancePositions finalEntrancePositions;

	public int finalCeilingHeight;

	private LevelGenerator generator;
	private int numberBlocksX;
	private int numberBlocksY;
	private int[,] section;
	private int groundHeight;
	private int ceilingHeight;
	private int blocksSinceLastChange;
	private List<int> pits;
	private SBParams sbParams;

	/// <summary>
	/// Initializes a new instance of the <see cref="SectionBuilder"/> class.
	/// </summary>
	/// <param name="mainGenerator">The <see cref="LevelGenerator"/> using this SectionBuilder. </param>
	/// <param name="sbParams">The various parameters characterizing this <see cref="SectionBuilder"/></param>
	public SectionBuilder(LevelGenerator mainGenerator, SBParams sectionParams)
	{
		sbParams = sectionParams;
		generator = mainGenerator;
		numberBlocksX = ConvertToBlocksX(sbParams.size.x);
		numberBlocksY = ConvertToBlocksY(sbParams.size.y);
		section = new int[numberBlocksX,numberBlocksY];
		groundHeight = sbParams.entrancePositions.westEntrance.location - 1;
		ceilingHeight = sbParams.ceilingHeight;

		blocksSinceLastChange = 0;
		finalEntrancePositions = new EntrancePositions(sbParams.entrancePositions);
		pits = new List<int>();

		MAX_PITS = (int)(2 + 4 * sbParams.difficulty);
		MIN_PIT_LENGTH = (int)(2 + 3 * sbParams.difficulty);
	}

	/// <returns>
	/// A 2D int array representing the built section.
	/// Uses <see cref="LevelGenerator.AssetTypeKey"/> as keys.
	/// </returns>
	public Section Build()
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

			if (ShouldChangeCeilingHeight(x))
			{
				ChangeCeilingHeightIfAble(x);
			}

			if (x == numberBlocksX-1)
			{
				if (sbParams.lastSection)
				{
					for (int i = groundHeight + 1; i < Mathf.Min(numberBlocksY, (int) (groundHeight + ConvertToBlocksY(generator.levelEnd.bounds.size.y))); i++)
					{
						section[x, i] = (int) LevelGenerator.AssetTypeKey.Empty;
					}

					section[x, (int) (groundHeight + 1 + ConvertToBlocksY(generator.levelEnd.bounds.extents.y))] = (int) LevelGenerator.AssetTypeKey.LevelEnd;
				}
				else
				{
					CreateEastWestEntrance(x, groundHeight+1);
					finalEntrancePositions.eastEntrance.location = groundHeight + 1;
				}
			}


			for (int y = 0; y < numberBlocksY; y++)
			{
				if (!(section[x,y] == (int) LevelGenerator.AssetTypeKey.None))
				{
					continue;
				}

				bool belowGround = y < groundHeight;
				bool atGround = y == groundHeight;
				bool atWall = !generator.openLevel && (x == 0 || x == numberBlocksX-1);
				bool atCeiling = !generator.openLevel && (y >= ceilingHeight);
				bool shouldPit = makingPit && y <= groundHeight;

				//CreateExits(x, y);
				if (shouldPit) 
				{
					section[x,y] = (int) LevelGenerator.AssetTypeKey.Pit;
				}
				else if (belowGround)
				{
					section[x,y] = (int) LevelGenerator.AssetTypeKey.UndergroundBlock;

				}
				else if (atGround)
				{
					blocksSinceLastChange++;
					section[x,y] = (int) LevelGenerator.AssetTypeKey.TopGroundBlock;
				}
				/*else if (atWall && !atCeiling)
				{
					section[x,y] = (int) LevelGenerator.AssetTypeKey.WallBlock;
				}*/
				else if (atCeiling)
				{
					section[x,y] = (int) LevelGenerator.AssetTypeKey.CeilingBlock;
				}
			}
		}

		finalCeilingHeight = ceilingHeight;

		return new Section(section, sbParams.sprites);
	}

	#region Pit Creation
	//determines which columns of the sections should represent pits
	private void DeterminePits()
	{
		if (!sbParams.allowPits)
		{
			return;
		}

		bool makingPit = false;
		int numberPits = 0;
		int pitLength = 0;
		int firstPit = Random.Range(2,(int)(sbParams.size.x * (1-.75f * sbParams.difficulty)));
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
				int maxLength = ConvertToBlocksX(generator.player.maxJumpDistance.x);
				currentMaxPitLength = Random.Range(MIN_PIT_LENGTH, Mathf.Min(maxLength+1, numberBlocksX - 1 - x));
				pitLength = 0;
			}
		}
	}

	//returns true if it's time to make a new pit
	private bool ShouldMakePit(int currentX, int lastPitX)
	{
		if (currentX >= numberBlocksX - 2 || currentX - lastPitX < 2)
		{
			return false;
		}

		bool shouldPit = (Random.Range(0f,1f) + (sbParams.difficulty-.3f)/4f) > 1-sbParams.Pittiness;

		//TODO work up a good algorithm for increasing the chance a pit appears proporitional to the difficulty and how long it's been since we've seen a pit
		/*if (lastPitX != -1)
		{
			shouldPit &= Beta(PercentChangeFromOne(lastPitX)) > Random.Range(.4f*(1-sbParams.Pittiness),1f*sbParams.Pittiness);
		}*/

		return shouldPit;
	}
	#endregion

	#region Ceiling Height
	private bool ShouldChangeCeilingHeight(int currentX)
	{
		return Random.Range(0f, 1f) > 1 - sbParams.Hilliness || ceilingHeight < (int) (groundHeight + 2 + generator.player.maxPlayerSize.y);
	}

	private void ChangeCeilingHeightIfAble(int currentX)
	{
		int minVal = (int) (groundHeight + 2 + generator.player.maxPlayerSize.y);
		bool goUp = Random.Range(0f, 1f) > sbParams.Caviness || ceilingHeight < (int) minVal;

		if (goUp)
		{
			ceilingHeight = Mathf.Min(ceilingHeight + 1, numberBlocksY - 1);
		}
		else
		{
			ceilingHeight = Mathf.Max(ceilingHeight - 1, minVal);
		}
	}
	#endregion

	#region Ground Height
	//returns true if the ground height should be changed
	private bool ShouldChangeGroundHeight(int currentX) 
	{
		if (currentX <= 1 || currentX >= numberBlocksX - 2)
		{
			return false;
		}

		return Random.Range(0f,1f) > 1-sbParams.Hilliness;
	}

	//changes the ground height. Direction is completely random.
	private void ChangeGroundHeightIfAble(int currentX)
	{
		float r = Random.Range(0f,1f);
		bool goUp = sbParams.OnlyGoesUp || r >= .5;
		
		int maxJump = (int) generator.player.maxJumpDistance.y;
		//int difference = (int)((float) maxJump * Beta(Random.Range(0f,1f)));
		if ((int) (ceilingHeight - groundHeight - generator.player.maxPlayerSize.y - 1) <= 0)
		{
			goUp = false;
		}

		int difference = Random.Range(1,maxJump);
		if (goUp)
		{
			groundHeight = Mathf.Min((int) (numberBlocksY - (generator.player.maxPlayerSize.y + 1) - 1), (int) (groundHeight + difference));
		}
		else
		{
			groundHeight = Mathf.Max(0, groundHeight - difference);
		}

		blocksSinceLastChange = 0;
	}
	#endregion

	#region Entrances
	//creates an entryway with a random height at the passed x-column and entrancePos-row
	private void CreateEastWestEntrance(int xCoord, int entrancePos)
	{
		for (int i = entrancePos; i < ceilingHeight; i++)
		{
			section[xCoord, i] = (int) LevelGenerator.AssetTypeKey.Entrance;
		}
	}

	//creates all of the section entrance based on it's passed in entrancePositions
	private void CreateEntrances()
	{
		if (sbParams.entrancePositions.westEntrance.location >= 0)
		{
			CreateEastWestEntrance(0, sbParams.entrancePositions.westEntrance.location);
		}

		if (sbParams.entrancePositions.eastEntrance.location >= 0)
		{
			CreateEastWestEntrance(0, sbParams.entrancePositions.eastEntrance.location);
		}

		if (sbParams.entrancePositions.southEntrance.location >= 0)
		{
			section[sbParams.entrancePositions.southEntrance.location, 0] = (int) LevelGenerator.AssetTypeKey.Entrance;;
			section[sbParams.entrancePositions.southEntrance.location + 1, 0] = (int) LevelGenerator.AssetTypeKey.Entrance;;
		}

		if (sbParams.entrancePositions.northEntrance.location >= 0)
		{
			section[sbParams.entrancePositions.northEntrance.location, numberBlocksY - 1] = (int) LevelGenerator.AssetTypeKey.Entrance;;
			section[sbParams.entrancePositions.northEntrance.location + 1, numberBlocksY - 1] = (int) LevelGenerator.AssetTypeKey.Entrance;;
		}
	}
	#endregion

	private int ConvertToBlocksY(float unityUnitsY)
	{
		return (int) (unityUnitsY / (generator.GetBaseBlock().sprite.bounds.extents.y * 2));
	}

	private int ConvertToBlocksX(float unityUnitsX)
	{
		return (int) (unityUnitsX / (generator.GetBaseBlock().sprite.bounds.extents.x * 2));
	}

	private float PercentChangeFromOne(float x)
	{
		return ((float)(x - 1) / x);
	}
	
	//beta probilitity distribution
	private float Beta(float x)
	{
		float alpha = 3;
		float beta = 1;
		return Mathf.Pow(x, alpha - 1)*Mathf.Pow(1-x,beta-1) / 3f;
	}
}
