using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Specifies the various sprites to use in a given section. The sprites for all of the BLOCKS
/// MUST be the same size.
/// 
/// Primary author - Stuart Long
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

    /// <summary>
    /// The enemies to be generated via the generator. 
    /// Enemies must utilize the Enemy Attachment Script
    /// </summary>
    public EnemyAttachment[] enemies;

	/// <summary>
	/// The sprites representing game decorations.
	/// </summary>
	public DecorationAttachment[] decorations;

	/// <summary>
	/// The sprites represents platform blocks.
	/// These should be the same size as the ground blocks.
	/// </summary>
	public SpriteRenderer[] platformBlocks;

	/// <summary>
	/// Specifices whether or not this section has a custom
	/// hill parameter. If false, the parameter
	/// will be selected pseudorandomly.
	/// </summary>
	public bool hasCustomHillParameter = false;
	/// <summary>
	/// The parameter determining how frequently the
	/// section gradient should change.
	/// </summary>
	public float hillParameter;

	/// <summary>
	/// Specifices whether or not this section has a custom
	/// pit frequency parameter. If false, the parameter
	/// will be selected pseudorandomly.
	/// </summary>
	public bool hasCustomPitParameter = false;
	/// <summary>
	/// The parameter determining how frequently
	/// pits should appear
	/// </summary>
	public float pitParameter;

	/// <summary>
	/// Specifices whether or not this section has a custom
	/// openess parameter. If false, the parameter
	/// will be selected pseudorandomly.
	/// </summary>
	public bool hasCustomOpennessParameter = false;
	/// <summary>
	/// The parameter determining approximately how
	/// far apart the ground and ceiling are
	/// </summary>
	public float opennessParameter;

	/// <summary>
	/// Specifices whether or not this section has a custom
	/// decoration frequency parameter. If false, the parameter
	/// will be selected pseudorandomly.
	/// </summary>
	public bool hasCustomDecorativeParameter = false;
	/// <summary>
	/// The parameter determining how frequently
	/// decorations should appear.
	/// </summary>
	public float decorativeParameter;

	/// <summary>
	/// Specifices whether or not this section has a custom
	/// platform parameter. If false, the parameter
	/// will be selected pseudorandomly.
	/// </summary>
	public bool hasCustomPlatformParameter = false;
	/// <summary>
	/// The parameter determining how frequently
	/// platforms should appear.
	/// </summary>
	public float platformParameter;

	private bool decorationDistributionPopulated = false;

	/// <summary>
	/// Gets a random decoration for this section.
	/// </summary>
	/// <returns>A random decoration.</returns>
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

	/// <summary>
	/// Gets the number of typers of decorations in this section.
	/// </summary>
	/// <returns>The number of typers of decorations.</returns>
	public int GetNumberOfTypersOfDecorations()
	{
		List<DecorationAttachment.DecorationType> types = new List<DecorationAttachment.DecorationType>();
		foreach (DecorationAttachment dec in decorations)
		{
			if (!types.Contains(dec.type))
			{
				types.Add(dec.type);
			}
		}

		return types.Count;
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
			sum += cdfVal;
		}
		probs.Add(1.0f);
		
		probs.Sort();
		decorationDistributionPopulated = true;
	}
}
