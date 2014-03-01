using UnityEngine;
using System.Collections;

public class SectionBuilder {
	//TODO should be based on player
	private static int MAX_JUMP = 3;
	private static int MAX_PITS = 3;
	private static int MAX_PIT_LENGTH = 10;
	private static int MIN_PIT_LENGTH = 3;

	private Vector2 size;
	private LevelGenerator generator;
	private EntrancePositions givenEntrancePos;
	public EntrancePositions finalEntrancePositions;
	private int numberBlocksX;
	private int numberBlocksY;
	private int[,] section;
	private int groundHeight;
	private int blocksSinceLastChange;
	private float hilly;
	private int numberPits;
	private bool makingPit;
	private int pitLength;
	private int lastPitX;
	private int firstPit;
	private int currentMaxPitLength;

	public SectionBuilder(Vector2 sectionSize, LevelGenerator mainGenerator, EntrancePositions entrancePosition, float hilliness)
	{
		size = sectionSize;
		givenEntrancePos = entrancePosition;
		generator = mainGenerator;
		numberBlocksX = (int) (size.x / generator.groundBlock.sprite.bounds.extents.x  * 2);
		numberBlocksY = (int) (size.y / generator.groundBlock.sprite.bounds.extents.y * 2);
		section = new int[numberBlocksX,numberBlocksY];
		groundHeight = entrancePosition.westEntrance - 1;
		blocksSinceLastChange = 0;
		finalEntrancePositions = new EntrancePositions(givenEntrancePos);
		hilly = hilliness;
		numberPits = 0;
		makingPit = false;
		pitLength = 0;
		lastPitX = -1;
		firstPit = Random.Range(2,(int)sectionSize.x);
		currentMaxPitLength = 0;
	}

	public int[,] Build()
	{
		CreateEntrances();

		for (int x = 0; x < numberBlocksX; x++) 
		{
			if (!makingPit)
			{
				makingPit = ShouldMakePit(x);
			}
			else if (pitLength >= currentMaxPitLength)
			{
				lastPitX = x;
				makingPit = false;
			}

			if (makingPit)
			{
				pitLength++;
			}
			else if (ShouldChangeGroundHeight(x)) 
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

	private bool ShouldMakePit(int currentX)
	{
		if (currentX <= firstPit || currentX >= numberBlocksX - 2 || numberPits >= MAX_PITS || currentX - lastPitX < 2)
		{
			return false;
		}

		bool shouldPit = Random.Range(0f,1f) > .9;
		if (lastPitX != -1)
		{
			shouldPit &= Random.Range(0f,1f) > .5 && Beta(PercentChangeFromOne(lastPitX)) > .7;
		}

		if (!shouldPit)
		{
			pitLength = 0;
		}
		else
		{
			currentMaxPitLength = Random.Range(MIN_PIT_LENGTH, Mathf.Min(MAX_PIT_LENGTH+1, numberBlocksX - 1 - currentX));
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
