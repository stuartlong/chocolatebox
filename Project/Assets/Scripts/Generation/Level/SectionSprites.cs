using UnityEngine;
using System.Collections;

/// <summary>
/// Specifies the various sprites to use in a given section. The sprites for all of the blocks
/// MUST be the same size.
/// </summary>
[System.Serializable]
public class SectionSprites
{
	public string name;

	/// <summary>
	/// The sprites to use for ground blocks. These act as the default blocks that make up
	/// your level. All block sprites must be the same size.
	/// </summary>
	public SpriteRenderer[] belowGroundBlocks;

	/// <summary>
	/// The sprites for the blocks that the player actually walks on. If empty, ceiling blocks will default to ground blocks.
	/// All block sprites must be the same size.
	/// </summary>
	public SpriteRenderer[] topGroundBlocks;

	/// <summary>
	/// The sprites to use for ceiling blocks. If empty, ceiling blocks will default to ground blocks.
	/// All block sprites must be the same size.
	/// </summary>
	public SpriteRenderer[] wallBlocks;

	/// <summary>
	/// The sprites to use for ceiling blocks. If empty, ceiling blocks will default to ground blocks.
	/// All block sprites must be the same size.
	/// </summary>
	public SpriteRenderer[] ceilingBlocks;
}
