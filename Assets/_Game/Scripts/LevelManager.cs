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
    [SerializeField] private OffScreenIndicator offScreenIndicator;

    private Player player;
    private List<Character> characters = new List<Character>();

    private LevelItemData currentLevel;
    private NavMeshData currentNavMeshData;
    private int alive;
    private int numBot;
    private int remainBot;

    private MapControler currentMapControler;
    private List<Transform> l_SpawnPos = new List<Transform>();

    private int levelIndex;

    public List<Transform> L_SpawnPos { get => l_SpawnPos; set => l_SpawnPos = value; }
    public int Alive { get => alive; set => alive = value; }
    public int NumBot { get => numBot; set => numBot = value; }
    public int RemainBot { get => remainBot; set => remainBot = value; }
    public Player Player { get => player; set => player = value; }
    public OffScreenIndicator OffScreenIndicator { get => offScreenIndicator; set => offScreenIndicator = value; }

    private void Start()
    {
        levelIndex = SaveLoadManager.Ins.UserData.CurrentLevel;
        LoadLevel(levelIndex);
        OffScreenIndicator.gameObject.SetActive(false);
        UIManager.Ins.OpenUI<MainMenu>();
    }

    public void OnInit()
    {
        //NavMesh.RemoveAllNavMeshData();
        //NavMesh.AddNavMeshData(currentLevel.navMeshData);

        Alive = currentLevel.aliveCount;
        NumBot = currentLevel.botCount;
        remainBot = alive - numBot - 1;
        GeneralMap(currentLevel.map, currentLevel.navMeshData);
        characters = SpawnManager.Ins.InitSpawn(NumBot);        
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
        offScreenIndicator.gameObject.SetActive(true);
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i] is Player) continue;
            characters[i].ChangeState(new PatrolState());
        }
    }

    public void OnFinishGame()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            characters[i].ChangeState(null);
            characters[i].StopMoving();
        }
        offScreenIndicator.gameObject.SetActive(false);
    }

    public void OnReset()
    {
        HBPool.CollectAll();
        characters.Clear();
        RemoveCurrentLevel();
        offScreenIndicator.gameObject.SetActive(false);
    }

    internal void OnRetry()
    {
        OnReset();
        LoadLevel(levelIndex);
        UIManager.Ins.OpenUI<MainMenu>();
    }

    [ContextMenu("Next")]
    internal void OnNextLevel()
    {
        OnReset();
        levelIndex++;
        SaveLoadManager.Ins.UserData.CurrentLevel = levelIndex;
        SaveLoadManager.Ins.Save();
        LoadLevel(levelIndex);
        UIManager.Ins.OpenUI<MainMenu>();
    }

    private void GeneralMap(MapControler map, NavMeshData navMeshData)
    {
        currentMapControler = Instantiate(map);
        currentMapControler.transform.SetParent(mapParent,false);
        
        navMeshSurface.BuildNavMesh();
        //navMeshSurface.navMeshData = navMeshData;

        l_SpawnPos = currentMapControler.L_SpawnPos;
    }


    public void RemoveCurrentLevel()
    {
        Destroy(currentMapControler.gameObject);
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public Vector3 GetRandomSpawnPos()
    {
        List<Vector3> l_Spawn = new List<Vector3>();
        for (int i = 0; i < L_SpawnPos.Count; i++)
        {
            if (Cache.GetSpawn(L_SpawnPos[i]).IsSpawn())
            {
                l_Spawn.Add(L_SpawnPos[i].position);
            }
        }

        if (l_Spawn.Count > 0)
            return l_Spawn[Random.Range(0, l_Spawn.Count)];
        else
        {
            return L_SpawnPos[Random.Range(0, L_SpawnPos.Count)].position;
        }
    }

    public void HandleCharacterDead(Character character)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].Equals(character)) continue;
            characters[i].RemoveTarget(character);
        }
    }
}
