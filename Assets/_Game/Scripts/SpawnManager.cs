using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private ColorSO colorSO;

    public List<Character> InitSpawn(int numBot)
    {
        List<Character> characters = new List<Character>();
        Player player = HBPool.Spawn<Player>(PoolType.Player, Vector3.zero, Quaternion.identity);
        characters.Add(player);


        int x = System.Enum.GetValues(typeof(ColorType)).Length - 1;
        player.OnInit();
        player.Point = 0;
        player.NameChar = SaveLoadManager.Ins.UserData.UserName;
        player.CurrentColor =(ColorType)(x - 1);

        player.ChangeColorSkin(colorSO.colorList[x-1]);

        player.ChangeWeapon(SaveLoadManager.Ins.UserData.CurrentWeapon);

        if(SaveLoadManager.Ins.UserData.CurrentSet != SetType.None)
        {
            player.ChangeSet(SaveLoadManager.Ins.UserData.CurrentSet);
        }
        else
        {
            player.ChangeHead(SaveLoadManager.Ins.UserData.CurrentHead);
            player.ChangePant(SaveLoadManager.Ins.UserData.CurrentPant);
            player.ChangeTail(SaveLoadManager.Ins.UserData.CurrentTail);
            player.ChangeShield(SaveLoadManager.Ins.UserData.CurrentShield);
            player.ChangeWing(SaveLoadManager.Ins.UserData.CurrentWing);
        }
        

        LevelManager.Ins.Player = player;
        for (int i = 0; i < numBot; i++)
        {

            if (i >= LevelManager.Ins.L_SpawnPos.Count)
            {
                break;
            }
            
            Bot bot = HBPool.Spawn<Bot>(PoolType.Bot, LevelManager.Ins.L_SpawnPos[i].position,Quaternion.identity);
            bot.CurrentColor = (ColorType)(i);
            bot.NameChar = "Bot " + i;
            bot.ChangeWeapon(EquipmentController.Ins.GetWeapon());

            int randomeq = Random.Range(1, 101);

            if(randomeq <= 3)
            {
                bot.ChangeSet(EquipmentController.Ins.GetSet());
            }
            else
            {

                bot.ChangeColorSkin(colorSO.colorList[i]);
                if (Random.Range(1, 101) > 80) bot.ChangeHead(EquipmentController.Ins.GetHead());
                if (Random.Range(1, 101) > 60) bot.ChangePant(EquipmentController.Ins.GetPant());
                if (Random.Range(1, 101) > 95) bot.ChangeTail(EquipmentController.Ins.GetTail());
                if (Random.Range(1, 101) > 90) bot.ChangeShield(EquipmentController.Ins.GetShield());
                if (Random.Range(1, 101) > 95) bot.ChangeWing(EquipmentController.Ins.GetWing());
            }

            
            bot.Point = 0;
            bot.OnInit();

            characters.Add(bot);
        }
        return characters;
    }

    public Material GetColorSkinSet(SetType setType)
    {
        return colorSO.colorSet[(int)setType - 1];
    }
}
