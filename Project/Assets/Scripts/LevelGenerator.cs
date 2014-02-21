using UnityEngine;
using System.Collections;

public class LevelGenerator : MonoBehaviour {
	public Vector2 levelSize;
	public Vector2 baseBlockSize;
	public SpriteRenderer groundBlock;
	private bool done = false;

	public void Start () {
		int[,] section = new SectionBuilder(levelSize, this).Build();
		for (int i = 0; i < section.GetLength(0); i++)
		{
			for (int j = 0; j < section.GetLength(1); j++)
			{
				if (section[i,j] == (int) AssetTypeKey.GroundBlock)
				{
					float centerX = groundBlock.sprite.bounds.extents.x  * 2 * i;
					float centerY = groundBlock.sprite.bounds.extents.y * 2 * j;
					Instantiate(groundBlock, new Vector3(centerX,centerY,0), new Quaternion());
					Debug.Log (centerX + ", " + centerY);
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
