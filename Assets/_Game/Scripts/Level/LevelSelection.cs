using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    [SerializeField] private LevelItemUI buttonPrefab;
    [SerializeField] private Transform parentPosition;
    [SerializeField] private LevelSO levelDataSO;

    private void Start()
    {
        SpawmLevelList();
    }

    private void SpawnLevelItem(Sprite sprite, string textIndex, int botCount)
    {
        LevelItemUI levelItemUI = Instantiate(buttonPrefab, parentPosition);
        levelItemUI.Init(sprite, textIndex, botCount,OnLevelItemUIClickHandle);
    }
    
    private void SpawnLevelItem(LevelItemData levelItemData)
    {
        LevelItemUI levelItemUI = Instantiate(buttonPrefab, parentPosition);
        levelItemUI.Init(levelItemData.sprite, levelItemData.levelIndex.ToString(), levelItemData.botCount,OnLevelItemUIClickHandle);
    }

    private void OnLevelItemUIClickHandle(string index, int botCount)
    {
        LevelManager.Ins.BotCount = botCount;
        LevelManager.Ins.spawnMapToIndex(index);
        LevelManager.Ins.SpawnBot(botCount);
        
    }

    private void SpawmLevelList()
    {
        List<LevelItemData> list = levelDataSO.listLevelItems;
        for (int i = 0; i < list.Count; i++)
        {
            //SpawnLevelItem(list[i].sprite, list[i].levelIndex.ToString(), list[i].botCount);

            SpawnLevelItem(list[i]);
        }
    }

}
