using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinWeaponUI : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private Button selectButton;

    public Button SelectButton { get => selectButton; set => selectButton = value; }

    public void OnInit(Material material, RenderTexture rawImage,Action<Material> itemButtonClick)
    {
       this.rawImage.texture = rawImage;
        SelectButton.onClick.AddListener(() =>
        {
            itemButtonClick.Invoke(material);
        });
    }
}
