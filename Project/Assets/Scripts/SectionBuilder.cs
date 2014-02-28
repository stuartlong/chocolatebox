using UnityEngine;
using System.Collections;

public class SectionBuilder {
	private Vector2 size;
	private LevelGenerator generator;
	private EntrancePositions givenEntrancePos;
	public EntrancePositions finalEntrancePositions;
	private int numberBlocksX;
	private int numberBlocksY;
	private int[,] section;
	private int groundHeight;
	private int blocksSinceLastChange;

	public SectionBuilder(Vector2 sectionSize, LevelGenerator mainGenerator, EntrancePositions entrancePosition)
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
	}

	public int[,] Build()
	{
		CreateEntrances();

		for (int x = 0; x < numberBlocksX; x++) 
		{

			if (ShouldChangeGroundHeight(x)) 
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

				//for now just fill section border with blocks
				if (section[x,y] == (int) LevelGenerator.AssetTypeKey.None &&
				    (y <= groundHeight || ((generator.floorCount > 1 || !generator.openLevel) && (x == 0 || x == numberBlocksX-1 || y == numberBlocksY-1))))
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

	private bool ShouldChangeGroundHeight(int currentX) 
	{
		//temporary fix so entrances line up correctly
		if (currentX == 0 || currentX >= numberBlocksX - 2)
		{
			return false;
		}

		float r = Random.Range(0f,1f);
		return Mathf.Min(blocksSinceLastChange, 99) / 100f + r < 0.6;
	}

	private void ChangeGroundHeightIfAble(int currentX)
	{
		float r = Random.Range(0f,1f);
		bool goUp = r >= .5;

		if (goUp)
		{
			groundHeight = Mathf.Min(numberBlocksY-1, groundHeight + 1);
		}
		else
		{
			groundHeight = Mathf.Max(0, groundHeight - 1);
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
