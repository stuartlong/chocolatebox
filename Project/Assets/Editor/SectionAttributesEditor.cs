using UnityEngine;
using System.Collections;
using UnityEditor;

/// <summary>
/// Custom Section Attributes Unity editor.
/// 
/// Primary Author - Stuart Long
/// </summary>
[CustomEditor(typeof(SectionAttributes))]
public class SectionAttributesEditor : Editor 
{

	private static readonly int VERTICAL_TAB = LevelGeneratorEditor.VERTICAL_TAB;
	private bool blockFold = false;
	public override void OnInspectorGUI()
	{
		
		serializedObject.Update();
		SectionAttributes attributes = (SectionAttributes) target;

		EditorGUILayout.BeginVertical();

		blockFold = EditorGUILayout.Foldout(blockFold, "Sprites");
		//generator.groundBlocks = (SpriteRenderer[]) EditorGUILayout.ObjectField("Ground Blocks", generator.groundBlocks, typeof(SpriteRenderer[]), true);
		if (blockFold) 
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Space(20);
			
			EditorGUILayout.BeginVertical();
		EditorGUILayout.LabelField("The sprites to use for you level. These should be prefabs.");
		SerializedProperty pitsObjects = serializedObject.FindProperty("pitObjects");
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(pitsObjects, true);
		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}
		
		SerializedProperty belowGroundBlocks = serializedObject.FindProperty("belowGroundBlocks");
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(belowGroundBlocks, true);
		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}

		SerializedProperty topGroundBlocks = serializedObject.FindProperty("topGroundBlocks");
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(topGroundBlocks, true);
		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}

		SerializedProperty ceilingBlocks = serializedObject.FindProperty("ceilingBlocks");
		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(ceilingBlocks, true);
		if (EditorGUI.EndChangeCheck())
		{
			serializedObject.ApplyModifiedProperties();
		}

			SerializedProperty decs = serializedObject.FindProperty("decorations");
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(decs, true);
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}

			SerializedProperty platforms = serializedObject.FindProperty("platformBlocks");
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(platforms, true);
			if (EditorGUI.EndChangeCheck())
			{
				serializedObject.ApplyModifiedProperties();
			}
		}
		else
		{
			EditorGUILayout.LabelField("The sprites to use for you level. These should be prefabs.");
		}

		GUILayout.Space(VERTICAL_TAB);

		attributes.hasCustomOpennessParameter = EditorGUILayout.ToggleLeft("Set Custom Openness", attributes.hasCustomOpennessParameter);
		if (attributes.hasCustomOpennessParameter)
			{
			attributes.opennessParameter = EditorGUILayout.Slider("Section Openness", attributes.opennessParameter, 0, 1);
				EditorGUILayout.LabelField("Determines approximately how far apart the ceiling and floor of this section should be.");
				GUILayout.Space(VERTICAL_TAB);
			}
			else
			{
				EditorGUILayout.LabelField("If not checked, this section will have a random parameter determining how \"open\" it is");
				GUILayout.Space(VERTICAL_TAB);
			}
		
		attributes.hasCustomHillParameter = EditorGUILayout.ToggleLeft("Set Custom Gradient Parameter", attributes.hasCustomHillParameter);
		if (attributes.hasCustomHillParameter)
		{
			attributes.hillParameter = EditorGUILayout.Slider("Section Gradient", attributes.hillParameter, 0, 1);
			EditorGUILayout.LabelField("Determines how often the height of the floor should go up or down.");
			GUILayout.Space(VERTICAL_TAB);
		}
		else
		{
			EditorGUILayout.LabelField("If not checked, this section will have a random parameter determining how often the ground gradient should change");
			GUILayout.Space(VERTICAL_TAB);
		}
		
		attributes.hasCustomPitParameter = EditorGUILayout.ToggleLeft("Set Custom Pit Frequency Parameter", attributes.hasCustomPitParameter);
		if (attributes.hasCustomPitParameter)
		{
			attributes.pitParameter = EditorGUILayout.Slider("Pit Frequency", attributes.pitParameter, 0, 1);
			EditorGUILayout.LabelField("Determines approximately how often pits should appear.");
			GUILayout.Space(VERTICAL_TAB);
		}
		else
		{
			EditorGUILayout.LabelField("If not checked, this section will have a random parameter determining how frequently pits should spawn");
			GUILayout.Space(VERTICAL_TAB);
		}

		attributes.hasCustomDecorativeParameter = EditorGUILayout.ToggleLeft("Set Decoration Frequency Paramter", attributes.hasCustomDecorativeParameter);
		if (attributes.hasCustomDecorativeParameter)
		{
			attributes.decorativeParameter = EditorGUILayout.Slider("Decoration Frequency", attributes.decorativeParameter, 0, 1);
			EditorGUILayout.LabelField("Determines approximately how often decorations should appear.");
			GUILayout.Space(VERTICAL_TAB);
		}
		else
		{
			EditorGUILayout.LabelField("If not checked, this section will have a random parameter determining how frequently decorations should spawn");
			GUILayout.Space(VERTICAL_TAB);
		}

		attributes.hasCustomPlatformParameter = EditorGUILayout.ToggleLeft("Set Platform Frequency Paramter", attributes.hasCustomPlatformParameter);
		if (attributes.hasCustomPlatformParameter)
		{
			attributes.platformParameter = EditorGUILayout.Slider("Platform Frequency", attributes.platformParameter, 0, 1);
			EditorGUILayout.LabelField("Determines approximately how often platforms should appear.");
			GUILayout.Space(VERTICAL_TAB);
		}
		else
		{
			EditorGUILayout.LabelField("If not checked, this section will have a random parameter determining how frequently decorations should spawn");
			GUILayout.Space(VERTICAL_TAB);
		}

		EditorGUILayout.EndVertical();
	}
}
