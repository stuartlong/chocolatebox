using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

	public DecorationAttachment[] decorations;

	public bool hasCustomHillParameter = false;
	public float hillParameter;

	public bool hasCustomPitParameter = false;
	public float pitParameter;

	public bool hasCustomOpennessParameter = false;
	public float opennessParameter;

	public bool hasCustomDecorativeParameter = false;
	public float decorativeParameter;

	private bool decorationDistributionPopulated = false;

	public DecorationAttachment GetRandomDecoration()
	{
		if (!decorationDistributionPopulated)
		{
			BuildDecorationDistribution();
		}

		float rand = Random.Range(0f,1f);
		for (int i = 0; i < probs.Count - 1; i++)
		{
			if (probs.ElementAt(i) <= rand && (i == probs.Count - 1 || rand < probs.ElementAt(i + 1)))
			{
				return decMap[(int) (probs.ElementAt(i + 1)*100)];
			}
		}

		Debug.Log("ERROR: Couldn't find a random decoration!");
		return null;
	}

	private List<float> probs;
	private Dictionary<int, DecorationAttachment> decMap;
	private void BuildDecorationDistribution()
	{
		float normalize = 0f;
		probs = new List<float>();
		decMap = new Dictionary<int, DecorationAttachment>();
		foreach (DecorationAttachment dec in decorations)
		{
			normalize += dec.frequency;
		}
		
		float sum = 0f;
		probs.Add(0f);
		foreach (DecorationAttachment dec in decorations)
		{
			float cdfVal = (dec.frequency / normalize) + sum;
			probs.Add(cdfVal);
			decMap.Add((int) (cdfVal*100), dec);
		}
		probs.Add(1.0f);
		
		probs.Sort();
		decorationDistributionPopulated = true;
	}
}
