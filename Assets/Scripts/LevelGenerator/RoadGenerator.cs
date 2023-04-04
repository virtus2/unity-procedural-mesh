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
    
    public void Generate(ref float[] noiseMap, int width, int height)
    {
        splineContainer = GetComponent<SplineContainer>();
        spline = splineContainer.Spline;
        startPosition = spline[0].Position;
        endPosition = spline[spline.Count-1].Position;

        RandomizeSpline();
        
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float amount = 0;
                for (int i = 0; i < spline.Count; i++)
                {
                    float distanceX = Mathf.Abs(x - spline[i].Position.x);
                    float distanceY = Mathf.Abs(y - spline[i].Position.z);
                    if (distanceX < 0.3f && distanceY < 0.3f)
                    {
                        amount += 0.05f;
                        noiseMap[y * width + x] = -1f;
                    }
                }

            }
        }
    }
    private void RandomizeSpline()
    {
        // TODO: Use NativeSpline and Jobs?
        spline.Clear();
        // Modify the starting position
        float3 newStartPosition = startPosition;
        newStartPosition.z = Mathf.Clamp(newStartPosition.z + Random.Range(-startZRange, startZRange), 0f, Constants.ChunkSize);

        // Modify the ending position 
        float3 newEndPosition = endPosition;
        newEndPosition.z = Mathf.Clamp(newEndPosition.z + Random.Range(-endZRange, endZRange), 0f, Constants.ChunkSize);
        
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
