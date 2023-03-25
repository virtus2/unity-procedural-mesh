// 
// Copyright (c) 2023 Kim Hyun Deok
//
// YieldInstructionCache.cs
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal static class YieldInstructionCache
{
    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();
    private static Dictionary<float, WaitForSeconds> _WaitForSeconds;

    public static WaitForSeconds WaitForSeconds(float seconds)
    {
        if (_WaitForSeconds.TryGetValue(seconds, out var waitForSeconds))
        {
            _WaitForSeconds.Add(seconds, waitForSeconds = new WaitForSeconds(seconds));
        }
        return waitForSeconds;
    }
}
