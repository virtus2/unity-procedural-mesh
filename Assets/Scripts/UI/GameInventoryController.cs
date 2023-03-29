// 
// Copyright (c) 2023 Kim Hyun Deok
//
// GameInventoryController.cs
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameInventoryController : MonoBehaviour
{
    private VisualElement root;
    private VisualElement slotContainer;
    private int itemSlotCount = 100;
    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        slotContainer = root.Query<VisualElement>("SlotContainer");

        for (int i = 0; i < itemSlotCount; i++)
        {
            GameInventoryItemSlot itemSlot = new GameInventoryItemSlot();

            slotContainer.Add(itemSlot);
        }
    }
}
