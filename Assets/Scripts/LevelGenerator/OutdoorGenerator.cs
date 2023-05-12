// 
// Copyright (c) 2023 Kim Hyun Deok
//
// OutdoorGenerator.cs
//
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public struct TerrainData
{
    public float3 position;
    public float noise;
}

public class OutdoorGenerator : MonoBehaviour
{
    [Header("Noise Settings")]
    public int levelWidth = 100;
    public int levelHeight = 100;
    public float noiseScale = 10f;
    public int octaves = 4;
    [Range(0,1)] public float persistance = 0.5f;
    public float lacunarity = 2f;
    public int seed;
    public Vector2 offset;
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;
    public bool autoUpdate = true;

    [Header("Game Level Settings")] 
    public RoadGenerator roadGenerator;
    public Vector3 entrancePosition;
    public Vector3 exitPosition;

    private TerrainData[] terrain;
    private float[] heightMap;
    
    public void GenerateMap()
    {
        //
        // Generate Noise
        //
        terrain = new TerrainData[levelHeight * levelWidth];
        float[] noiseMap = Noise.GenerateNoiseMap(levelWidth, levelHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        for(int i=0; i<levelHeight; i++)
        {
            for(int j=0; j<levelWidth; j++)
            {
                int index = i * levelWidth + j;
                terrain[index].noise = noiseMap[index];
            }
        }
        roadGenerator.Generate(ref noiseMap, levelWidth, levelHeight);
        
        //
        // Generate Mesh
        //
        MeshData meshData = MeshGenerator.GenerateMesh(noiseMap, levelWidth, levelHeight, meshHeightMultiplier, meshHeightCurve);

        // 
        // Debug draw
        // 
        MapDisplay display = FindObjectOfType<MapDisplay>();
        Texture2D texture = new Texture2D(levelWidth, levelHeight);
        Color[] colorMap = new Color[levelWidth * levelHeight];
        for (int y = 0; y < levelHeight; y++)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                colorMap[y * levelWidth + x] = Color.Lerp(Color.black, Color.white, noiseMap[y * levelWidth + x]);
            }
        }
        texture.SetPixels(colorMap);
        texture.Apply();

        display.DrawMesh(meshData, texture);
    }

    private void OnValidate()
    {
        if (levelWidth < 1)
        {
            levelWidth = 1;
        }

        if (levelHeight < 1)
        {
            levelHeight = 1;
        }

        if (lacunarity < 1)
        {
            lacunarity = 1;
        }

        if (octaves < 0)
        {
            octaves = 0;
        }
    }
}
