// 
// Copyright (c) 2023 Kim Hyun Deok
//
// OutdoorGeneratorEditor.cs
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OutdoorGenerator))]
public class OutdoorGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        OutdoorGenerator outdoorGenerator = (OutdoorGenerator)target;
        if (DrawDefaultInspector())
        {
            if (outdoorGenerator.autoUpdate)
            {
                outdoorGenerator.GenerateMap();
            }
        }

        if (GUILayout.Button("Generate"))
        {
            outdoorGenerator.GenerateMap();
        }

    }
}
