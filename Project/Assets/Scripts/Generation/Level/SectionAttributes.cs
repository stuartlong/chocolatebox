using UnityEngine;
using System.Collections;

/// <summary>
/// Specifies the various sprites to use in a given section. The sprites for all of the BLOCKS
/// MUST be the same size.
/// </summary>
public class SectionAttributes : MonoBehaviour
{
	/// <summary>
	/// The sprites that will be placed at the bottom of pits.
	/// </summary>
	public SpriteRenderer[] pitObjects;

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
	public SpriteRenderer[] ceilingBlocks;

	public bool hasCustomHillParameter = false;
	public float hillParameter;

	public bool hasCustomPitParameter = false;
	public float pitParameter;

	public bool hasCustomOpennessParameter = false;
	public float opennessParameter;
}
