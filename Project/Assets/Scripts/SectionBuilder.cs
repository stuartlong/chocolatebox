using UnityEngine;
using System.Collections;

public class SectionBuilder {
	private Vector2 size;
	private LevelGenerator generator;

	public SectionBuilder(Vector2 sectionSize, LevelGenerator mainGenerator)
	{
		size = sectionSize;
		generator = mainGenerator;
	}

	public int[,] Build(int entrancePosition)
	{
		int numberBlocksX = (int) (size.x / generator.groundBlock.sprite.bounds.extents.x  * 2);
		int numberBlocksY = (int) (size.y / generator.groundBlock.sprite.bounds.extents.y * 2);
		int[,] section = new int[numberBlocksX,numberBlocksX];
		float centerX = 0f;

		section[0,entrancePosition] = (int) LevelGenerator.AssetTypeKey.Entrance;
		section[0,entrancePosition + 1] = (int) LevelGenerator.AssetTypeKey.Entrance;
		section[numberBlocksY-1, entrancePosition] = (int) LevelGenerator.AssetTypeKey.Entrance;
		section[numberBlocksY-1, entrancePosition + 1] = (int) LevelGenerator.AssetTypeKey.Entrance;

		for (int i = 0; i < numberBlocksX; i++) 
		{
			for (int j = 0; j < numberBlocksY; j++)
			{
				//for now just fill section border with blocks
				if (section[i,j] == (int) LevelGenerator.AssetTypeKey.None &&
				    (j == 0 || j == numberBlocksY-1 || i == 0 || i == numberBlocksY-1))
				{
					section[i,j] = (int) LevelGenerator.AssetTypeKey.GroundBlock;
				}
			}
		}

		return section;
	}
}
