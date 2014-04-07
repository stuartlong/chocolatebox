using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

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
	public SectionAttributes[] sectionAttributes;
    public int MergeChance;
	public PlayerAttachment player;
	public SpriteRenderer levelEnd;
	public Section[,] master;
	public bool openLevel;
	public SectionAttributes globalAttributes;
	public bool customSeed = false;
	public float difficulty;

	public void Awake () 
	{
		if (!customSeed)
		{
			DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
			seed = (int) (System.DateTime.UtcNow - epochStart).TotalSeconds;
		}
		UnityEngine.Random.seed = seed;

		//determine premerge section sizes
		float xSize = levelSize.x / sectionsX;
		float ySize = levelSize.y / sectionsY;

		//List of which sections merge left
		List<int> merges = new List<int>();


		//Determine which sections to merge
		for(int section = sectionsX; section > 1; section--)
		{
			//Determine if you want to merge right
			if (UnityEngine.Random.Range(0,100) < MergeChance)
			{
				merges.Add(section-1);
				sectionsX--;
			}
		}

		//total how many sections belong in each multi-section
		float[] sectionMultiplier = new float[sectionsX];
		int focusIndex = 1;
		for(int x = 0; x < sectionsX; x++)
		{
			sectionMultiplier[x] = 1;
			while(merges.Contains(focusIndex))
			{
				sectionMultiplier[x]++;
				focusIndex++;
			}
			focusIndex++;
		}


		//A grid of sections
		master = new Section[sectionsX, sectionsY];

		//build each section
		SectionBuilder lastSection = null;
		for (int width = 0; width < master.GetLength(0); width++)
		{
			for (int height = 0; height < master.GetLength(1); height++)
			{
				SBParams sbParams = new SBParams();
				EntrancePositions entrances;
				if (lastSection == null) 
				{
					int initialGroundHeight = UnityEngine.Random.Range(1,(int) (levelSize.y/sectionsY));
					entrances = new EntrancePositions(new EntrancePosition(initialGroundHeight,2),new EntrancePosition(),new EntrancePosition(),new EntrancePosition());
					int min = (int) (player.maxPlayerSize.y + initialGroundHeight + 1);
					int max = (int) (levelSize.y - 1);
					sbParams.ceilingHeight = UnityEngine.Random.Range(min, max);
	
				}
				else
				{
					entrances = new EntrancePositions(lastSection.finalEntrancePositions.eastEntrance, new EntrancePosition(), new EntrancePosition(), new EntrancePosition());
					sbParams.ceilingHeight = lastSection.finalCeilingHeight;
				}

				Vector2 scaleNewSection = new Vector2(xSize*sectionMultiplier[width], ySize);

				sbParams.size = scaleNewSection;
				sbParams.entrancePositions = entrances;

				sbParams.difficulty = difficulty;

				if (sectionAttributes.GetLength(0) > 0)
				{
					sbParams.sprites = sectionAttributes[UnityEngine.Random.Range(0, sectionAttributes.GetLength(0))];
				}
				else
				{
					sbParams.sprites = globalAttributes;
				}

				if (sbParams.sprites.hasCustomOpennessParameter)
				{
					sbParams.Caviness = 1.0f - sbParams.sprites.opennessParameter;
				}
				else
				{
					if (globalAttributes.hasCustomOpennessParameter)
					{
						sbParams.Caviness = 1.0f - globalAttributes.opennessParameter;
					}
					else
					{
						sbParams.Caviness = UnityEngine.Random.Range(0f, 1f);
					}
				}

				if (sbParams.sprites.hasCustomHillParameter)
				{
					sbParams.Hilliness = sbParams.sprites.hillParameter;
				}
				else
				{
					if (globalAttributes.hasCustomHillParameter)
					{
						sbParams.Hilliness = globalAttributes.hillParameter;
					}
					else
					{
						sbParams.Hilliness = UnityEngine.Random.Range(0f, 1f);
					}
				}

				if (sbParams.sprites.hasCustomPitParameter)
				{
					sbParams.Pittiness = sbParams.sprites.pitParameter;
				}
				else
				{
					if (globalAttributes.hasCustomPitParameter)
					{
						sbParams.Pittiness = globalAttributes.pitParameter;
					}
					else
					{
						sbParams.Pittiness = UnityEngine.Random.Range(0f, 1f);
					}
				}

				if (UnityEngine.Random.Range(0f, 1f) > .9f)
				{
					if (UnityEngine.Random.Range(0f, 1f) > .5f)
					{
						sbParams.OnlyGoesUp = true;
					}
					else
					{
						sbParams.OnlyGoesDown = true;
					}
				}

				if (width == master.GetLength(0) - 1 && height == master.GetLength(1) - 1)
				{
					sbParams.lastSection = true;
				}


				//TODO should be based on difficulty and the pittiness
				if (UnityEngine.Random.Range(0f, 1f) > .9f)
				{
					sbParams.allowPits = false;
				}

				SectionBuilder newSection = new SectionBuilder(this, sbParams);

				//Store each section in master
				master[width, height] = newSection.Build();
				lastSection = newSection;
			}
		}

		//generate the sections using the representative arrays
		float widthOffset = 0;
		for (int width = 0; width < master.GetLength(0); width++)
		{
			for (int height = 0; height < master.GetLength(1); height++)
			{
				Section section = master[width,height];
				SpriteRenderer baseBlock = section.Sprites.belowGroundBlocks[0];

				for (int i = 0; i < section.Grid.GetLength(0); i++)
				{
					for (int j = 0; j < section.Grid.GetLength(1); j++)
					{
						float centerX = baseBlock.sprite.bounds.extents.x  * 2 * i + widthOffset;
						float centerY = baseBlock.sprite.bounds.extents.y * 2 * j + (
							baseBlock.sprite.bounds.extents.y * 2 * section.Grid.GetLength(1) * height);

						AssetTypeKey key = (AssetTypeKey) section.Grid[i,j];
						UnityEngine.Object toInstantiate = GetBlockOfTypeForSection(key, section);

						if (toInstantiate != null)
						{
							Instantiate(toInstantiate, new Vector3(centerX,centerY,0), new Quaternion());
						}
					}
				}
				widthOffset += baseBlock.sprite.bounds.extents.x  * 2 * section.Grid.GetLength(0);
				//widthOffset += baseBlock.sprite.bounds.extents.y * 2 * section.Grid.GetLength(0)* width * sectionMultiplier[width];
			}
		}
	}

	private UnityEngine.Object GetBlockOfTypeForSection(AssetTypeKey type, Section s)
	{
		switch (type)
		{
		case AssetTypeKey.UndergroundBlock:
			return GetBlockFromArrays(globalAttributes.belowGroundBlocks, s.Sprites.belowGroundBlocks);
		case AssetTypeKey.TopGroundBlock:
			return GetBlockFromArrays(globalAttributes.topGroundBlocks, s.Sprites.topGroundBlocks);
		/*case AssetTypeKey.WallBlock:
			return GetBlockFromArrays(globalSprites.wallBlocks, s.Sprites.wallBlocks);*/
		case AssetTypeKey.CeilingBlock:
			return GetBlockFromArrays(globalAttributes.ceilingBlocks, s.Sprites.ceilingBlocks);
		case AssetTypeKey.LevelEnd:
			return levelEnd;
		default:
			return null;
		}
	}

	public SpriteRenderer GetBaseBlock()
	{
		if (globalAttributes.belowGroundBlocks.GetLength(0) > 0)
		{
			return globalAttributes.belowGroundBlocks[0];
		}
		else
		{
			return sectionAttributes[0].belowGroundBlocks[0];
		}
	}

	private UnityEngine.Object GetBlockFromArrays(params SpriteRenderer[][] arrays)
	{
		List<SpriteRenderer> allSprites = new List<SpriteRenderer>();
		int totalSize = 0;
		for (int i = 0; i < arrays.GetLength(0); i++)
		{
			for (int j = 0; j < ((SpriteRenderer[]) arrays[i]).GetLength(0); j++)
			{
				allSprites.Add(((SpriteRenderer[]) arrays[i])[j]);
			}
		}
		
		int randomIndex = UnityEngine.Random.Range(0, allSprites.Count);
		return allSprites.ElementAt(randomIndex);
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
		CeilingBlock = 5,
		TopGroundBlock = 6,
		LevelEnd = 7,
		Empty = 8
	}
}
