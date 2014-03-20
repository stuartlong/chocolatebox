using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Custom Level generator Unity editor.
/// 
/// Primary Author - Stuart Long
/// </summary>
[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		LevelGenerator generator = (LevelGenerator) target;

		Vector2 floatLevelSize = EditorGUILayout.Vector2Field("Level Size", generator.levelSize);
		generator.levelSize.x = (int) floatLevelSize.x;
		generator.levelSize.y = (int) floatLevelSize.y;

		Vector2 numbSections = EditorGUILayout.Vector2Field("Number of Sections", generator.levelSize);
		generator.sectionsX = (int) numbSections.x;
		generator.sectionsY = (int) numbSections.y;

		generator.groundBlock = (SpriteRenderer) EditorGUILayout.ObjectField("Ground Block", generator.groundBlock, typeof(SpriteRenderer), true);
		generator.player = (PlayerAttachment) EditorGUILayout.ObjectField("Player", generator.player, typeof(PlayerAttachment), true);
		generator.MergeChance = EditorGUILayout.IntSlider("Section Merge Chance", generator.MergeChance, 0, 100);

		generator.customSeed = EditorGUILayout.BeginToggleGroup("Set Custom Seed", generator.customSeed);
		generator.seed = EditorGUILayout.IntField("Seed", generator.seed);
		EditorGUILayout.EndToggleGroup();
	}
}
