// 
// Copyright (c) 2023 Kim Hyun Deok
//
// OutdoorGenerator.cs
//
using NUnit.Framework.Constraints;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using static UnityEngine.Mesh;

public struct TerrainData
{
    public float3 position;
    public float noise;
    public TerrainType type;
}

[Serializable]
public struct TerrainColor
{
    public TerrainType type;
    public Color color;
    public TerrainColor(TerrainType type, Color color)
    {
        this.type = type;
        this.color = color;
    }
}

public class OutdoorGenerator : MonoBehaviour
{
    [Header("Noise Settings")]
    [Range(10, 250)] public int levelWidth = 100;
    [Range(10, 250)] public int levelHeight = 100;
    [Range(1, 20)] public float noiseScale = 10f;
    [Range(1, 8)] public int octaves = 4;
    [Range(0, 1)] public float persistance = 0.5f;
    [Range(1, 4)] public float lacunarity = 2f;
    public int seed;
    public Vector2 offset;
    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;
    public bool autoUpdate = true;

    [Header("Terrain Settings")]
    public int chunkSize = 64;
    [Range(0, 6)] public int levelOfDetail;
    public TerrainColor[] terrainColor = new TerrainColor[] 
    { 
        new TerrainColor(TerrainType.None, Color.clear),
        new TerrainColor(TerrainType.Grass, Color.clear),
        new TerrainColor(TerrainType.GrassRoad, Color.clear),
        new TerrainColor(TerrainType.Water, Color.clear),
        new TerrainColor(TerrainType.Stone, Color.clear),
        new TerrainColor(TerrainType.StoneRoad, Color.clear),
    };

    [Header("Game Level Settings")]
    public RoadGenerator roadGenerator;
    public Vector3 entrancePosition;
    public Vector3 exitPosition;


    private TerrainData[] terrain;
    private MeshData terrainMesh;

    public void GenerateMap()
    {
        GenerateNoiseMap();
        GenerateTerrainMesh();
        DebugDraw();
    }

    private void GenerateNoiseMap()
    {
        // Generate Perlin noise 
        terrain = new TerrainData[levelHeight * levelWidth];
        float[] noiseMap = Noise.GenerateNoiseMap(levelWidth, levelHeight, seed, noiseScale, octaves, persistance, lacunarity, offset);
        for (int i = 0; i < levelHeight; i++)
        {
            for (int j = 0; j < levelWidth; j++)
            {
                int index = i * levelWidth + j;
                terrain[index].noise = noiseMap[index];
                terrain[index].type = TerrainType.Grass;
            }
        }
        // Modify noise using Spline(road)
        roadGenerator.Generate(ref terrain, levelWidth, levelHeight);
    }
    private void GenerateTerrainMesh()
    {
        terrainMesh = MeshGenerator.GenerateMesh(ref terrain, levelWidth, levelHeight, meshHeightMultiplier, meshHeightCurve, levelOfDetail);
    }
    private void DebugDraw()
    {
        MapDisplay display = FindObjectOfType<MapDisplay>();
        Texture2D texture = new Texture2D(levelWidth, levelHeight);
        Color[] colorMap = new Color[levelWidth * levelHeight];
        for (int y = 0; y < levelHeight; y++)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                int index = y * levelWidth + x;
                colorMap[index] = terrainColor[(int)terrain[index].type].color;
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();

        display.DrawMesh(terrainMesh, texture);
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
