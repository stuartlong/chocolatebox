using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// The Overlord script that generates each section and combines them. This script must be
/// attached to some GameObject in your scene and it is where you can specifiy most of the
/// various parameters to determine the characteristics of your level. 
/// 
/// Primary Author(s) - Stuart Long, Frank Singel
/// </summary>
public class LevelGenerator : MonoBehaviour 
{
	public int seed;
	public int sectionsY;
	public int sectionsX;
	public Vector2 levelSize;
	public SpriteRenderer groundBlock;
	public SectionSprites[] sectionGroups;
    	public int MergeChange;
	public PlayerAttachment player;
	public Section[,] master;
	public bool openLevel;

	[HideInInspector] public bool customSeed = false;

	public void Awake () 
	{
		if (!customSeed)
		{
			DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
			seed = (int) (System.DateTime.UtcNow - epochStart).TotalSeconds;
		}
		UnityEngine.Random.seed = seed;
		//A grid of sections
		master = new int[sectionsX,sectionsY][,];
		Section[,] master = new Section[sectionsX, sectionsY];

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

				//Store each section in master
				master[width, height] = newSection.Build();
				lastSection = newSection;
			}
		}

		//TODO Section merging needs to happen before we build the actual sections
		//merge random sections
		//for each section border shared with another section roll a number
		//each section border will be compared only once by going through each section and checking their top and right edges
		/*for (int width = 0; width < master.GetLength(0)-1; width++)
		{
			for (int height = 0; height < master.GetLength(1); height++)
			{
				//Determine if you want to merge right
				if (UnityEngine.Random.Range(0,100) < MergeChance)
				{
					//Merge to the right
					for (int j = 0; j < master[width,height].GetLength(1); j++)
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
		}*/

		//generate the sections using the representative arrays
		for (int width = 0; width < master.GetLength(0); width++)
		{
			for (int height = 0; height < master.GetLength(1); height++)
			{
				for (int i = 0; i < master[width,height].getGrid().GetLength(0); i++)
				{
					for (int j = 0; j < master[width,height].getGrid().GetLength(1); j++)
					{
						if (master[width,height].getGrid()[i,j] == (int) AssetTypeKey.UndergroundBlock)
						{
							float centerX = groundBlock.sprite.bounds.extents.x  * 2 * i + (
								groundBlock.sprite.bounds.extents.y * 2 * master[width,height].getGrid().GetLength(0)* width);
							float centerY = groundBlock.sprite.bounds.extents.y * 2 * j + (
								groundBlock.sprite.bounds.extents.y * 2 * master[width,height].getGrid().GetLength(1) * height);
							Instantiate(groundBlock, new Vector3(centerX,centerY,0), new Quaternion());
						}
					}
				}
			}
		}
	}

	/// <summary>
	/// The keys for what integers represent what 
	/// in the array representations of levels.
	/// </summary>
	public enum AssetTypeKey 
	{
		None = 0,
		UndergroundBlock = 1,
		Entrance = 2,
		Pit = 3,
		WallBlock = 4,
		CeilingBlock = 5,
		TopGroundBlock = 6
	}
}
