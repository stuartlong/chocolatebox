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
	public LevelEndAttachment levelEnd;
	public Section[,] master;
	public bool openLevel;
	public SectionAttributes globalAttributes;
	public bool customSeed = false;
	public float difficulty;
	public float currentDifficulty;
	public float initialDifficulty;
	public float terminalDifficulty;
	public bool infiniteLevel;

	#region Level Build
	public void Awake () 
	{
		if (!customSeed)
		{
			DateTime epochStart = new System.DateTime(1970, 1, 1, 8, 0, 0, System.DateTimeKind.Utc);
			seed = (int) (System.DateTime.UtcNow - epochStart).TotalSeconds;
		}
		UnityEngine.Random.seed = seed;

		//player.repeatingLevel = infiniteLevel;

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
					if (openLevel)
					{
						sbParams.ceilingHeight = (int) levelSize.y;
					}
					else
					{
						int min = (int) (ConvertToBlocksY(player.maxPlayerSize.y) + initialGroundHeight + 1);
						int max = (int) (levelSize.y - 1);
						sbParams.ceilingHeight = UnityEngine.Random.Range(min, max);
					}
				}
				else
				{
					entrances = new EntrancePositions(lastSection.finalEntrancePositions.eastEntrance, new EntrancePosition(), new EntrancePosition(), new EntrancePosition());
					sbParams.ceilingHeight = lastSection.finalCeilingHeight;
				}

				Vector2 scaleNewSection = new Vector2(xSize*sectionMultiplier[width], ySize);

				sbParams.size = scaleNewSection;
				sbParams.entrancePositions = entrances;

				//make difficulty linearly scale from minimul to maximum
				currentDifficulty = initialDifficulty + (terminalDifficulty/(master.GetLength(0)-1)) * (width+1);

				sbParams.difficulty = currentDifficulty;

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

				if (sbParams.sprites.hasCustomPlatformParameter)
				{
					sbParams.Platforminess = sbParams.sprites.platformParameter;
				}
				else
				{
					if (globalAttributes.hasCustomPlatformParameter)
					{
						sbParams.Platforminess = globalAttributes.platformParameter;
					}
					else
					{
						sbParams.Platforminess = UnityEngine.Random.Range(0f, 1f);
					}
				}

				if (UnityEngine.Random.Range(0f, 1f) > .75f)
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
				SpriteRenderer baseBlock = GetBaseBlock();
				bool finalSection = width == (master.GetLength(0) - 1);

				for (int i = 0; i < section.Grid.GetLength(0); i++)
				{
					for (int j = 0; j < section.Grid.GetLength(1); j++)
					{
						float centerX = ConvertToUnityUnitsX(i) + widthOffset;
						float centerY = ConvertToUnityUnitsY(j) + (ConvertToUnityUnitsY(height)*section.Grid.GetLength(1));

						AssetTypeKey key = (AssetTypeKey) section.Grid[i,j];
						UnityEngine.Object toInstantiate = GetBlockOfTypeForSection(key, section);

						if (finalSection && i == section.Grid.GetLength(0) - 1 && j == section.GroundHeights[i]) 
						{
							float levelEndY = centerY + GetBaseBlock().bounds.extents.y + levelEnd.maxSize.y / 2f;
							Instantiate(levelEnd, new Vector3(centerX, levelEndY, 0), new Quaternion());
						}

						if (toInstantiate != null)
						{
							Instantiate(toInstantiate, new Vector3(centerX,centerY,0), new Quaternion());
						}
					}
				}

                //Generate the enemies in these sections.
                section.GenerateEnemyRangeTree();
                foreach (EnemySection es in section.EnemySections)
                {
                    int nextX = es.leftBound;

                    while (nextX < es.rightBound && es.rightBound > 5)
                    {
                        EnemyAttachment nextEnemy = GetNextEnemy(section);

                        float centerX = (baseBlock.sprite.bounds.extents.x + nextEnemy.requiredSpace.x) * 2 * nextX + widthOffset +
                                            nextEnemy.gameObject.renderer.bounds.extents.x*2;

                        float yPos = (float)UnityEngine.Random.Range(es.lowerBound, es.upperBound);

    					float centerY = (1+yPos) * baseBlock.sprite.bounds.extents.y * 2 * baseBlock.transform.localScale.y;
                        //centerY *= section.Grid.GetLength(1) * height; //This line multiplies by 0
                        centerY += nextEnemy.renderer.bounds.extents.y * 4 * nextEnemy.transform.localScale.y;

                        if (centerX >= 3)
                        {
                            Instantiate(nextEnemy.gameObject, new Vector3(centerX, centerY, 0), new Quaternion());
                        }

                        nextX += (int)nextEnemy.requiredSpace.x;
                    }
                }

                widthOffset += baseBlock.sprite.bounds.extents.x * 2 * section.Grid.GetLength(0);

			}
		}

		Decorate();
		player.OnLevelLoad();
	}
	#endregion

	#region Decorating
	private void Decorate()
	{
		SpriteRenderer baseBlock = GetBaseBlock();
		float widthOffset = 0;
		for (int width = 0; width < master.GetLength(0); width++)
		{
			for (int height = 0; height < master.GetLength(1); height++)
			{
				Section section = master[width,height];
				float avgNumbDecs = section.Attributes.decorativeParameter * section.getWidth() / (2*(Enum.GetValues(typeof(DecorationAttachment.DecorationType)).Length - (float) section.Attributes.GetNumberOfTypersOfDecorations() + 1));
				int numbDecorations = (int) GenerateNormalVar(avgNumbDecs, avgNumbDecs / 6);

				for (int d = 0; d < numbDecorations; d++)
				{
					DecorationAttachment decoration = section.Attributes.GetRandomDecoration();
					if (openLevel && decoration.type.Equals(DecorationAttachment.DecorationType.Hanging)) {
						continue;
					}

					PlaceDecoration(section.Attributes.GetRandomDecoration(), section, widthOffset);
				}

				widthOffset += ConvertToUnityUnitsX(section.Grid.GetLength(0));
			}
		}
	}

	private void PlaceGroundDecoration(DecorationAttachment dec, Section section, float widthOffset)
	{
		List<int> placesToPlace = new List<int>();
		Vector2 maxSize = new Vector2(ConvertToBlocksX(dec.maxSize.x), ConvertToBlocksY(dec.maxSize.y));
		int nextX = 0;
		for (int column = 0; column < section.GroundHeights.GetLength(0); column++)
		{
			for (int x = nextX; x < column + maxSize.x; x++)
			{
				if (x + (int) maxSize.x < section.getWidth()
				    && section.CeilingHeights[x] - section.GroundHeights[x] > maxSize.y
				    && (x == section.GroundHeights.Length || section.GroundHeights[x] == section.GroundHeights[x + 1])
			   		&& !section.PitIndeces.Contains(x))
				{
					if (x == column + maxSize.x - 1)
					{
						if (!CollidesWithDecorationOrPlatform(maxSize, section, new Vector2(column, section.GroundHeights[column]), dec.allowOverlap))
						{
							placesToPlace.Add(column);
						}
					}
				}
				else
				{
					column = x; 
					nextX = x + 1;
					break;
				}
			}
		}
		
		if (placesToPlace.Count < 1)
		{
			return;
		}
		int index = UnityEngine.Random.Range(0, placesToPlace.Count - 1);
		int toPlace = placesToPlace[index];


		for (int x = toPlace; x < toPlace + maxSize.x; x++)
		{
			for (int y = section.GroundHeights[x]; y < section.GroundHeights[x] + maxSize.y; y++)
			{
				section.DecorationGrid[x,y] = 1;
			}
		}

			float centerX = ConvertToUnityUnitsX(toPlace) + widthOffset + (dec.maxSize.x / 2) - GetBaseBlock().sprite.bounds.extents.x;
			float centerY = ConvertToUnityUnitsY(section.GroundHeights[toPlace]) + GetBaseBlock().sprite.bounds.extents.y + (dec.maxSize.y / 2);
			Instantiate(dec, new Vector3(centerX, centerY,1), new Quaternion());
	}
	
	private void PlaceDecoration(DecorationAttachment dec, Section section, float widthOffset)
	{
		switch(dec.type)
		{
		case DecorationAttachment.DecorationType.Hanging:
			PlaceHangingDecoration(dec, section, widthOffset);
			break;
		case DecorationAttachment.DecorationType.OnGround:
			PlaceGroundDecoration(dec, section, widthOffset);
			break;
		default:
			Debug.Log("ERROR: DecorationType not recognized" + dec.type);
			break;
		}
	}

	private bool CollidesWithDecorationOrPlatform(Vector2 maxSize, Section section, Vector2 pointToPlace, bool allowOverlap)
	{
		for (int x = (int) pointToPlace.x; x < (int) (pointToPlace.x + maxSize.x); x++)
		{
			for (int y = (int)pointToPlace.y; y < (int) (pointToPlace.y + maxSize.y); y++)
			{
				if (!allowOverlap && section.DecorationGrid[x,y] == 1)
				{
					return true;
				}

				if (section.Grid[x,y] == (int) AssetTypeKey.Platform)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void PlaceHangingDecoration(DecorationAttachment dec, Section section, float widthOffset)
	{
		List<int> placesToPlace = new List<int>();
		Vector2 maxSize = new Vector2(ConvertToBlocksX(dec.maxSize.x), ConvertToBlocksY(dec.maxSize.y));
		int nextX = 0;
		for (int column = 0; column < section.GroundHeights.GetLength(0); column++)
		{
			for (int x = nextX; x < column + maxSize.x; x++)
			{
				if (x + (int) maxSize.x < section.getWidth()
				    && section.CeilingHeights[x] - section.GroundHeights[x] > maxSize.y
				    && (column == section.CeilingHeights.Length || section.CeilingHeights[column] == section.CeilingHeights[column + 1]))
				{
					if (x == column + maxSize.x - 1)
					{
						if (!CollidesWithDecorationOrPlatform(maxSize, section, new Vector2(column, section.CeilingHeights[column] - maxSize.y), dec.allowOverlap))
						{
							placesToPlace.Add(column);
						}
					}
				}
				else
				{
					column = x; 
					nextX = x + 1;
					break;
				}
			}
		}
		
		if (placesToPlace.Count < 1)
		{
			return;
		}
		int index = UnityEngine.Random.Range(0, placesToPlace.Count - 1);
		int toPlace = placesToPlace[index];
		
		
		for (int x = toPlace; x < toPlace + maxSize.x; x++)
		{
			for (int y = section.CeilingHeights[x] - (int) maxSize.y; y < section.CeilingHeights[x]; y++)
			{
				section.DecorationGrid[x,y] = 1;
			}
		}
		
		float centerX = ConvertToUnityUnitsX(toPlace) + widthOffset + (dec.maxSize.x / 2) - GetBaseBlock().sprite.bounds.extents.x;
		float centerY = ConvertToUnityUnitsY(section.CeilingHeights[toPlace]) - GetBaseBlock().sprite.bounds.extents.y - (dec.maxSize.y / 2);
		Instantiate(dec, new Vector3(centerX, centerY,1), new Quaternion());
	}
	#endregion

	private UnityEngine.Object GetBlockOfTypeForSection(AssetTypeKey type, Section s)
	{
		switch (type)
		{
		case AssetTypeKey.UndergroundBlock:
			return GetBlockFromArrays(globalAttributes.belowGroundBlocks, s.Attributes.belowGroundBlocks);
		case AssetTypeKey.TopGroundBlock:
			return GetBlockFromArrays(globalAttributes.topGroundBlocks, s.Attributes.topGroundBlocks);
		case AssetTypeKey.CeilingBlock:
			return GetBlockFromArrays(globalAttributes.ceilingBlocks, s.Attributes.ceilingBlocks);
		case AssetTypeKey.Platform:
			return GetBlockFromArrays(globalAttributes.platformBlocks, s.Attributes.platformBlocks);
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
    /// Select the next enemy based on the size of the available Enemy Section and
    /// Enemy occurance probabilities
    /// </summary>
    /// <returns></returns>
    private EnemyAttachment GetNextEnemy(Section section)
    {
        ///Better algorithm incoming. 
        return section.enemyTree.Get(section.enemyTree.RandomIndex());
    }

	public float ConvertToUnityUnitsY(int blocks)
	{
		return GetBaseBlock().sprite.bounds.extents.y * 2 * blocks;
	}

	public float ConvertToUnityUnitsX(int blocks)
	{
		return GetBaseBlock().sprite.bounds.extents.x * 2 * blocks;
	}


	public int ConvertToBlocksY(float unityUnitsY)
	{
		return Mathf.CeilToInt((unityUnitsY / (GetBaseBlock().sprite.bounds.extents.y * 2)));
	}
	
	public int ConvertToBlocksX(float unityUnitsX)
	{
		return Mathf.CeilToInt((unityUnitsX / (GetBaseBlock().sprite.bounds.extents.x * 2)));
	}

	public static float GenerateNormalVar(float mean, float stdDev)
	{
		float uOne = UnityEngine.Random.Range(0f, 1f);
		float uTwo = UnityEngine.Random.Range(0f, 1f);
		float normalVar = Mathf.Sqrt(-2f * Mathf.Log(uOne)) * Mathf.Sin(2f*Mathf.PI*uTwo);
		return mean + stdDev * normalVar;
	}

	public static float GenerateExponentialVar(float mu)
	{
		return -mu * Mathf.Log(1 - UnityEngine.Random.Range(0f, 1f));
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
		Empty = 8,
		Platform = 9
	}
}
