using UnityEngine;
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
	//TODO determined by difficulty?
	private static int MIN_PIT_LENGTH = 3;
	private static int MAX_PITS = 3;

	/// <summary>
	/// The final entrance positions after building.
	/// </summary>
	public EntrancePositions finalEntrancePositions;

	private LevelGenerator generator;
	private int numberBlocksX;
	private int numberBlocksY;
	private int[,] section;
	private int groundHeight;
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
		blocksSinceLastChange = 0;
		finalEntrancePositions = new EntrancePositions(sbParams.entrancePositions);
		pits = new List<int>();
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

			if (x == numberBlocksX-1)
			{
				CreateEastWestEntrance(x, groundHeight+1);
				finalEntrancePositions.eastEntrance.location = groundHeight + 1;
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
				bool atCeiling = !generator.openLevel && (y == numberBlocksY-1);
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
				else if (atWall && !atCeiling)
				{
					section[x,y] = (int) LevelGenerator.AssetTypeKey.WallBlock;
				}
				else if (atCeiling)
				{
					section[x,y] = (int) LevelGenerator.AssetTypeKey.CeilingBlock;
				}
			}
		}

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
		int firstPit = Random.Range(2,(int)sbParams.size.x);
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

		bool shouldPit = Random.Range(0f,1f) > 1-sbParams.Pittiness;

		//TODO work up a good algorithm for increasing the chance a pit appears proporitional to the difficulty and how long it's been since we've seen a pit
		/*if (lastPitX != -1)
		{
			shouldPit &= Beta(PercentChangeFromOne(lastPitX)) > Random.Range(.4f*(1-sbParams.Pittiness),1f*sbParams.Pittiness);
		}*/

		return shouldPit;
	}
	#endregion

	#region Ground Height
	//returns true if the ground height should be changed
	private bool ShouldChangeGroundHeight(int currentX) 
	{
		//temporary fix so entrances line up correctly
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
		int min = (int) generator.player.maxPlayerSize.y+entrancePos;
		int max = numberBlocksY - 1;
		int max_height = Random.Range(min, max);

		if (max < min)
		{
			Debug.Log (min + ", " + max);
		}
		
		for (int i = entrancePos; i < max_height; i++)
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
		return (int) (unityUnitsY / (sbParams.sprites.groundBlocks[0].sprite.bounds.extents.y * 2));
	}

	private int ConvertToBlocksX(float unityUnitsX)
	{
		return (int) (unityUnitsX / (sbParams.sprites.groundBlocks[0].sprite.bounds.extents.x * 2));
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
