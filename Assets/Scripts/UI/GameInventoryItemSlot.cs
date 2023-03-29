// 
// Copyright (c) 2023 Kim Hyun Deok
//
// GameInventoryItemSlot.cs
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameInventoryItemSlot : VisualElement
{
    public Image icon;

    public GameInventoryItemSlot()
    {
        icon = new Image();
        Add(icon);

        icon.AddToClassList("slotIcon");
        AddToClassList("slotContainer");
    }
}
