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
	public static readonly int VERTICAL_TAB = 10;
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

		generator.infiniteLevel = EditorGUILayout.Toggle("Loop Level", generator.infiniteLevel);
		EditorGUILayout.LabelField("If toggled, ending the level by reaching the Level End object will generate a new level with these current parameters");
		GUILayout.Space(VERTICAL_TAB);

		generator.sectionsX = EditorGUILayout.IntField("Number of Sections", generator.sectionsX);
		generator.sectionsY = 1;
		EditorGUILayout.LabelField("The number of sections that will comprise your level.");
		GUILayout.Space(VERTICAL_TAB);

		blockFold = EditorGUILayout.Foldout(blockFold, "Section Attributes");
		//generator.groundBlocks = (SpriteRenderer[]) EditorGUILayout.ObjectField("Ground Blocks", generator.groundBlocks, typeof(SpriteRenderer[]), true);
		if (blockFold) 
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(20);

			EditorGUILayout.BeginVertical();
			EditorGUILayout.LabelField("The attributes that will characterize the sections of your level.");

			generator.globalAttributes = (SectionAttributes) EditorGUILayout.ObjectField("Default Attributes", generator.globalAttributes, typeof(SectionAttributes), true);

			SerializedProperty blocks = serializedObject.FindProperty("sectionAttributes");
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
			EditorGUILayout.LabelField("The attributes that will characterize the sections of your level.");
		}
		GUILayout.Space(VERTICAL_TAB);

		generator.player = (PlayerAttachment) EditorGUILayout.ObjectField("Player", generator.player, typeof(PlayerAttachment), true);
		EditorGUILayout.LabelField("The player object");
		GUILayout.Space(VERTICAL_TAB);

		generator.levelEnd = (LevelEndAttachment) EditorGUILayout.ObjectField("Level End", generator.levelEnd, typeof(LevelEndAttachment), true);
		EditorGUILayout.LabelField("The object to appear at the end of the level.");
		GUILayout.Space(VERTICAL_TAB);

		generator.MergeChance = EditorGUILayout.IntSlider("Section Merge Chance", generator.MergeChance, 0, 100);
		EditorGUILayout.LabelField("The chance that one or more of your sections will merge to form a single large section.");
		GUILayout.Space(VERTICAL_TAB);

		generator.initialDifficulty = EditorGUILayout.Slider("Initial Difficulty", generator.initialDifficulty, 0, 1);
		EditorGUILayout.LabelField("The general difficulty at the beginning of the level.");
		GUILayout.Space(VERTICAL_TAB);

		generator.terminalDifficulty = EditorGUILayout.Slider("Final Difficulty", generator.terminalDifficulty, 0, 1);
		EditorGUILayout.LabelField("The initial difficulty linearly ramps into this value by the final section.");
		GUILayout.Space(VERTICAL_TAB);

		generator.customSeed = EditorGUILayout.BeginToggleGroup("Set Custom Seed", generator.customSeed);
		generator.seed = EditorGUILayout.IntField("Seed", generator.seed);
		EditorGUILayout.EndToggleGroup();
		EditorGUILayout.EndVertical();

		generator.allowEnemies = EditorGUILayout.Toggle("Allow Enemies", generator.allowEnemies);
	}
}
