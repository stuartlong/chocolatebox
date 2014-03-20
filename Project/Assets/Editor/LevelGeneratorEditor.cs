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

		generator.openLevel = EditorGUILayout.Toggle("Open Level (No ceiling)", generator.openLevel);

		if (!generator.openLevel) {
		Vector2 numbSections = EditorGUILayout.Vector2Field("Number of Sections", new Vector2(generator.sectionsX, generator.sectionsY));
		generator.sectionsX = (int) numbSections.x;
		generator.sectionsY = (int) numbSections.y;
		}
		else
		{
			Vector2 numbSections = EditorGUILayout.Vector2Field("Number of Sections", new Vector2(generator.sectionsX, 1));
			generator.sectionsX = (int) numbSections.x;
			generator.sectionsY = 1;
		}

		generator.groundBlock = (SpriteRenderer) EditorGUILayout.ObjectField("Ground Block", generator.groundBlock, typeof(SpriteRenderer), true);
		generator.player = (PlayerAttachment) EditorGUILayout.ObjectField("Player", generator.player, typeof(PlayerAttachment), true);
		generator.MergeChance = EditorGUILayout.IntSlider("Section Merge Chance", generator.MergeChance, 0, 100);

		generator.customSeed = EditorGUILayout.BeginToggleGroup("Set Custom Seed", generator.customSeed);
		generator.seed = EditorGUILayout.IntField("Seed", generator.seed);
		EditorGUILayout.EndToggleGroup();
	}
}
