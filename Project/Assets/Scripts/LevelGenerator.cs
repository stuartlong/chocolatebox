﻿using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
	public Vector2 levelSize;
	public Vector2 baseBlockSize;
	public SpriteRenderer groundBlock;
	public const int NUMSECTIONS = 15; //Number of sections to divide map into
	private bool done = false;

	public void Start () {
		//A grid of sections, might not even need to store this
		SectionBuilder[,] master = new SectionBuilder[NUMSECTIONS,NUMSECTIONS];
		for (int width = 0; width < master.GetLength(0); width++)
		{
			for (int height = 0; height < master.GetLength(1); height++)
			{
				int[,] section = new SectionBuilder(levelSize/NUMSECTIONS, this).Build();
				master[width, height] = section;
				for (int i = 0; i < section.GetLength(0); i++)
				{
					for (int j = 0; j < section.GetLength(1); j++)
					{
						if (section[i,j] == (int) AssetTypeKey.GroundBlock)
						{
							float centerX = groundBlock.sprite.bounds.extents.x  * 2 * i + (
								groundBlock.sprite.bounds.extents.y * 2 * section.GetLength(0)* width);
							float centerY = groundBlock.sprite.bounds.extents.y * 2 * j + (
								groundBlock.sprite.bounds.extents.y * 2 * section.GetLength(1) * height);
							Instantiate(groundBlock, new Vector3(centerX,centerY,0), new Quaternion());
							Debug.Log (centerX + ", " + centerY);
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
		GroundBlock = 1,
		Entrance = 2,
		Exit = 3
	}
}
