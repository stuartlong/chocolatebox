using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
	public int sectionsY;
	public int sectionsX;
	public Vector2 sectionSize;
	public Vector2 baseBlockSize;
	public SpriteRenderer groundBlock;
    public bool openLevel;

    [HideInInspector]
    public int[,][,] master;


	public void Start () {
		//A grid of sections
		master = new int[sectionsX,sectionsY][,];

		SectionBuilder lastSection = null;
		for (int width = 0; width < master.GetLength(0); width++)
		{
			for (int height = 0; height < master.GetLength(1); height++)
			{
				EntrancePositions entrances;
				if (lastSection == null) 
				{
					entrances = new EntrancePositions(Random.Range(1,(int) (sectionSize.y/sectionsY)),-1,-1,-1);
				}
				else
				{
					entrances = new EntrancePositions(lastSection.finalEntrancePositions.eastEntrance, -1, -1, -1);
				}
				Vector2 scaleNewSection = new Vector2(sectionSize.x/sectionsX, sectionSize.y/sectionsY);
				SectionBuilder newSection = new SectionBuilder(scaleNewSection, this, entrances, .5f);
				int[,] section = newSection.Build();

				//Store each section in master
				master[width, height] = section;

				//generate the section
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
                            
							//Debug.Log (centerX + ", " + centerY);
						}
					}
				}

				lastSection = newSection;
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
