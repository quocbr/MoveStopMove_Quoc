using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelItemUI : MonoBehaviour
{
    [SerializeField] private Image imageLevel;
    [SerializeField] private TextMeshProUGUI textIndex;
    [SerializeField] private Button levelButton;

    public void Init(Sprite sprite, string textIndex, int botCount, Action<string,int> levelButtonClick)
    {
        imageLevel.sprite = sprite;
        this.textIndex.text = textIndex;
        levelButton.onClick.AddListener(() =>
        {
            levelButtonClick.Invoke(textIndex,botCount);
        });
    }
}
