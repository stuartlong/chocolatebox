﻿using UnityEngine;
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
	private static readonly int VERTICAL_TAB = 10;
	private bool blockFold = false;
	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		LevelGenerator generator = (LevelGenerator) target;

		EditorGUILayout.BeginVertical();
		GUILayout.Space(VERTICAL_TAB);
		Vector2 floatLevelSize = EditorGUILayout.Vector2Field("Level Size", generator.levelSize);
		generator.levelSize.x = (int) floatLevelSize.x;
		generator.levelSize.y = (int) floatLevelSize.y;
		EditorGUILayout.LabelField("The total size of the level to be generated.");

		GUILayout.Space(VERTICAL_TAB);
		generator.openLevel = EditorGUILayout.Toggle("Open Level", generator.openLevel);
		EditorGUILayout.LabelField("Open levels will have no walls or ceilings.");
		GUILayout.Space(VERTICAL_TAB);

		if (!generator.openLevel) 
		{
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
		EditorGUILayout.LabelField("The number of sections that will comprise your level.");
		GUILayout.Space(VERTICAL_TAB);

		blockFold = EditorGUILayout.Foldout(blockFold, "Sprites");
		//generator.groundBlocks = (SpriteRenderer[]) EditorGUILayout.ObjectField("Ground Blocks", generator.groundBlocks, typeof(SpriteRenderer[]), true);
		if (blockFold) 
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(20);

			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("The sprites to use for you level. These should be prefabs.");

			SerializedProperty globablSprites = serializedObject.FindProperty("globalSprites");
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(globablSprites, true);
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}

			SerializedProperty blocks = serializedObject.FindProperty("sectionGroups");
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(blocks, true);
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndHorizontal();
		}
		else
		{
			EditorGUILayout.LabelField("The sprites to use for you level. These should be prefabs.");
		}
		GUILayout.Space(VERTICAL_TAB);

		generator.player = (PlayerAttachment) EditorGUILayout.ObjectField("Player", generator.player, typeof(PlayerAttachment), true);
		EditorGUILayout.LabelField("The player object");
		GUILayout.Space(VERTICAL_TAB);

		generator.levelEnd = (SpriteRenderer) EditorGUILayout.ObjectField("Level End", generator.levelEnd, typeof(SpriteRenderer), true);
		EditorGUILayout.LabelField("The object to appear at the end of the level.");
		GUILayout.Space(VERTICAL_TAB);

		generator.MergeChance = EditorGUILayout.IntSlider("Section Merge Chance", generator.MergeChance, 0, 100);
		EditorGUILayout.LabelField("The chance that one or more of your sections will merge to form a single large section.");
		GUILayout.Space(VERTICAL_TAB);

		generator.difficulty = EditorGUILayout.Slider("Difficulty", generator.difficulty, 0, 1);
		EditorGUILayout.LabelField("The general difficulty of the level to be generated.");
		GUILayout.Space(VERTICAL_TAB);

		generator.openness = EditorGUILayout.Slider("Section Openness", generator.openness, 0, 1);
		EditorGUILayout.LabelField("Determines approximately how far apart the ceiling and floor of sections should be.");
		GUILayout.Space(VERTICAL_TAB);

		generator.hilliness = EditorGUILayout.Slider("Section Gradient", generator.hilliness, 0, 1);
		EditorGUILayout.LabelField("Determines how often the height of the floor should go up or down.");
		GUILayout.Space(VERTICAL_TAB);

		generator.pittiness = EditorGUILayout.Slider("Pit Frequency", generator.pittiness, 0, 1);
		EditorGUILayout.LabelField("Determines approximately how often pits should appear.");
		GUILayout.Space(VERTICAL_TAB);

		generator.customSeed = EditorGUILayout.BeginToggleGroup("Set Custom Seed", generator.customSeed);
		generator.seed = EditorGUILayout.IntField("Seed", generator.seed);
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.EndVertical();
	}
}
