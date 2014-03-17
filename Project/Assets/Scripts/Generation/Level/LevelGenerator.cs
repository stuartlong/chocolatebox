using UnityEngine;
using System.Collections;
using System;

public class LevelGenerator : MonoBehaviour 
{
	public int seed = -1;
	public int sectionsY;
	public int sectionsX;
	public Vector2 levelSize;
	public SpriteRenderer groundBlock;
	public bool openLevel;
	public PlayerAttachment player;
	public int MergeChance;

	public void Start () 
	{
		//TODO a temporary fix until we work up a custom unity interface
		if (seed == -1)
		{
			DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
			seed = (int) (System.DateTime.UtcNow - epochStart).TotalSeconds;
		}
		UnityEngine.Random.seed = seed;
		//A grid of sections
		int[,][,] master = new int[sectionsX,sectionsY][,];

		//build each section
		SectionBuilder lastSection = null;
		for (int width = 0; width < master.GetLength(0); width++)
		{
			for (int height = 0; height < master.GetLength(1); height++)
			{
				EntrancePositions entrances;
				if (lastSection == null) 
				{
					entrances = new EntrancePositions(new EntrancePosition(UnityEngine.Random.Range(1,(int) (levelSize.y/sectionsY)),2),new EntrancePosition(),new EntrancePosition(),new EntrancePosition());
				}
				else
				{
					entrances = new EntrancePositions(lastSection.finalEntrancePositions.eastEntrance, new EntrancePosition(), new EntrancePosition(), new EntrancePosition());
				}
				Vector2 scaleNewSection = new Vector2(levelSize.x/sectionsX, levelSize.y/sectionsY);
				SBParams sbParams = new SBParams();
				sbParams.size = scaleNewSection;
				sbParams.entrancePositions = entrances;
				sbParams.Pittiness = 0.09f;
				sbParams.Hilliness = .09f;

				SectionBuilder newSection = new SectionBuilder(this, sbParams);
				int[,] section = newSection.Build();

				//Store each section in master
				master[width, height] = section;
				lastSection = newSection;
			}
		}

		//merge random sections
		//for each section border shared with another section roll a number
		//each section border will be compared only once by going through each section and checking their top and right edges
		for (int width = 0; width < master.GetLength(0)-1; width++)
		{
			for (int height = 0; height < master.GetLength(1); height++)
			{
				//Determine if you want to merge right
				if (UnityEngine.Random.Range(0,100) < MergeChance)
				{
					//Merge to the right
					for (int j = 0; j < master[width,height].GetLength(1); j++)
					{
						//Don't overwrite entrances
						if (master[width,height][1,j] != (int) AssetTypeKey.Entrance)
						{
							//overwrite right most tiles with second to leftmost tiles in next section
							master[width,height][master[width,height].GetLength(0)-1,j] = 
								master[width+1,height][1,j];
							//overwrite the tiles in the first section now
							master[width+1,height][0,j] = 
								master[width,height][master[width,height].GetLength(0)-2,j];
						}
					}
				}
			}
		}

		//generate the sections using the representative arrays
		for (int width = 0; width < master.GetLength(0); width++)
		{
			for (int height = 0; height < master.GetLength(1); height++)
			{
				for (int i = 0; i < master[width,height].GetLength(0); i++)
				{
					for (int j = 0; j < master[width,height].GetLength(1); j++)
					{
						if (master[width,height][i,j] == (int) AssetTypeKey.GroundBlock)
						{
							float centerX = groundBlock.sprite.bounds.extents.x  * 2 * i + (
								groundBlock.sprite.bounds.extents.y * 2 * master[width,height].GetLength(0)* width);
							float centerY = groundBlock.sprite.bounds.extents.y * 2 * j + (
								groundBlock.sprite.bounds.extents.y * 2 * master[width,height].GetLength(1) * height);
							Instantiate(groundBlock, new Vector3(centerX,centerY,0), new Quaternion());
                            
							//Debug.Log (centerX + ", " + centerY);
						}
					}
				}
			}
		}
	}
	
	public void Update () {

	}

	public enum AssetTypeKey 
	{
		None = 0,
		GroundBlock = 1,
		Entrance = 2,
		Pit = 3
	}
}
