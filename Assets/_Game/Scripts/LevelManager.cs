using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField]private Character player;
    [SerializeField]private Character bot;
    [SerializeField]private Transform startPos;
    [SerializeField]private int botCount;

    private Character c_player;
    private List<Character> l_Bot;
    private GameObject currentMap;

    public Character C_player { get => c_player;}
    public int BotCount { get { return botCount; } set {  botCount = value; } }

    private void Start()
    {
        l_Bot = new List<Character>();
    }
    public void GeneralMap()
    {

    }
    [ContextMenu("Test")]
    public void GeneralCharacter()
    {
        c_player = HBPool.Spawn<Player>(PoolType.Player,startPos.position,Quaternion.identity);
        for (int i = 0; i < botCount; i++)
        {
            Bot t_bot = HBPool.Spawn<Bot>(PoolType.Bot, startPos.position + new Vector3(Random.Range(-10,10),0, Random.Range(-10, 10)), Quaternion.identity);
            l_Bot.Add(t_bot);
        }
    }

    public void RemoveCharacter(Character character)
    {
        l_Bot.Remove(character);
        for (int i = 0; i < l_Bot.Count; i++)
        {
            l_Bot[i].RemoveCharacter(character);
        }
    }

    public void spawnMapToIndex(string index)
    {
        
    }

    public void SpawnBot(int countBot)
    {
        
    }

    public void DeleteCurentMap()
    {
        
    }


}
