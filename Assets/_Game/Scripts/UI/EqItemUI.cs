using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EqItemUI : GameUnit
{
    [SerializeField] private RawImage imageItem;
    [SerializeField] private Button selectButton;

    public Button SelectButton { get => selectButton; set => selectButton = value; }

    public void OnInit(EquipmentData equipmentData, Action<EquipmentData> itemButtonClick)
    {
        this.imageItem.texture = equipmentData.image;
        SelectButton.onClick.AddListener(() =>
        {
            itemButtonClick.Invoke(equipmentData);
        });
    }
}
