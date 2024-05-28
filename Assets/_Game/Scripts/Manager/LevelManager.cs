using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;
using static Cinemachine.DocumentationSortingAttribute;
using static UnityEngine.Mesh;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private LevelSO levelSO;
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private Transform mapParent;

    private Character player;
    private List<Character> bots = new List<Character>();
    private LevelItemData currentLevel;
    private NavMeshData currentNavMeshData;
    private int alive;
    private int numBot;
    private int remainBot;
    private MapControler currentMapControler;
    //private List<Transform> l_SpawnPos = new List<Transform>();
    private int levelIndex;

    private bool isRevive;

    //public List<Transform> L_SpawnPos { get => l_SpawnPos; set => l_SpawnPos = value; }
    public int Alive { get => alive; set => alive = value; }
    public int NumBot { get => numBot; set => numBot = value; }
    public int RemainBot { get => remainBot; set => remainBot = value; }
    public Character Player { get => player; set => player = value; }

    private void Start()
    {
        //levelIndex = SaveLoadManager.Ins.UserData.CurrentLevel;
        //LoadLevel(levelIndex);
        //UIManager.Ins.OpenUI<MainMenu>();
    }

    public void Init()
    {
        levelIndex = SaveLoadManager.Ins.UserData.CurrentLevel;
        LoadLevel(levelIndex);
        CameraFollowe.Ins.Init();
        UIManager.Ins.OpenUI<MainMenu>();
    }

    private void OnInit()
    {
        //NavMesh.RemoveAllNavMeshData();
        //NavMesh.AddNavMeshData(currentLevel.navMeshData);
        isRevive = false;

        Alive = currentLevel.aliveCount;
        NumBot = currentLevel.botCount;
        remainBot = alive - numBot - 1;
        GeneralMap(currentLevel.map, currentLevel.navMeshData);

        player = SpawnManager.Ins.InitPlayer();
        bots = SpawnManager.Ins.InitSpawn(NumBot);

        SetTargetIndicatorAlpha(0);
    }

    private void GeneralMap(MapControler map, NavMeshData navMeshData)
    {
        currentMapControler = Instantiate(map);
        currentMapControler.transform.SetParent(mapParent, false);

        //navMeshSurface.BuildNavMesh();
        //navMeshSurface.navMeshData = navMeshData;

        //l_SpawnPos = currentMapControler.L_SpawnPos;
    }

    private void RemoveCurrentLevel()
    {
        Destroy(currentMapControler.gameObject);
    }

    [ContextMenu("Win")]
    public void Victory()
    {
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<Win>().SetCoinText(player.Point);
        player.ChangeAnim(Anim.WIN);
    }

    public void Fail(float timeDeley = 0f)
    {
        StartCoroutine(OnFail(timeDeley));
    }

    IEnumerator OnFail(float time)
    {
        yield return new WaitForSeconds(time);
        UIManager.Ins.CloseAll();
        UIManager.Ins.OpenUI<Lose>().SetCoinText(player.Point);
    }


    public void LoadLevel(int level)
    {
        if (currentLevel != null)
        {
            currentLevel = null;
        }
        if (level < levelSO.listLevelItems.Count)
        {
            currentLevel = levelSO.listLevelItems[level];
            this.OnInit();
        }
        else
        {
            //TODO: level vuot qua limit
        }
    }

    public void OnStartGame()
    {
        for (int i = 0; i < bots.Count; i++)
        {
            if (bots[i] is Player) continue;
            bots[i].ChangeState(new PatrolState());
        }
    }

    public void OnFinishGame()
    {
        for (int i = 0; i < bots.Count; i++)
        {
            bots[i].ChangeState(null);
            bots[i].StopMoving();
        }
    }

    public void OnReset()
    {
        //HBPool.CollectAll();
        //HBPool.Despawn(Player);
        for (int i = 0; i < bots.Count; i++)
        {
            HBPool.Despawn(bots[i]);
        }
        bots.Clear();
        //List<GameUnit> l_indicator = HBPool.GetAllUnitIsActive(PoolType.TargetIndicator);
        //for (int i = 0; i< l_indicator.Count ; i++) 
        //{

        //    HBPool.Despawn(l_indicator[i]);
        //}
        HBPool.CollectAll();
        RemoveCurrentLevel();
    }

    internal void OnRetry()
    {
        OnReset();
        LoadLevel(levelIndex);
        UIManager.Ins.OpenUI<MainMenu>();
    }

    public void OnRevive()
    {
        player.TF.position = RandomPoint();
        (player as Player).OnRevive();
    }

    internal void OnNextLevel()
    {
        OnReset();
        levelIndex++;
        SaveLoadManager.Ins.UserData.CurrentLevel = levelIndex;
        SaveLoadManager.Ins.SaveTofile();
        LoadLevel(levelIndex);
        UIManager.Ins.OpenUI<MainMenu>();
    }

    //[MethodImpl(MethodImplOptions.Synchronized)]
    //public Vector3 GetRandomSpawnPos()
    //{
    //    List<Vector3> l_Spawn = new List<Vector3>();
    //    for (int i = 0; i < L_SpawnPos.Count; i++)
    //    {
    //        if (Cache.GetSpawn(L_SpawnPos[i]).IsSpawn())
    //        {
    //            l_Spawn.Add(L_SpawnPos[i].position);
    //        }
    //    }

    //    Vector3 randPoint = Vector3.zero;
        
        
        
    //    float size = Character.ATT_RANGE + Character.MAX_SIZE + 1f;

    //    for (int t = 0; t < 50; t++)
    //    {
    //        if (l_Spawn.Count > 0)
    //            randPoint = l_Spawn[Random.Range(0, l_Spawn.Count)];
    //        else
    //        {
    //            randPoint = L_SpawnPos[Random.Range(0, L_SpawnPos.Count)].position;
    //        }
    //        //randPoint = currentMapControler.RandomPoint();
    //        if (Vector3.Distance(randPoint, player.TF.position) < size)
    //        {
    //            continue;
    //        }

    //        for (int j = 0; j < 20; j++)
    //        {
    //            for (int i = 0; i < bots.Count; i++)
    //            {
    //                if (Vector3.Distance(randPoint, bots[i].TF.position) < size)
    //                {
    //                    break;
    //                }
    //            }

    //            if (j == 19)
    //            {
    //                return randPoint;
    //            }
    //        }
    //    }
    //    return randPoint;
    //}

    public Vector3 RandomPoint()
    {
        Vector3 randPoint = Vector3.zero;

        float size = Character.ATT_RANGE + Character.MAX_SIZE + 1f;

        for (int t = 0; t < 50; t++)
        {

            randPoint = currentMapControler.RandomPoint();
            if (Vector3.Distance(randPoint, player.TF.position) < size)
            {
                continue;
            }

            for (int j = 0; j < 20; j++)
            {
                for (int i = 0; i < bots.Count; i++)
                {
                    if (Vector3.Distance(randPoint, bots[i].TF.position) < size)
                    {
                        break;
                    }
                }

                if (j == 19)
                {
                    return randPoint;
                }
            }


        }

        return randPoint;
    }
    public void HandleCharecterDeath(Character character)
    {
        
        for (int i = 0; i < bots.Count; i++)
        {
            if (bots[i].Equals(character)) continue;
            bots[i].RemoveTarget(character);
        }

        //TODO: Xu ly khi character chet
        if (character is Player)
        {
            UIManager.Ins.CloseAll();

            //revive
            if (!isRevive)
            {
                isRevive = true;
                UIManager.Ins.OpenUI<Revive>();
            }
            else
            {
                Fail(2f);
            }
        }
        else
        if (character is Bot)
        {
            alive--;
            //
            player.RemoveTarget(character);
            bots.Remove(character);

            if (GameManager.Ins.IsState(GameState.Revive) || GameManager.Ins.IsState(GameState.Setting))
            {
                NewBot(Utilities.Chance(50, 100) ? new IdleState() : new PatrolState(),character);
            }
            else
            {
                if (remainBot > 0)
                {
                    remainBot--;
                    NewBot(Utilities.Chance(50, 100) ? new IdleState() : new PatrolState(), character);
                }

                if (bots.Count == 0)
                {
                    Victory();
                }

                UIManager.Ins.GetUI<GamePlay>().SetAliveText(LevelManager.Ins.Alive);
            }

        }

        //UIManager.Ins.GetUI<UIGameplay>().UpdateTotalCharacter();
    }

    public Character NewBot(IState<Character> state,Character chara)
    {
        state = new PatrolState();
        //Character bot = HBPool.Spawn<Bot>(PoolType.Bot, GetRandomSpawnPos(), Quaternion.identity);
        Vector3 newPos = RandomPoint();
        Character bot = HBPool.Spawn<Bot>(PoolType.Bot,newPos, Quaternion.identity);
        bot.CurrentColor = chara.CurrentColor;
        bot.ChangeColorSkin(chara.ColorSkin.material);
        bot.OnInit();
        ParticlePool.Play(ParticleType.Spawn,newPos + Vector3.up,Quaternion.identity);
        bot.ChangeState(state);
        bots.Add(bot);
        bot.SetPoint(player.Point > 0 ? Random.Range(player.Point - 7, player.Point + 7) : 1);

        return bot;
    }

    public void SetTargetIndicatorAlpha(float alpha)
    {
        List<GameUnit> list = HBPool.GetAllUnitIsActive(PoolType.TargetIndicator);

        for (int i = 0; i < list.Count; i++)
        {
            (list[i] as TargetIndicator).SetAlpha(alpha);
        }
    }
}
