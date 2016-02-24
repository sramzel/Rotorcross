/*
---------------------- Unity Terrain Toolkit ----------------------
--
-- Unity Summer of Code 2009
-- Terrain Toolkit for Unity (Version 1.0.2)
-- All code by S�ndor Mold�n.
--
-- TerrainToolkitEditor.cs
--
-------------------------------------------------------------------
*/

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

// -------------------------------------------------------------------------------------------------------- EDITOR

[CustomEditor(typeof(TerrainToolkit))]
public class TerrainToolkitEditor : Editor {
	
	private bool showAdvancedSettings;
	private bool showInterfaceSettings;
	private string dragControl = "";
	private bool assignTexture = false;
	int i;
	int n;
	
	public override void OnInspectorGUI() {
		EditorGUIUtility.LookLikeControls();
		TerrainToolkit terrain = (TerrainToolkit) target as TerrainToolkit;
		if (!terrain.gameObject) {
			return;
		}
		Terrain terComponent = (Terrain) terrain.GetComponent(typeof(Terrain));
		if (!terrain.guiSkin) {
			terrain.guiSkin = (GUISkin) AssetDatabase.LoadAssetAtPath("Assets/TerrainToolkit/Editor/Resources/TerrainErosionEditorSkin.guiskin", typeof(GUISkin));
			terrain.createIcon = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/TerrainToolkit/Editor/Resources/createIcon.png", typeof(Texture2D));
			terrain.erodeIcon = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/TerrainToolkit/Editor/Resources/erodeIcon.png", typeof(Texture2D));
			terrain.textureIcon = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/TerrainToolkit/Editor/Resources/textureIcon.png", typeof(Texture2D));
			terrain.mooreIcon = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/TerrainToolkit/Editor/Resources/mooreIcon.png", typeof(Texture2D));
			terrain.vonNeumannIcon = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/TerrainToolkit/Editor/Resources/vonNeumannIcon.png", typeof(Texture2D));
			terrain.mountainsIcon = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/TerrainToolkit/Editor/Resources/mountainsIcon.png", typeof(Texture2D));
			terrain.hillsIcon = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/TerrainToolkit/Editor/Resources/hillsIcon.png", typeof(Texture2D));
			terrain.plateausIcon = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/TerrainToolkit/Editor/Resources/plateausIcon.png", typeof(Texture2D));
			terrain.defaultTexture = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/TerrainToolkit/Textures/default.jpg", typeof(Texture2D));
		}
		if (!terrain.presetsInitialised) {
			terrain.addPresets();
		}
		if (terComponent == null) {
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			GUI.skin = terrain.guiSkin;
			GUILayout.Label("The GameObject that Terrain Toolkit is attached to", "errorText");
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("does not have a Terrain component.", "errorText");
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Please attach a Terrain component.", "errorText");
			GUI.skin = null;
			EditorGUIUtility.LookLikeControls();
			EditorGUILayout.EndHorizontal();
			return;
		}
		if (terrain.heightBlendPoints == null) {
			terrain.heightBlendPoints = new List<float>();
		}
		Rect buttonRect;
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		GUIContent[] toolbarOptions = new GUIContent[3];
		toolbarOptions[0] = new GUIContent("Create", terrain.createIcon);
		toolbarOptions[1] = new GUIContent("Erode", terrain.erodeIcon);
		toolbarOptions[2] = new GUIContent("Texture", terrain.textureIcon);
		terrain.toolModeInt = GUILayout.Toolbar(terrain.toolModeInt, toolbarOptions);
		EditorGUILayout.EndHorizontal();
		switch (terrain.toolModeInt) {
			// -------------------------------------------------------------------------------------------------------- GENERATOR TOOLS
			case 0:
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			string[] generatorOptions = new string[5];
			generatorOptions[0] = "Voronoi";
			generatorOptions[1] = "Fractal";
			generatorOptions[2] = "Perlin";
			generatorOptions[3] = "Smooth";
			generatorOptions[4] = "Normalise";
			terrain.generatorTypeInt = GUILayout.Toolbar(terrain.generatorTypeInt, generatorOptions);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			switch (terrain.generatorTypeInt) {
				case 0:
				// Voronoi...
				EditorGUILayout.BeginHorizontal();
				if (GUI.changed) {
					EditorUtility.SetDirty(terrain);
				}
				GUI.changed = false;
				EditorGUILayout.PrefixLabel("Preset");
				string[] voronoiPresetNames = new string[terrain.voronoiPresets.Count + 1];
				int[] voronoiPresetInts = new int[terrain.voronoiPresets.Count + 1];
				voronoiPresetNames[0] = "None";
				TerrainToolkit.voronoiPresetData voronoiPreset;
				for (i = 1; i <= terrain.voronoiPresets.Count; i++) {
					voronoiPreset = (TerrainToolkit.voronoiPresetData) terrain.voronoiPresets[i - 1];
					voronoiPresetNames[i] = voronoiPreset.presetName;
					voronoiPresetInts[i] = i;
				}
				terrain.voronoiPresetId = EditorGUILayout.IntPopup(terrain.voronoiPresetId, voronoiPresetNames, voronoiPresetInts);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				if (GUI.changed && terrain.voronoiPresetId > 0) {
					voronoiPreset = (TerrainToolkit.voronoiPresetData) terrain.voronoiPresets[terrain.voronoiPresetId - 1];
					terrain.setVoronoiPreset(voronoiPreset);
				}
				if (GUI.changed) {
					EditorUtility.SetDirty(terrain);
				}
				GUI.changed = false;
				Rect featureRect = EditorGUILayout.BeginHorizontal();
				featureRect.x = 110;
				featureRect.width = 120;
				featureRect.height = 20;
				EditorGUILayout.PrefixLabel("Feature type");
				GUIContent[] featureStates = new GUIContent[3];
				featureStates[0] = new GUIContent(terrain.mountainsIcon);
				featureStates[1] = new GUIContent(terrain.hillsIcon);
				featureStates[2] = new GUIContent(terrain.plateausIcon);
				terrain.voronoiTypeInt = GUI.Toolbar(featureRect, terrain.voronoiTypeInt, featureStates);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Cells");
				terrain.voronoiCells = (int) EditorGUILayout.Slider(terrain.voronoiCells, 2, 100);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Features");
				terrain.voronoiFeatures = EditorGUILayout.Slider(terrain.voronoiFeatures, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Scale");
				terrain.voronoiScale = EditorGUILayout.Slider(terrain.voronoiScale, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Blend");
				terrain.voronoiBlend = EditorGUILayout.Slider(terrain.voronoiBlend, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				if (GUI.changed) {
					terrain.voronoiPresetId = 0;
				}
				EditorGUILayout.Separator();
				buttonRect = EditorGUILayout.BeginHorizontal();
				buttonRect.x = buttonRect.width / 2 - 100;
				buttonRect.width = 200;
				buttonRect.height = 18;
				GUI.skin = terrain.guiSkin;
				if (GUI.Button(buttonRect, "Generate Voronoi Features")) {
					// Undo...
					Terrain ter = (Terrain) terrain.GetComponent(typeof(Terrain));
					if (ter == null) {
						return;
					}
					TerrainData terData = ter.terrainData;
					Undo.RegisterUndo(terData, "Terrain Generator");
					TerrainToolkit.GeneratorProgressDelegate generatorProgressDelegate = new TerrainToolkit.GeneratorProgressDelegate(updateGeneratorProgress);
					terrain.generateTerrain(generatorProgressDelegate);
					EditorUtility.ClearProgressBar();
					GUIUtility.ExitGUI();
				}
				GUI.skin = null;
				EditorGUIUtility.LookLikeControls();
				EditorGUILayout.EndHorizontal();
				break;
				case 1:
				// Diamond square...
				EditorGUILayout.BeginHorizontal();
				if (GUI.changed) {
					EditorUtility.SetDirty(terrain);
				}
				GUI.changed = false;
				EditorGUILayout.PrefixLabel("Preset");
				string[] fractalPresetNames = new string[terrain.fractalPresets.Count + 1];
				int[] fractalPresetInts = new int[terrain.fractalPresets.Count + 1];
				fractalPresetNames[0] = "None";
				TerrainToolkit.fractalPresetData fractalPreset;
				for (i = 1; i <= terrain.fractalPresets.Count; i++) {
					fractalPreset = (TerrainToolkit.fractalPresetData) terrain.fractalPresets[i - 1];
					fractalPresetNames[i] = fractalPreset.presetName;
					fractalPresetInts[i] = i;
				}
				terrain.fractalPresetId = EditorGUILayout.IntPopup(terrain.fractalPresetId, fractalPresetNames, fractalPresetInts);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				if (GUI.changed && terrain.fractalPresetId > 0) {
					fractalPreset = (TerrainToolkit.fractalPresetData) terrain.fractalPresets[terrain.fractalPresetId - 1];
					terrain.setFractalPreset(fractalPreset);
				}
				if (GUI.changed) {
					EditorUtility.SetDirty(terrain);
				}
				GUI.changed = false;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Delta");
				terrain.diamondSquareDelta = EditorGUILayout.Slider(terrain.diamondSquareDelta, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Blend");
				terrain.diamondSquareBlend = EditorGUILayout.Slider(terrain.diamondSquareBlend, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				if (GUI.changed) {
					terrain.fractalPresetId = 0;
				}
				buttonRect = EditorGUILayout.BeginHorizontal();
				buttonRect.x = buttonRect.width / 2 - 100;
				buttonRect.width = 200;
				buttonRect.height = 18;
				GUI.skin = terrain.guiSkin;
				if (GUI.Button(buttonRect, "Generate Fractal Terrain")) {
					// Undo...
					Terrain ter = (Terrain) terrain.GetComponent(typeof(Terrain));
					if (ter == null) {
						return;
					}
					TerrainData terData = ter.terrainData;
					Undo.RegisterUndo(terData, "Terrain Generator");
					TerrainToolkit.GeneratorProgressDelegate generatorProgressDelegate = new TerrainToolkit.GeneratorProgressDelegate(updateGeneratorProgress);
					terrain.generateTerrain(generatorProgressDelegate);
					EditorUtility.ClearProgressBar();
					GUIUtility.ExitGUI();
				}
				GUI.skin = null;
				EditorGUIUtility.LookLikeControls();
				EditorGUILayout.EndHorizontal();
				break;
				case 2:
				// Perlin...
				EditorGUILayout.BeginHorizontal();
				if (GUI.changed) {
					EditorUtility.SetDirty(terrain);
				}
				GUI.changed = false;
				EditorGUILayout.PrefixLabel("Preset");
				string[] perlinPresetNames = new string[terrain.perlinPresets.Count + 1];
				int[] perlinPresetInts = new int[terrain.perlinPresets.Count + 1];
				perlinPresetNames[0] = "None";
				TerrainToolkit.perlinPresetData perlinPreset;
				for (i = 1; i <= terrain.perlinPresets.Count; i++) {
					perlinPreset = (TerrainToolkit.perlinPresetData) terrain.perlinPresets[i - 1];
					perlinPresetNames[i] = perlinPreset.presetName;
					perlinPresetInts[i] = i;
				}
				terrain.perlinPresetId = EditorGUILayout.IntPopup(terrain.perlinPresetId, perlinPresetNames, perlinPresetInts);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				if (GUI.changed && terrain.perlinPresetId > 0) {
					perlinPreset = (TerrainToolkit.perlinPresetData) terrain.perlinPresets[terrain.perlinPresetId - 1];
					terrain.setPerlinPreset(perlinPreset);
				}
				if (GUI.changed) {
					EditorUtility.SetDirty(terrain);
				}
				GUI.changed = false;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Frequency");
				terrain.perlinFrequency = EditorGUILayout.IntSlider(terrain.perlinFrequency, 1, 16);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Amplitude");
				terrain.perlinAmplitude = EditorGUILayout.Slider(terrain.perlinAmplitude, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Octaves");
				terrain.perlinOctaves = EditorGUILayout.IntSlider(terrain.perlinOctaves, 1, 12);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Blend");
				terrain.perlinBlend = EditorGUILayout.Slider(terrain.perlinBlend, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				if (GUI.changed) {
					terrain.perlinPresetId = 0;
				}
				buttonRect = EditorGUILayout.BeginHorizontal();
				buttonRect.x = buttonRect.width / 2 - 100;
				buttonRect.width = 200;
				buttonRect.height = 18;
				GUI.skin = terrain.guiSkin;
				if (GUI.Button(buttonRect, "Generate Perlin Terrain")) {
					// Undo...
					Terrain ter = (Terrain) terrain.GetComponent(typeof(Terrain));
					if (ter == null) {
						return;
					}
					TerrainData terData = ter.terrainData;
					Undo.RegisterUndo(terData, "Terrain Generator");
					TerrainToolkit.GeneratorProgressDelegate generatorProgressDelegate = new TerrainToolkit.GeneratorProgressDelegate(updateGeneratorProgress);
					terrain.generateTerrain(generatorProgressDelegate);
					EditorUtility.ClearProgressBar();
					GUIUtility.ExitGUI();
				}
				GUI.skin = null;
				EditorGUIUtility.LookLikeControls();
				EditorGUILayout.EndHorizontal();
				break;
				case 3:
				// Smooth...
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Iterations");
				terrain.smoothIterations = (int) EditorGUILayout.Slider(terrain.smoothIterations, 1, 5);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Blend");
				terrain.smoothBlend = EditorGUILayout.Slider(terrain.smoothBlend, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				buttonRect = EditorGUILayout.BeginHorizontal();
				buttonRect.x = buttonRect.width / 2 - 100;
				buttonRect.width = 200;
				buttonRect.height = 18;
				GUI.skin = terrain.guiSkin;
				if (GUI.Button(buttonRect, "Smooth Terrain")) {
					// Undo...
					Terrain ter = (Terrain) terrain.GetComponent(typeof(Terrain));
					if (ter == null) {
						return;
					}
					TerrainData terData = ter.terrainData;
					Undo.RegisterUndo(terData, "Smooth Terrain");
					TerrainToolkit.GeneratorProgressDelegate generatorProgressDelegate = new TerrainToolkit.GeneratorProgressDelegate(updateGeneratorProgress);
					terrain.generateTerrain(generatorProgressDelegate);
					EditorUtility.ClearProgressBar();
					GUIUtility.ExitGUI();
				}
				GUI.skin = null;
				EditorGUIUtility.LookLikeControls();
				EditorGUILayout.EndHorizontal();
				break;
				case 4:
				// Normalise...
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Minimum height");
				terrain.normaliseMin = EditorGUILayout.Slider(terrain.normaliseMin, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Maximum height");
				terrain.normaliseMax = EditorGUILayout.Slider(terrain.normaliseMax, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Blend");
				terrain.normaliseBlend = EditorGUILayout.Slider(terrain.normaliseBlend, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				buttonRect = EditorGUILayout.BeginHorizontal();
				buttonRect.x = buttonRect.width / 2 - 100;
				buttonRect.width = 200;
				buttonRect.height = 18;
				GUI.skin = terrain.guiSkin;
				if (GUI.Button(buttonRect, "Normalise Terrain")) {
					// Undo...
					Terrain ter = (Terrain) terrain.GetComponent(typeof(Terrain));
					if (ter == null) {
						return;
					}
					TerrainData terData = ter.terrainData;
					Undo.RegisterUndo(terData, "Normalise Terrain");
					TerrainToolkit.GeneratorProgressDelegate generatorProgressDelegate = new TerrainToolkit.GeneratorProgressDelegate(updateGeneratorProgress);
					terrain.generateTerrain(generatorProgressDelegate);
					EditorUtility.ClearProgressBar();
					GUIUtility.ExitGUI();
				}
				GUI.skin = null;
				EditorGUIUtility.LookLikeControls();
				EditorGUILayout.EndHorizontal();
				break;
			}
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			drawAdvancedSettingsGUI();
			break;
			// -------------------------------------------------------------------------------------------------------- EROSION TOOLS
			case 1:
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			string[] erosionOptions = new string[4];
			erosionOptions[0] = "Thermal";
			erosionOptions[1] = "Hydraulic";
			erosionOptions[2] = "Tidal";
			erosionOptions[3] = "Wind";
			terrain.erosionTypeInt = GUILayout.Toolbar(terrain.erosionTypeInt, erosionOptions);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			GUI.skin = terrain.guiSkin;
			GUILayout.Label("Filters");
			GUI.skin = null;
			EditorGUIUtility.LookLikeControls();
			EditorGUILayout.EndHorizontal();
			switch (terrain.erosionTypeInt) {
				case 0:
				// Thermal...
				EditorGUILayout.BeginHorizontal();
				if (GUI.changed) {
					EditorUtility.SetDirty(terrain);
				}
				GUI.changed = false;
				EditorGUILayout.PrefixLabel("Preset");
				string[] thermalErosionPresetNames = new string[terrain.thermalErosionPresets.Count + 1];
				int[] thermalErosionPresetInts = new int[terrain.thermalErosionPresets.Count + 1];
				thermalErosionPresetNames[0] = "None";
				TerrainToolkit.thermalErosionPresetData thermalErosionPreset;
				for (i = 1; i <= terrain.thermalErosionPresets.Count; i++) {
					thermalErosionPreset = (TerrainToolkit.thermalErosionPresetData) terrain.thermalErosionPresets[i - 1];
					thermalErosionPresetNames[i] = thermalErosionPreset.presetName;
					thermalErosionPresetInts[i] = i;
				}
				terrain.thermalErosionPresetId = EditorGUILayout.IntPopup(terrain.thermalErosionPresetId, thermalErosionPresetNames, thermalErosionPresetInts);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				if (GUI.changed && terrain.thermalErosionPresetId > 0) {
					thermalErosionPreset = (TerrainToolkit.thermalErosionPresetData) terrain.thermalErosionPresets[terrain.thermalErosionPresetId - 1];
					terrain.setThermalErosionPreset(thermalErosionPreset);
				}
				if (GUI.changed) {
					EditorUtility.SetDirty(terrain);
				}
				GUI.changed = false;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Iterations");
				terrain.thermalIterations = (int) EditorGUILayout.Slider(terrain.thermalIterations, 1, 250);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Minimum slope");
				terrain.thermalMinSlope = EditorGUILayout.Slider(terrain.thermalMinSlope, 0.01f, 89.99f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Falloff");
				terrain.thermalFalloff = EditorGUILayout.Slider(terrain.thermalFalloff, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				if (GUI.changed) {
					terrain.thermalErosionPresetId = 0;
				}
				buttonRect = EditorGUILayout.BeginHorizontal();
				buttonRect.x = buttonRect.width / 2 - 100;
				buttonRect.width = 200;
				buttonRect.height = 18;
				GUI.skin = terrain.guiSkin;
				if (GUI.Button(buttonRect, "Apply thermal erosion")) {
					// Undo
					Terrain ter = (Terrain) terrain.GetComponent(typeof(Terrain));
					if (ter == null) {
						return;
					}
					TerrainData terData = ter.terrainData;
					Undo.RegisterUndo(terData, "Terrain Erosion");
					// Start time...
					DateTime startTime = DateTime.Now;
					TerrainToolkit.ErosionProgressDelegate erosionProgressDelegate = new TerrainToolkit.ErosionProgressDelegate(updateErosionProgress);
					terrain.erodeAllTerrain(erosionProgressDelegate);
					EditorUtility.ClearProgressBar();
					TimeSpan processTime = DateTime.Now - startTime;
					Debug.Log("Process complete in: "+processTime.ToString());
					GUIUtility.ExitGUI();
				}
				GUI.skin = null;
				EditorGUIUtility.LookLikeControls();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				break;
				case 1:
				// Hydraulic...
				Rect toggleRect = EditorGUILayout.BeginHorizontal();
				toggleRect.x = 110;
				toggleRect.width = 180;
				toggleRect.height = 20;
				EditorGUILayout.PrefixLabel("Type");
				string[] toggleStates = new string[3];
				toggleStates[0] = "Fast";
				toggleStates[1] = "Full";
				toggleStates[2] = "Velocity";
				terrain.hydraulicTypeInt = GUI.Toolbar(toggleRect, terrain.hydraulicTypeInt, toggleStates);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				switch (terrain.hydraulicTypeInt) {
					case 0:
					// Fast...
					EditorGUILayout.BeginHorizontal();
					if (GUI.changed) {
						EditorUtility.SetDirty(terrain);
					}
					GUI.changed = false;
					EditorGUILayout.PrefixLabel("Preset");
					string[] fastHydraulicErosionPresetNames = new string[terrain.fastHydraulicErosionPresets.Count + 1];
					int[] fastHydraulicErosionPresetInts = new int[terrain.fastHydraulicErosionPresets.Count + 1];
					fastHydraulicErosionPresetNames[0] = "None";
					TerrainToolkit.fastHydraulicErosionPresetData fastHydraulicErosionPreset;
					for (i = 1; i <= terrain.fastHydraulicErosionPresets.Count; i++) {
						fastHydraulicErosionPreset = (TerrainToolkit.fastHydraulicErosionPresetData) terrain.fastHydraulicErosionPresets[i - 1];
						fastHydraulicErosionPresetNames[i] = fastHydraulicErosionPreset.presetName;
						fastHydraulicErosionPresetInts[i] = i;
					}
					terrain.fastHydraulicErosionPresetId = EditorGUILayout.IntPopup(terrain.fastHydraulicErosionPresetId, fastHydraulicErosionPresetNames, fastHydraulicErosionPresetInts);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Separator();
					if (GUI.changed && terrain.fastHydraulicErosionPresetId > 0) {
						fastHydraulicErosionPreset = (TerrainToolkit.fastHydraulicErosionPresetData) terrain.fastHydraulicErosionPresets[terrain.fastHydraulicErosionPresetId - 1];
						terrain.setFastHydraulicErosionPreset(fastHydraulicErosionPreset);
					}
					if (GUI.changed) {
						EditorUtility.SetDirty(terrain);
					}
					GUI.changed = false;
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Iterations");
					terrain.hydraulicIterations = (int) EditorGUILayout.Slider(terrain.hydraulicIterations, 1, 250);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Maximum slope");
					terrain.hydraulicMaxSlope = EditorGUILayout.Slider(terrain.hydraulicMaxSlope, 0.0f, 89.99f);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Falloff");
					terrain.hydraulicFalloff = EditorGUILayout.Slider(terrain.hydraulicFalloff, 0.0f, 1.0f);
					EditorGUILayout.EndHorizontal();
					if (GUI.changed) {
						terrain.fastHydraulicErosionPresetId = 0;
					}
					break;
					case 1:
					// Full...
					EditorGUILayout.BeginHorizontal();
					if (GUI.changed) {
						EditorUtility.SetDirty(terrain);
					}
					GUI.changed = false;
					EditorGUILayout.PrefixLabel("Preset");
					string[] fullHydraulicErosionPresetNames = new string[terrain.fullHydraulicErosionPresets.Count + 1];
					int[] fullHydraulicErosionPresetInts = new int[terrain.fullHydraulicErosionPresets.Count + 1];
					fullHydraulicErosionPresetNames[0] = "None";
					TerrainToolkit.fullHydraulicErosionPresetData fullHydraulicErosionPreset;
					for (i = 1; i <= terrain.fullHydraulicErosionPresets.Count; i++) {
						fullHydraulicErosionPreset = (TerrainToolkit.fullHydraulicErosionPresetData) terrain.fullHydraulicErosionPresets[i - 1];
						fullHydraulicErosionPresetNames[i] = fullHydraulicErosionPreset.presetName;
						fullHydraulicErosionPresetInts[i] = i;
					}
					terrain.fullHydraulicErosionPresetId = EditorGUILayout.IntPopup(terrain.fullHydraulicErosionPresetId, fullHydraulicErosionPresetNames, fullHydraulicErosionPresetInts);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Separator();
					if (GUI.changed && terrain.fullHydraulicErosionPresetId > 0) {
						fullHydraulicErosionPreset = (TerrainToolkit.fullHydraulicErosionPresetData) terrain.fullHydraulicErosionPresets[terrain.fullHydraulicErosionPresetId - 1];
						terrain.setFullHydraulicErosionPreset(fullHydraulicErosionPreset);
					}
					if (GUI.changed) {
						EditorUtility.SetDirty(terrain);
					}
					GUI.changed = false;
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Iterations");
					terrain.hydraulicIterations = (int) EditorGUILayout.Slider(terrain.hydraulicIterations, 1, 250);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Rainfall");
					terrain.hydraulicRainfall = EditorGUILayout.Slider(terrain.hydraulicRainfall, 0, 1);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Evaporation");
					terrain.hydraulicEvaporation = EditorGUILayout.Slider(terrain.hydraulicEvaporation, 0, 1);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Solubility");
					terrain.hydraulicSedimentSolubility = EditorGUILayout.Slider(terrain.hydraulicSedimentSolubility, 0, 1);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Saturation");
					terrain.hydraulicSedimentSaturation = EditorGUILayout.Slider(terrain.hydraulicSedimentSaturation, 0, 1);
					EditorGUILayout.EndHorizontal();
					if (GUI.changed) {
						terrain.fullHydraulicErosionPresetId = 0;
					}
					break;
					case 2:
					// Velocity...
					EditorGUILayout.BeginHorizontal();
					if (GUI.changed) {
						EditorUtility.SetDirty(terrain);
					}
					GUI.changed = false;
					EditorGUILayout.PrefixLabel("Preset");
					string[] velocityHydraulicErosionPresetNames = new string[terrain.velocityHydraulicErosionPresets.Count + 1];
					int[] velocityHydraulicErosionPresetInts = new int[terrain.velocityHydraulicErosionPresets.Count + 1];
					velocityHydraulicErosionPresetNames[0] = "None";
					TerrainToolkit.velocityHydraulicErosionPresetData velocityHydraulicErosionPreset;
					for (i = 1; i <= terrain.velocityHydraulicErosionPresets.Count; i++) {
						velocityHydraulicErosionPreset = (TerrainToolkit.velocityHydraulicErosionPresetData) terrain.velocityHydraulicErosionPresets[i - 1];
						velocityHydraulicErosionPresetNames[i] = velocityHydraulicErosionPreset.presetName;
						velocityHydraulicErosionPresetInts[i] = i;
					}
					terrain.velocityHydraulicErosionPresetId = EditorGUILayout.IntPopup(terrain.velocityHydraulicErosionPresetId, velocityHydraulicErosionPresetNames, velocityHydraulicErosionPresetInts);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Separator();
					if (GUI.changed && terrain.velocityHydraulicErosionPresetId > 0) {
						velocityHydraulicErosionPreset = (TerrainToolkit.velocityHydraulicErosionPresetData) terrain.velocityHydraulicErosionPresets[terrain.velocityHydraulicErosionPresetId - 1];
						terrain.setVelocityHydraulicErosionPreset(velocityHydraulicErosionPreset);
					}
					if (GUI.changed) {
						EditorUtility.SetDirty(terrain);
					}
					GUI.changed = false;
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Iterations");
					terrain.hydraulicIterations = (int) EditorGUILayout.Slider(terrain.hydraulicIterations, 1, 250);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Rainfall");
					terrain.hydraulicVelocityRainfall = EditorGUILayout.Slider(terrain.hydraulicVelocityRainfall, 0, 1);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Evaporation");
					terrain.hydraulicVelocityEvaporation = EditorGUILayout.Slider(terrain.hydraulicVelocityEvaporation, 0, 1);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Solubility");
					terrain.hydraulicVelocitySedimentSolubility = EditorGUILayout.Slider(terrain.hydraulicVelocitySedimentSolubility, 0, 1);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Saturation");
					terrain.hydraulicVelocitySedimentSaturation = EditorGUILayout.Slider(terrain.hydraulicVelocitySedimentSaturation, 0, 1);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Velocity");
					terrain.hydraulicVelocity = EditorGUILayout.Slider(terrain.hydraulicVelocity, 0, 10);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Momentum");
					terrain.hydraulicMomentum = EditorGUILayout.Slider(terrain.hydraulicMomentum, 0, 10);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Entropy");
					terrain.hydraulicEntropy = EditorGUILayout.Slider(terrain.hydraulicEntropy, 0, 1);
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Downcutting");
					terrain.hydraulicDowncutting = EditorGUILayout.Slider(terrain.hydraulicDowncutting, 0, 1);
					EditorGUILayout.EndHorizontal();
					if (GUI.changed) {
						terrain.velocityHydraulicErosionPresetId = 0;
					}
					break;
				}
				EditorGUILayout.Separator();
				buttonRect = EditorGUILayout.BeginHorizontal();
				buttonRect.x = buttonRect.width / 2 - 100;
				buttonRect.width = 200;
				buttonRect.height = 18;
				GUI.skin = terrain.guiSkin;
				if (GUI.Button(buttonRect, "Apply hydraulic erosion")) {
					// Undo
					Terrain ter = (Terrain) terrain.GetComponent(typeof(Terrain));
					if (ter == null) {
						return;
					}
					TerrainData terData = ter.terrainData;
					Undo.RegisterUndo(terData, "Terrain Erosion");
					// Start time...
					DateTime startTime = DateTime.Now;
					TerrainToolkit.ErosionProgressDelegate erosionProgressDelegate = new TerrainToolkit.ErosionProgressDelegate(updateErosionProgress);
					terrain.erodeAllTerrain(erosionProgressDelegate);
					EditorUtility.ClearProgressBar();
					TimeSpan processTime = DateTime.Now - startTime;
					Debug.Log("Process complete in: "+processTime.ToString());
					GUIUtility.ExitGUI();
				}
				GUI.skin = null;
				EditorGUIUtility.LookLikeControls();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				break;
				case 2:
				// Tidal...
				EditorGUILayout.BeginHorizontal();
				if (GUI.changed) {
					EditorUtility.SetDirty(terrain);
				}
				GUI.changed = false;
				EditorGUILayout.PrefixLabel("Preset");
				string[] tidalErosionPresetNames = new string[terrain.tidalErosionPresets.Count + 1];
				int[] tidalErosionPresetInts = new int[terrain.tidalErosionPresets.Count + 1];
				tidalErosionPresetNames[0] = "None";
				TerrainToolkit.tidalErosionPresetData tidalErosionPreset;
				for (i = 1; i <= terrain.tidalErosionPresets.Count; i++) {
					tidalErosionPreset = (TerrainToolkit.tidalErosionPresetData) terrain.tidalErosionPresets[i - 1];
					tidalErosionPresetNames[i] = tidalErosionPreset.presetName;
					tidalErosionPresetInts[i] = i;
				}
				terrain.tidalErosionPresetId = EditorGUILayout.IntPopup(terrain.tidalErosionPresetId, tidalErosionPresetNames, tidalErosionPresetInts);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				if (GUI.changed && terrain.tidalErosionPresetId > 0) {
					tidalErosionPreset = (TerrainToolkit.tidalErosionPresetData) terrain.tidalErosionPresets[terrain.tidalErosionPresetId - 1];
					terrain.setTidalErosionPreset(tidalErosionPreset);
				}
				if (GUI.changed) {
					EditorUtility.SetDirty(terrain);
				}
				GUI.changed = false;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Iterations");
				terrain.tidalIterations = (int) EditorGUILayout.Slider(terrain.tidalIterations, 1, 250);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Sea level");
				terrain.tidalSeaLevel = EditorGUILayout.FloatField(terrain.tidalSeaLevel);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Tidal range");
				terrain.tidalRangeAmount = EditorGUILayout.FloatField(terrain.tidalRangeAmount);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Cliff limit");
				terrain.tidalCliffLimit = EditorGUILayout.Slider(terrain.tidalCliffLimit, 0.0f, 90.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				if (GUI.changed) {
					terrain.tidalErosionPresetId = 0;
				}
				buttonRect = EditorGUILayout.BeginHorizontal();
				buttonRect.x = buttonRect.width / 2 - 100;
				buttonRect.width = 200;
				buttonRect.height = 18;
				GUI.skin = terrain.guiSkin;
				if (GUI.Button(buttonRect, "Apply tidal erosion")) {
					// Undo
					Terrain ter = (Terrain) terrain.GetComponent(typeof(Terrain));
					if (ter == null) {
						return;
					}
					TerrainData terData = ter.terrainData;
					Undo.RegisterUndo(terData, "Terrain Erosion");
					// Start time...
					DateTime startTime = DateTime.Now;
					TerrainToolkit.ErosionProgressDelegate erosionProgressDelegate = new TerrainToolkit.ErosionProgressDelegate(updateErosionProgress);
					terrain.erodeAllTerrain(erosionProgressDelegate);
					EditorUtility.ClearProgressBar();
					TimeSpan processTime = DateTime.Now - startTime;
					Debug.Log("Process complete in: "+processTime.ToString());
					GUIUtility.ExitGUI();
				}
				GUI.skin = null;
				EditorGUIUtility.LookLikeControls();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				break;
				case 3:
				// Wind...
				EditorGUILayout.BeginHorizontal();
				if (GUI.changed) {
					EditorUtility.SetDirty(terrain);
				}
				GUI.changed = false;
				EditorGUILayout.PrefixLabel("Preset");
				string[] windErosionPresetNames = new string[terrain.windErosionPresets.Count + 1];
				int[] windErosionPresetInts = new int[terrain.windErosionPresets.Count + 1];
				windErosionPresetNames[0] = "None";
				TerrainToolkit.windErosionPresetData windErosionPreset;
				for (i = 1; i <= terrain.windErosionPresets.Count; i++) {
					windErosionPreset = (TerrainToolkit.windErosionPresetData) terrain.windErosionPresets[i - 1];
					windErosionPresetNames[i] = windErosionPreset.presetName;
					windErosionPresetInts[i] = i;
				}
				terrain.windErosionPresetId = EditorGUILayout.IntPopup(terrain.windErosionPresetId, windErosionPresetNames, windErosionPresetInts);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				if (GUI.changed && terrain.windErosionPresetId > 0) {
					windErosionPreset = (TerrainToolkit.windErosionPresetData) terrain.windErosionPresets[terrain.windErosionPresetId - 1];
					terrain.setWindErosionPreset(windErosionPreset);
				}
				if (GUI.changed) {
					EditorUtility.SetDirty(terrain);
				}
				GUI.changed = false;
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Iterations");
				terrain.windIterations = (int) EditorGUILayout.Slider(terrain.windIterations, 1, 250);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				terrain.windDirection = EditorGUILayout.Slider("Wind direction", terrain.windDirection, 0.0f, 360.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				terrain.windForce = EditorGUILayout.Slider("Wind force", terrain.windForce, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				terrain.windLift = EditorGUILayout.Slider("Lift", terrain.windLift, 0.0f, 0.01f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				terrain.windGravity = EditorGUILayout.Slider("Gravity", terrain.windGravity, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				terrain.windCapacity = EditorGUILayout.Slider("Capacity", terrain.windCapacity, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				terrain.windEntropy = EditorGUILayout.Slider("Entropy", terrain.windEntropy, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.BeginHorizontal();
				terrain.windSmoothing = EditorGUILayout.Slider("Smoothing", terrain.windSmoothing, 0.0f, 1.0f);
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				if (GUI.changed) {
					terrain.windErosionPresetId = 0;
				}
				buttonRect = EditorGUILayout.BeginHorizontal();
				buttonRect.x = buttonRect.width / 2 - 100;
				buttonRect.width = 200;
				buttonRect.height = 18;
				GUI.skin = terrain.guiSkin;
				if (GUI.Button(buttonRect, "Apply wind erosion")) {
					// Undo
					Terrain ter = (Terrain) terrain.GetComponent(typeof(Terrain));
					if (ter == null) {
						return;
					}
					TerrainData terData = ter.terrainData;
					Undo.RegisterUndo(terData, "Terrain Erosion");
					// Start time...
					DateTime startTime = DateTime.Now;
					TerrainToolkit.ErosionProgressDelegate erosionProgressDelegate = new TerrainToolkit.ErosionProgressDelegate(updateErosionProgress);
					terrain.erodeAllTerrain(erosionProgressDelegate);
					EditorUtility.ClearProgressBar();
					TimeSpan processTime = DateTime.Now - startTime;
					Debug.Log("Process complete in: "+processTime.ToString());
					GUIUtility.ExitGUI();
				}
				GUI.skin = null;
				EditorGUIUtility.LookLikeControls();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				break;
			}
			if (terrain.erosionTypeInt == 0 || terrain.erosionTypeInt == 2 || (terrain.erosionTypeInt == 1 && terrain.hydraulicTypeInt == 0)) {
				EditorGUILayout.Separator();
				drawBrushToolsGUI();
			} else {
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
			}
			EditorGUILayout.Separator();
			drawAdvancedSettingsGUI();
			break;
			// -------------------------------------------------------------------------------------------------------- TEXTURING TOOLS
			case 2:
			Terrain ter = (Terrain) terrain.GetComponent(typeof(Terrain));
			if (ter == null) {
				return;
			}
			TerrainData terData = ter.terrainData;
			terrain.splatPrototypes = terData.splatPrototypes;
			EditorGUILayout.Separator();
			float mouseX;
			EditorGUILayout.BeginHorizontal();
			GUI.skin = terrain.guiSkin;
			GUILayout.Label("Texture Slope");
			GUI.skin = null;
			EditorGUIUtility.LookLikeControls();
			EditorGUILayout.EndHorizontal();
			Rect gradientRect = EditorGUILayout.BeginHorizontal();
			float gradientWidth = gradientRect.width - 55;
			gradientRect.width = 15;
			gradientRect.height = 19;
			GUI.skin = terrain.guiSkin;
			// Slope stop 1...
			if (dragControl == "slopeStop1" && Event.current.type == EventType.MouseDrag) {
				mouseX = Event.current.mousePosition.x - 7;
				if (mouseX < 20) {
					mouseX = 20;
				} else if (mouseX > 19 + gradientWidth * (terrain.slopeBlendMaxAngle / 90)) {
					mouseX = 19 + gradientWidth * (terrain.slopeBlendMaxAngle / 90);
				}
				gradientRect.x = mouseX;
				terrain.slopeBlendMinAngle = ((mouseX - 20) / (gradientWidth + 1)) * 90;
			} else {
				gradientRect.x = 20 + gradientWidth * (terrain.slopeBlendMinAngle / 90);
			}
			if (Event.current.type == EventType.MouseDown && gradientRect.Contains(Event.current.mousePosition)) {
				dragControl = "slopeStop1";
			}
			if (dragControl == "slopeStop1" && Event.current.type == EventType.MouseUp) {
				dragControl = "";
			}
			GUI.Box(gradientRect, "", "slopeStop1");
			// Slope stop 2...
			if (dragControl == "slopeStop2" && Event.current.type == EventType.MouseDrag) {
				mouseX = Event.current.mousePosition.x - 7;
				if (mouseX < 21 + gradientWidth * (terrain.slopeBlendMinAngle / 90)) {
					mouseX = 21 + gradientWidth * (terrain.slopeBlendMinAngle / 90);
				} else if (mouseX > 21 + gradientWidth) {
					mouseX = 21 + gradientWidth;
				}
				gradientRect.x = mouseX;
				terrain.slopeBlendMaxAngle = ((mouseX - 20) / (gradientWidth + 1)) * 90;
			} else {
				gradientRect.x = 20 + gradientWidth * (terrain.slopeBlendMaxAngle / 90);
			}
			if (Event.current.type == EventType.MouseDown && gradientRect.Contains(Event.current.mousePosition)) {
				dragControl = "slopeStop2";
			}
			if (dragControl == "slopeStop2" && Event.current.type == EventType.MouseUp) {
				dragControl = "";
			}
			GUI.Box(gradientRect, "", "slopeStop2");
			gradientRect.y += 19;
			gradientRect.width = gradientWidth * (terrain.slopeBlendMinAngle / 90);
			gradientRect.x = 27;
			GUI.Box(gradientRect, "", "black");
			gradientRect.width = gradientWidth * ((terrain.slopeBlendMaxAngle / 90) - (terrain.slopeBlendMinAngle / 90));
			gradientRect.x = 27 + gradientWidth * (terrain.slopeBlendMinAngle / 90);
			GUI.Box(gradientRect, "", "blackToWhite");
			gradientRect.width = gradientWidth - gradientWidth * (terrain.slopeBlendMaxAngle / 90);
			gradientRect.x = 27 + gradientWidth * (terrain.slopeBlendMaxAngle / 90);
			GUI.Box(gradientRect, "", "white");
			GUI.skin = null;
			EditorGUIUtility.LookLikeControls();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Cliff start");
			terrain.slopeBlendMinAngle = EditorGUILayout.FloatField(terrain.slopeBlendMinAngle);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Cliff end");
			terrain.slopeBlendMaxAngle = EditorGUILayout.FloatField(terrain.slopeBlendMaxAngle);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUI.skin = terrain.guiSkin;
			GUILayout.Label("Texture Height");
			GUI.skin = null;
			EditorGUIUtility.LookLikeControls();
			EditorGUILayout.EndHorizontal();
			gradientRect = EditorGUILayout.BeginHorizontal();
			gradientWidth = gradientRect.width - 55;
			gradientRect.width = 15;
			gradientRect.height = 19;
			Rect gradientRect2 = gradientRect;
			gradientRect2.y += 19;
			GUI.skin = terrain.guiSkin;
			string[] gradientStyles = new string[9];
			gradientStyles[0] = "red";
			gradientStyles[1] = "redToYellow";
			gradientStyles[2] = "yellow";
			gradientStyles[3] = "yellowToGreen";
			gradientStyles[4] = "green";
			gradientStyles[5] = "greenToCyan";
			gradientStyles[6] = "cyan";
			gradientStyles[7] = "cyanToBlue";
			gradientStyles[8] = "blue";
			List<float> heightBlendPoints = terrain.heightBlendPoints;
			int numPoints = heightBlendPoints.Count;
			float firstLimit = 1;
			if (numPoints > 0) {
				firstLimit = (float) heightBlendPoints[0];
			} else {
				gradientRect.x = 20;
				GUI.Box(gradientRect, "", "greyStop");
				gradientRect.x = 20 + gradientWidth;
				GUI.Box(gradientRect, "", "greyStop");
			}
			gradientRect2.width = gradientWidth * firstLimit;
			gradientRect2.x = 27;
			if (terrain.splatPrototypes.Length < 2) {
				GUI.Box(gradientRect2, "", "grey");
			} else {
				GUI.Box(gradientRect2, "", "red");
			}
			for (i = 0; i < numPoints; i++) {
				// Height stop...
				float lowerLimit = 0;
				float upperLimit = 1;
				if (i > 0) {
					lowerLimit = (float) heightBlendPoints[i - 1];
				}
				if (i < numPoints - 1) {
					upperLimit = (float) heightBlendPoints[i + 1];
				}
				if (dragControl == "heightStop"+i && Event.current.type == EventType.MouseDrag) {
					mouseX = Event.current.mousePosition.x - 7;
					if (mouseX < 20 + gradientWidth * lowerLimit) {
						mouseX = 20 + gradientWidth * lowerLimit;
					} else if (mouseX > 19 + gradientWidth * upperLimit) {
						mouseX = 19 + gradientWidth * upperLimit;
					}
					gradientRect.x = mouseX;
					heightBlendPoints[i] = (mouseX - 20) / (gradientWidth + 1);
				} else {
					gradientRect.x = 20 + gradientWidth * (float) heightBlendPoints[i];
				}
				if (Event.current.type == EventType.MouseDown && gradientRect.Contains(Event.current.mousePosition)) {
					dragControl = "heightStop"+i;
				}
				if (dragControl == "heightStop"+i && Event.current.type == EventType.MouseUp) {
					dragControl = "";
				}
				int stopNum = (int) Mathf.Ceil((float) i / 2) + 1;
				if (i % 2 == 0) {
					GUI.Box(gradientRect, ""+stopNum, "blackStop");
				} else {
					GUI.Box(gradientRect, ""+stopNum, "whiteStop");
				}
				gradientRect2.width = gradientWidth * (upperLimit - (float) heightBlendPoints[i]);
				gradientRect2.x = 27 + gradientWidth * (float) heightBlendPoints[i];
				GUI.Box(gradientRect2, "", gradientStyles[i + 1]);
			}
			GUI.skin = null;
			EditorGUIUtility.LookLikeControls();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			string startOrEnd = "end";
			for (i = 0; i < numPoints; i++) {
				EditorGUILayout.BeginHorizontal();
				int floatFieldNum = (int) Mathf.Ceil((float) i / 2) + 1;
				EditorGUILayout.PrefixLabel("Texture "+floatFieldNum+" "+startOrEnd);
				heightBlendPoints[i] = EditorGUILayout.FloatField((float) heightBlendPoints[i]);
				EditorGUILayout.EndHorizontal();
				if (startOrEnd == "end") {
					startOrEnd = "start";
				} else {
					startOrEnd = "end";
				}
			}
			terrain.heightBlendPoints = heightBlendPoints;
			EditorGUILayout.BeginHorizontal();
			GUI.skin = terrain.guiSkin;
			GUILayout.Label("Textures");
			GUI.skin = null;
			EditorGUIUtility.LookLikeControls();
			EditorGUILayout.EndHorizontal();
			int nTextures = 0;
			EditorGUILayout.Separator();
			if (GUI.changed) {
				EditorUtility.SetDirty(terrain);
			}
			GUI.changed = false;
			EditorGUILayout.BeginHorizontal();
			foreach (SplatPrototype splatPrototype in terrain.splatPrototypes) {
				EditorGUIUtility.LookLikeControls(80, 0);
				Rect textureRect = EditorGUILayout.BeginHorizontal();
				if (nTextures == 0) {
					splatPrototype.texture = EditorGUILayout.ObjectField("Cliff texture", splatPrototype.texture, typeof(Texture2D)) as Texture2D;
				} else {
					splatPrototype.texture = EditorGUILayout.ObjectField("Texture "+nTextures, splatPrototype.texture, typeof(Texture2D)) as Texture2D;
				}
				GUI.skin = terrain.guiSkin;
				textureRect.x += 146;
				textureRect.width = 18;
				textureRect.height = 18;
				if (GUI.Button(textureRect, "", "deleteButton")) {
					GUI.changed = true;
					terrain.deleteSplatPrototype(terrain.tempTexture, nTextures);
					EditorUtility.SetDirty(terrain);
				}
				GUI.skin = null;
				EditorGUIUtility.LookLikeControls();
				EditorGUILayout.EndHorizontal();
				if (nTextures % 2 == 1) {
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Separator();
					EditorGUILayout.BeginHorizontal();
				}
				nTextures++;
				if (nTextures > 5) {
					break;
				}
			}
			EditorGUIUtility.LookLikeControls();
			EditorGUILayout.EndHorizontal();
			if (GUI.changed) {
				terData.splatPrototypes = terrain.splatPrototypes;
			}
			if (nTextures == 0 && !assignTexture) {
				EditorGUILayout.BeginHorizontal();
				GUI.skin = terrain.guiSkin;
				GUILayout.Label("No textures have been assigned! Assign a texture.", "errorText");
				GUI.skin = null;
				EditorGUIUtility.LookLikeControls();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
			}
			if (nTextures < 6) {
				EditorGUILayout.Separator();
				buttonRect = EditorGUILayout.BeginHorizontal();
				buttonRect.x = buttonRect.width / 2 - 50;
				buttonRect.width = 100;
				buttonRect.height = 18;
				if (GUI.Button(buttonRect, "Add texture")) {
					terrain.addSplatPrototype(terrain.defaultTexture, nTextures);
					terData.splatPrototypes = terrain.splatPrototypes;
					EditorUtility.SetDirty(terrain);
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
			}
			EditorGUILayout.Separator();
			buttonRect = EditorGUILayout.BeginHorizontal();
			buttonRect.x = buttonRect.width / 2 - 100;
			buttonRect.width = 200;
			buttonRect.height = 18;
			GUI.skin = terrain.guiSkin;
			if (nTextures < 2) {
				GUI.Box(buttonRect, "Apply procedural texture", "disabledButton");
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				EditorGUILayout.Separator();
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("This feature is disabled! You must assign at least 2 textures.", "errorText");
			} else {
				if (GUI.Button(buttonRect, "Apply procedural texture")) {
					// Undo not supported!
					TerrainToolkit.TextureProgressDelegate textureProgressDelegate = new TerrainToolkit.TextureProgressDelegate(updateTextureProgress);
					terrain.textureTerrain(textureProgressDelegate);
					EditorUtility.ClearProgressBar();
					GUIUtility.ExitGUI();
				}
			}
			GUI.skin = null;
			EditorGUIUtility.LookLikeControls();
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			drawAdvancedSettingsGUI();
			// If the user has added or removed textures in the Terrain component, correct the number of blend points...
			if (Event.current.type == EventType.Repaint) {
				if (numPoints % 2 != 0) {
					terrain.deleteAllBlendPoints();
				}
				int correctNumPoints = (nTextures - 2) * 2;
				if (nTextures < 3) {
					correctNumPoints = 0;
				}
				if (numPoints < correctNumPoints) {
					terrain.addBlendPoints();
				} else if (numPoints > correctNumPoints) {
					terrain.deleteBlendPoints();
				}
			}
			break;
		}
		if (GUI.changed) {
			EditorUtility.SetDirty(terrain);
		}
	}
	
	public void OnSceneGUI() {
		TerrainToolkit terrain = (TerrainToolkit) target as TerrainToolkit;
		if (Event.current.type == EventType.MouseDown) {
			terrain.isBrushPainting = true;
		}
		if (Event.current.type == EventType.MouseUp) {
			terrain.isBrushPainting = false;
		}
		if (Event.current.shift) {
			if (!terrain.isBrushPainting) {
				// Undo...
				Terrain ter = (Terrain) terrain.GetComponent(typeof(Terrain));
				if (ter == null) {
					return;
				}
				TerrainData terData = ter.terrainData;
				Undo.RegisterUndo(terData, "Terrain Erosion Brush");
			}
			terrain.isBrushPainting = true;
		} else {
			terrain.isBrushPainting = false;
		}
		terrain.isBrushHidden = false;
		if (terrain.isBrushOn) {
			Vector2 mouse = Event.current.mousePosition;
			mouse.y = Camera.current.pixelHeight - mouse.y + 20;
			Ray ray = Camera.current.ScreenPointToRay(mouse);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
				if (hit.transform.GetComponent("TerrainToolkit")) {
					terrain.brushPosition = hit.point;
					if (terrain.isBrushPainting) {
						// Paint...
						terrain.paint();
					}
				}
			} else {
				terrain.isBrushHidden = true;
			}
		}
	}
	
	private void drawBrushToolsGUI() {
		TerrainToolkit terrain = (TerrainToolkit) target as TerrainToolkit;
		EditorGUILayout.Separator();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		GUI.skin = terrain.guiSkin;
		GUILayout.Label("Brushes");
		GUI.skin = null;
		EditorGUIUtility.LookLikeControls();
		EditorGUILayout.EndHorizontal();
		Rect toggleRect = EditorGUILayout.BeginHorizontal();
		toggleRect.x = 110;
		toggleRect.width = 80;
		toggleRect.height = 20;
		EditorGUILayout.PrefixLabel("Brush");
		string[] brushStates = new string[2];
		brushStates[0] = "Off";
		brushStates[1] = "On";
		int brushInt = 0;
		if (terrain.isBrushOn) {
			brushInt = 1;
		}
		brushInt = GUI.Toolbar(toggleRect, brushInt, brushStates);
		bool brushBool = false;
		if (brushInt == 1) {
			brushBool = true;
		}
		terrain.isBrushOn = brushBool;
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("             HINTS:");
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("             1. Hold down the SHIFT key to use the brush");
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("             2. Use the brush PRESET for best results");
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Separator();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Brush size");
		terrain.brushSize = EditorGUILayout.Slider(terrain.brushSize, 1, 100);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Opacity");
		terrain.brushOpacity = EditorGUILayout.Slider(terrain.brushOpacity, 0, 1);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.PrefixLabel("Softness");
		terrain.brushSoftness = EditorGUILayout.Slider(terrain.brushSoftness, 0, 1);
		EditorGUILayout.EndHorizontal();
	}
	
	private void drawAdvancedSettingsGUI() {
		TerrainToolkit terrain = (TerrainToolkit) target as TerrainToolkit;
		EditorGUILayout.BeginHorizontal();
		showAdvancedSettings = EditorGUILayout.Foldout(showAdvancedSettings, "Advanced settings");
		EditorGUILayout.EndHorizontal();
		if (showAdvancedSettings) {
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Cell neighbourhood");
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.BeginHorizontal();
			GUIContent[] neighbourhoodOptions = new GUIContent[2];
			neighbourhoodOptions[0] = new GUIContent("Moore", terrain.mooreIcon);
			neighbourhoodOptions[1] = new GUIContent("Von Neumann", terrain.vonNeumannIcon);
			terrain.neighbourhoodInt = GUILayout.Toolbar(terrain.neighbourhoodInt, neighbourhoodOptions);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label("Use difference maps in brush mode");
			EditorGUILayout.EndHorizontal();
			Rect toggleRect = EditorGUILayout.BeginHorizontal();
			toggleRect.x = 110;
			toggleRect.width = 80;
			toggleRect.height = 20;
			string[] diffMapStates = new string[2];
			diffMapStates[0] = "Off";
			diffMapStates[1] = "On";
			int diffMapInt = 0;
			if (terrain.useDifferenceMaps) {
				diffMapInt = 1;
			}
			diffMapInt = GUI.Toolbar(toggleRect, diffMapInt, diffMapStates);
			bool diffMapBool = false;
			if (diffMapInt == 1) {
				diffMapBool = true;
			}
			terrain.useDifferenceMaps = diffMapBool;
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			EditorGUILayout.BeginHorizontal();
			if (GUILayout.Button("Reload Presets")) {
				terrain.presetsInitialised = false;
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.Separator();
		}
	}
	
	public void updateErosionProgress(string titleString, string displayString, int iteration, int nIterations, float percentComplete) {
		EditorUtility.DisplayProgressBar(titleString, displayString+" Iteration "+iteration+" of "+nIterations+". Please wait.", percentComplete);
	}
	
	public void updateTextureProgress(string titleString, string displayString, float percentComplete) {
		EditorUtility.DisplayProgressBar(titleString, displayString, percentComplete);
	}
	
	public void updateGeneratorProgress(string titleString, string displayString, float percentComplete) {
		EditorUtility.DisplayProgressBar(titleString, displayString, percentComplete);
	}
}

// -------------------------------------------------------------------------------------------------------- END