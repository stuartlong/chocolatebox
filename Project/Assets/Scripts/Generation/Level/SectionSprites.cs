using UnityEngine;
using System.Collections;

/// <summary>
/// Essentially a wrapper class for a 2D array to allow users
/// to enter in groups of textures for sections.
/// </summary>
[System.Serializable]
public class SectionSprites
{
	public string name;
	public SpriteRenderer[] goundBlocks;
	public SpriteRenderer[] wallBlocks;
}
