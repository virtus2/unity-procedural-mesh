// 
// Copyright (c) 2023 Kim Hyun Deok
//
// RoadGenerator.cs
//
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;
using Random = UnityEngine.Random;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private int inputSeed = 0;
    [SerializeField] private int randomSeed;

    [Header("중간점의 개수")]
    [Range(1, 100)]
    [SerializeField] private int numKnots = 10;

    [Header("시작점의 z축 무작위 변화량")]
    [Range(0, 100)]
    [SerializeField] private float startZRange = 1f;

    [Header("중간점의 z축 무작위 변화량")]
    [Range(0f, 5f)]
    [SerializeField] private float randomZRange = 0.5f;

    [Header("끝점의 z축 무작위 변화량")]
    [Range(0, 100)]
    [SerializeField] private float endZRange = 1f;

    private SplineContainer splineContainer;
    private Spline spline;
    private float3 startPosition;
    private float3 endPosition;
    
    public void Generate(ref TerrainData[] terrain, int width, int height)
    {
        splineContainer = GetComponent<SplineContainer>();
        spline = splineContainer.Spline;
        startPosition = spline[0].Position;
        endPosition = spline[spline.Count-1].Position;

        RandomizeSpline(width, height);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                var pos = spline.EvaluatePosition((float)(y + 1) / (x + 1));
                // Debug.Log(pos);
                for (int i = 0; i < numKnots; i++)
                {
                    var distanceY = y - (width-spline[i].Position.z);
                    var distanceX = x - spline[i].Position.x;
                    distanceY *= distanceY;
                    distanceX *= distanceX;
                    var sqrDistance = distanceY + distanceX;
                    if (sqrDistance < 25f)
                    {
                        terrain[y * width + x].type = TerrainType.GrassRoad;
                        terrain[y * width + x].noise = 0f;
                        terrain[y * width + x].noise = Mathf.Clamp(terrain[y * width + x].noise, 0f, 1f);
                    }
                }
            }
        }
    }
    private void RandomizeSpline(int width, int height)
    {
        // TODO: Use NativeSpline and Jobs?
        spline.Clear();
        // Modify the starting position
        float3 newStartPosition = startPosition;
        newStartPosition.z = Mathf.Clamp(newStartPosition.z + Random.Range(-startZRange, startZRange), 0f, height);

        // Modify the ending position 
        float3 newEndPosition = endPosition;
        newEndPosition.z = Mathf.Clamp(newEndPosition.z + Random.Range(-endZRange, endZRange), 0f, height);
        
        // Add the starting knot
        BezierKnot newStartKnot = new BezierKnot(newStartPosition);
        spline.Add(newStartKnot);
        
        float3 distance = newEndPosition - newStartPosition;
        float incrementalX = distance.x / (numKnots + 1);
        float incrementalZ = distance.z / (numKnots + 1);
        
        for (int i = 1; i <= numKnots; i++)
        {
            var knot = spline[i - 1];
            var newX = knot.Position.x + incrementalX; // x좌표는 무조건 앞으로
            var newZ = knot.Position.z + incrementalZ + Random.Range(-randomZRange, randomZRange);
            var newPosition = new float3(newX, 0, newZ);
            BezierKnot newKnot = new BezierKnot(newPosition);
            spline.Insert(i, new BezierKnot(newPosition));
        }

        BezierKnot newEndKnot = new BezierKnot(newEndPosition);
        spline.Add(newEndKnot);
    }
    
}
