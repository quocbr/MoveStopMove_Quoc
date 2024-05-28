using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnManager : Singleton<SpawnManager>
{
    [SerializeField] private ColorSO colorSO;
    public Dictionary<string,bool> dicColor = new Dictionary<string, bool>();
    public Material playerColor;

    private Coroutine coroutine;

    private void Awake()
    {
        playerColor = colorSO.colorList[0];
        for (int i = 1; i < colorSO.colorList.Count; i++)
        {
            dicColor.Add(colorSO.colorList[i].name,false);
        }
    }

    public Character InitPlayer()
    {
        Player player = HBPool.Spawn<Player>(PoolType.Player, Vector3.zero, Quaternion.identity);

        //int x = System.Enum.GetValues(typeof(ColorType)).Length - 1;
        player.Point = 0;
        player.NameChar = SaveLoadManager.Ins.UserData.UserName;
        player.OnInit();
        //player.CurrentColor = (ColorType)(x - 1);
        //SaveLoadManager.Ins.UserData.currentColor = colorSO.colorList[x - 1];
        //player.ChangeColorSkin(SaveLoadManager.Ins.UserData.currentColor);

        //player.ChangeWeapon(SaveLoadManager.Ins.UserData.CurrentWeapon);

        //if (SaveLoadManager.Ins.UserData.CurrentSet != SetType.None)
        //{
        //    player.ChangeSet(SaveLoadManager.Ins.UserData.CurrentSet);
        //}
        //else
        //{
        //    player.ChangeHead(SaveLoadManager.Ins.UserData.CurrentHead);
        //    player.ChangePant(SaveLoadManager.Ins.UserData.CurrentPant);
        //    player.ChangeTail(SaveLoadManager.Ins.UserData.CurrentTail);
        //    player.ChangeShield(SaveLoadManager.Ins.UserData.CurrentShield);
        //    player.ChangeWing(SaveLoadManager.Ins.UserData.CurrentWing);
        //}
        return player;
    }

    public List<Character> InitSpawn(int numBot)
    {
        List<Character> characters = new List<Character>();
        //Player player = HBPool.Spawn<Player>(PoolType.Player, Vector3.zero, Quaternion.identity);
        //characters.Add(player);


        int x = System.Enum.GetValues(typeof(ColorType)).Length - 1;
        //player.OnInit();
        //player.Point = 0;
        //player.NameChar = SaveLoadManager.Ins.UserData.UserName;
        //player.CurrentColor =(ColorType)(x - 1);
        //SaveLoadManager.Ins.UserData.currentColor = colorSO.colorList[x - 1];
        //player.ChangeColorSkin(SaveLoadManager.Ins.UserData.currentColor);

        //player.ChangeWeapon(SaveLoadManager.Ins.UserData.CurrentWeapon);

        //if(SaveLoadManager.Ins.UserData.CurrentSet != SetType.None)
        //{
        //    player.ChangeSet(SaveLoadManager.Ins.UserData.CurrentSet);
        //}
        //else
        //{
        //    player.ChangeHead(SaveLoadManager.Ins.UserData.CurrentHead);
        //    player.ChangePant(SaveLoadManager.Ins.UserData.CurrentPant);
        //    player.ChangeTail(SaveLoadManager.Ins.UserData.CurrentTail);
        //    player.ChangeShield(SaveLoadManager.Ins.UserData.CurrentShield);
        //    player.ChangeWing(SaveLoadManager.Ins.UserData.CurrentWing);
        //}
        

        //LevelManager.Ins.Player = player;
        for (int i = 0; i < numBot; i++)
        {

            //if (i >= LevelManager.Ins.L_SpawnPos.Count)
            //{
            //    break;
            //}
            if (i >= colorSO.colorList.Count - 1)
            {
                break;
            }

            //Bot bot = HBPool.Spawn<Bot>(PoolType.Bot, LevelManager.Ins.L_SpawnPos[i].position,Quaternion.identity);
            Bot bot = HBPool.Spawn<Bot>(PoolType.Bot, LevelManager.Ins.RandomPoint(),Quaternion.identity);
            bot.CurrentColor = (ColorType)(i + 1);
            bot.ChangeColorSkin(colorSO.colorList[i + 1]);
            //bot.NameChar = "Bot " + i;
            //bot.ChangeWeapon(EquipmentController.Ins.GetWeapon());

            //int randomeq = Random.Range(1, 101);

            //if(randomeq <= 3)
            //{
            //    bot.ChangeSet(EquipmentController.Ins.GetSet());
            //}
            //else
            //{

            //    bot.ChangeColorSkin(colorSO.colorList[i]);
            //    if (Random.Range(1, 101) > 80) bot.ChangeHead(EquipmentController.Ins.GetHead());
            //    if (Random.Range(1, 101) > 60) bot.ChangePant(EquipmentController.Ins.GetPant());
            //    if (Random.Range(1, 101) > 95) bot.ChangeTail(EquipmentController.Ins.GetTail());
            //    if (Random.Range(1, 101) > 90) bot.ChangeShield(EquipmentController.Ins.GetShield());
            //    if (Random.Range(1, 101) > 95) bot.ChangeWing(EquipmentController.Ins.GetWing());
            //}


            //bot.Point = 0;
            bot.OnInit();

            characters.Add(bot);
        }

        HanldeSpawnGiftBox();

        return characters;
    }
    public Material GetColorSkin(ColorType colorType)
    {
        return colorSO.colorList[(int)colorType];
    }

    public Material GetColorSkinSet(SetType setType)
    {
        return colorSO.colorSet[(int)setType - 1];
    }

    public void HanldeSpawnGiftBox()
    {
       coroutine = StartCoroutine(SpawnGiftBox(Utilities.RandIntNumber(5,10)));
    }

    IEnumerator SpawnGiftBox(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        HBPool.Spawn<GameUnit>(PoolType.Gift_Box,LevelManager.Ins.RandomPoint(),Quaternion.identity);
        StopCoroutine(coroutine);
    }

    //public Tuple<Material,int> GetColor()
    //{
    //    List<Material> list = colorSO.colorList;
    //    for(int i = 1;i<list.Count;i++)
    //    {
    //        if (!dicColor[list[i].name]){
    //            dicColor[list[i].name] = true;
    //            return new Tuple<Material, int>(list[i],i);
    //        }
    //    }
    //    return null;
    //}

    //public void BackColor(Material material)
    //{
    //    dicColor[material.name] = false;
    //}
}
