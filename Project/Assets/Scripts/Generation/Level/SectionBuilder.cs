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
	private List<int> platforms;
	private SBParams sbParams;
	private int[] groundHeights;
	private int[] ceilingHeights;
	private int platformHeight;
	private int lastPlatform;

	/// <summary>
	/// Initializes a new instance of the <see cref="SectionBuilder"/> class.
	/// </summary>
	/// <param name="mainGenerator">The <see cref="LevelGenerator"/> using this SectionBuilder. </param>
	/// <param name="sbParams">The various parameters characterizing this <see cref="SectionBuilder"/></param>
	public SectionBuilder(LevelGenerator mainGenerator, SBParams sectionParams)
	{
		sbParams = sectionParams;
		generator = mainGenerator;
		numberBlocksX = generator.ConvertToBlocksX(sbParams.size.x);
		numberBlocksY = generator.ConvertToBlocksY(sbParams.size.y);
		section = new int[numberBlocksX,numberBlocksY];
		groundHeight = sbParams.entrancePositions.westEntrance.location - 1;
		ceilingHeight = sbParams.ceilingHeight;

		groundHeights = new int[numberBlocksX];
		ceilingHeights = new int[numberBlocksX];

		platformHeight = -1;
		lastPlatform = -1;

		blocksSinceLastChange = 0;
		finalEntrancePositions = new EntrancePositions(sbParams.entrancePositions);
		pits = new List<int>();
		platforms = new List<int>();

		//MAX_PITS = (int)(1 + 3 * sbParams.difficulty);

		//MIN_PIT_LENGTH = (int)(2 + 2 * sbParams.difficulty);
		MIN_PIT_LENGTH = (int) generator.player.maxPlayerSize.x + 1;
		MAX_PITS = (int)(numberBlocksX / (MIN_PIT_LENGTH * MIN_PIT_LENGTH) * sbParams.Pittiness);
	}

	/// <returns>
	/// A 2D int array representing the built section.
	/// Uses <see cref="LevelGenerator.AssetTypeKey"/> as keys.
	/// </returns>
	public Section Build()
	{
		CreateEntrances();
		DeterminePits();
		DeterminePlatforms();

		for (int x = 0; x < numberBlocksX; x++) 
		{
			bool makingPit = pits.Contains(x);
			bool makingPlatform = platforms.Contains(x);

			if (makingPlatform)
			{
				lastPlatform = x;
			}

			if (!makingPlatform && platforms.Contains(x + 1))
			{
				int minVal = -1;
				int maxVal = -1;

					if (lastPlatform != -1 && x - lastPlatform < generator.player.maxJumpDistance.x)
					{
						maxVal = (int) Mathf.Min(platformHeight + generator.player.maxJumpDistance.y / 2, ceilingHeight - generator.player.maxPlayerSize.y - 1);
					}
					else
					{
						maxVal = (int) (groundHeight + generator.player.maxJumpDistance.y - 1);
					}

				minVal = (int) (groundHeight + generator.player.maxPlayerSize.y + 1);

				platformHeight = Random.Range(minVal, maxVal); 
			}

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
					for (int i = groundHeight + 1; i < Mathf.Min(numberBlocksY, (int) (groundHeight + generator.ConvertToBlocksY(generator.levelEnd.bounds.size.y) + 1)); i++)
					{
						section[x, i] = (int) LevelGenerator.AssetTypeKey.Empty;
					}

					//section[x, (int) (groundHeight + 1 + generator.ConvertToBlocksY(generator.levelEnd.bounds.extents.y))] = (int) LevelGenerator.AssetTypeKey.LevelEnd;
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
				bool platform = makingPlatform && y == platformHeight;

				if (platform)
				{
					section[x,y] = (int) LevelGenerator.AssetTypeKey.Platform;
				}
				else if (shouldPit) 
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
				else if (atCeiling)
				{
					section[x,y] = (int) LevelGenerator.AssetTypeKey.CeilingBlock;
				}
			}

			groundHeights[x] = groundHeight;
			ceilingHeights[x] = ceilingHeight;
		}

		finalCeilingHeight = ceilingHeight;

		return new Section(section, sbParams.sprites,ceilingHeights, groundHeights, pits);
	}

	#region Platform Creation
	private void DeterminePlatforms()
	{
		if (!sbParams.allowPlatforms)
		{
			return;
		}

		bool makingPlatform = false;
		int numberPlatforms = 0;
		int platformLength = 0;
		int firstPlatform = Random.Range(2,(int) (sbParams.size.x / 2));
		int currentMaxPlatformLength = 0;
		
		for (int x = 0; x < numberBlocksX; x++) 
		{
			if (x < firstPlatform)
			{
				continue;
			}
			
			if (platformLength < currentMaxPlatformLength)
			{
				platforms.Add(x);
				platformLength++;
				continue;
			}

			int lastP = platforms.Count < 1 ? -1 : platforms.Last();
			
			if (ShouldMakePlatform(x, lastP))
			{
				currentMaxPlatformLength = Random.Range((int) generator.player.maxPlayerSize.x,(int) (generator.player.maxPlayerSize.x*generator.player.maxPlayerSize.x)); 
				platformLength = 0;
			}
		}
	}

	private bool ShouldMakePlatform(int currentX, int lastPlatformX)
	{
		if (currentX >= numberBlocksX - 2 || currentX - lastPlatformX < generator.player.maxPlayerSize.x || pits.Contains(currentX))
		{
			return false;
		}

		float rand1 = Random.Range(0f,1f);
		float rand2 = Random.Range(0f, 1f);
		float rand3 = Random.Range(0f, 1f);

		bool shouldPlatform = rand1 > 1f - sbParams.Platforminess && rand2 > 1f - sbParams.Platforminess && rand3 > 1 - sbParams.Platforminess;
		return shouldPlatform;
	}
	#endregion

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
		//int firstPit = Random.Range(2,(int)(sbParams.size.x * (1-.5f * sbParams.difficulty)));
		int firstPit = Random.Range(2, (int) Random.Range(numberBlocksX / (4 * sbParams.Pittiness), numberBlocksX / (2 * sbParams.Pittiness)));
		int currentMaxPitLength = 0;
		int nextPitSpace = 0;

		for (int x = 0; x < numberBlocksX; x++) 
		{
			if (x < firstPit || numberPits >= MAX_PITS || x < nextPitSpace)
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
				numberPits++;
				int maxLength = (int) generator.player.maxJumpDistance.x;
				//currentMaxPitLength = Random.Range(MIN_PIT_LENGTH, Mathf.Min((int)((maxLength+1)*(sbParams.difficulty/4f + .75)), (numberBlocksX - 1 - x)));
				currentMaxPitLength = Random.Range(MIN_PIT_LENGTH, Mathf.Min((int)(maxLength+1), (numberBlocksX - 1 - x)));
				int nextMin = (int) (generator.player.maxPlayerSize.x + x + currentMaxPitLength);
				nextPitSpace = Random.Range(nextMin, nextMin + Random.Range(1, (int) ((numberBlocksX - x) * (1 - sbParams.Pittiness))));
				pitLength = 0;
				Debug.Log (currentMaxPitLength);
			}
		}
	}

	//returns true if it's time to make a new pit
	private bool ShouldMakePit(int currentX, int lastPitX)
	{
		if (currentX >= numberBlocksX - generator.player.maxPlayerSize.x
		    || currentX - lastPitX < generator.player.maxPlayerSize.x / sbParams.Pittiness)
		{
			return false;
		}

		bool shouldPit = Random.Range(0f,1f) /*+ (sbParams.difficulty-.5f)/4f)*/ > 1-sbParams.Pittiness;

		return shouldPit;
	}
	#endregion

	#region Ceiling Height
	private bool ShouldChangeCeilingHeight(int currentX)
	{
		bool randomChange = Random.Range(0f, 1f) > 1 - sbParams.Hilliness;
		bool tooShort = ceilingHeight < (int) (groundHeight + 2 + generator.player.maxPlayerSize.y);
		bool platformChange = !platforms.Contains(currentX + 1) ? false : ceilingHeight < (int) (platformHeight + 1 + generator.player.maxPlayerSize.y);
		return randomChange || tooShort || platformChange;
	}

	private void ChangeCeilingHeightIfAble(int currentX)
	{
		int minVal = (int) (groundHeight + 2 + generator.player.maxPlayerSize.y);
		if (platforms.Contains(currentX + 1) || platforms.Contains(currentX))
		{
			minVal = Mathf.Max((int) minVal, (int) (platformHeight + 1 + generator.player.maxPlayerSize.y));
		}

		bool goUp = Random.Range(0f, 1f) > sbParams.Caviness || ceilingHeight < (int) minVal;

		if (goUp)
		{
			ceilingHeight = Mathf.Min(Mathf.Max(ceilingHeight + 1, minVal), numberBlocksY - 1);
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
		if (currentX <= 1 
		    || currentX >= numberBlocksX - 2 
		    || blocksSinceLastChange < generator.player.maxPlayerSize.x)
		{
			return false;
		}

		if ((int) (ceilingHeight - groundHeight - generator.player.maxPlayerSize.y - 1) <= 0)
		{
			return true;
		}

		return Random.Range(0f,1f) > 1-sbParams.Hilliness && Random.Range(0f,1f) > 1-sbParams.Hilliness;
	}

	//changes the ground height. Direction is completely random.
	private void ChangeGroundHeightIfAble(int currentX)
	{
		float r = Random.Range(0f,1f);
		bool goUp = sbParams.OnlyGoesUp || r >= .5;
		
		int maxJump = (int) generator.player.maxJumpDistance.y;
		//int difference = (int)((float) maxJump * Beta(Random.Range(0f,1f)));
		if ((int) (ceilingHeight - groundHeight - generator.player.maxPlayerSize.y - 1) <= 0
		    || platforms.Contains(currentX))
		{
			goUp = false;
		}

		int difference = Random.Range(1,maxJump);
		if (goUp)
		{
			groundHeight = Mathf.Min((int) (numberBlocksY - (generator.player.maxPlayerSize.y + 1) - 1), (int) (groundHeight + difference), (int) (ceilingHeight - generator.player.maxPlayerSize.y + 1));
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

	private float PercentChangeFromOne(float x)
	{
		return ((float)(x - 1) / x);
	}

	private float NormalApprox(float min, float max)
	{
		return (Random.Range(min, max) + Random.Range(min, max) + Random.Range(min, max)) / 3;
	}

	private int NormalApprox(int min, int max)
	{
		return (Random.Range(min, max) + Random.Range(min, max) + Random.Range (min, max)) / 3;
	}
	
	//beta probilitity distribution
	private float Beta(float x)
	{
		float alpha = 3;
		float beta = 1;
		return Mathf.Pow(x, alpha - 1)*Mathf.Pow(1-x,beta-1) / 3f;
	}
}
