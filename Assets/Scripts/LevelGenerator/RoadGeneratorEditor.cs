// 
// Copyright (c) 2023 Kim Hyun Deok
//
// RoadGeneratorEditor.cs
//

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoadGenerator))]
public class RandomizeSplineButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        RoadGenerator roadGenerator = (RoadGenerator)target;
        if (GUILayout.Button("Generate"))
        {
            roadGenerator.Generate();
        }
    }
}