using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class LoadingMenuManager : Singleton<LoadingMenuManager>
{
    [SerializeField] GameObject m_loadingScreenObj;
    [SerializeField] private Slider processBar;
    [SerializeField] private TextMeshProUGUI text;
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SwitchToScene(int id)
    {
        m_loadingScreenObj.SetActive(true);
        processBar.value = 0;
        text.text = "0%";
        StartCoroutine(SwitchToSceneAsyc(id));
    }
    public void HideLoadingMenu()
    {
        m_loadingScreenObj.SetActive(false);
    }

    IEnumerator SwitchToSceneAsyc(int id)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(id);
        while(!asyncLoad.isDone)
        {
            processBar.value = asyncLoad.progress;
            text.text = $"{processBar.value * 100f}%";
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        if (id == 1)
        {
            LevelManager.Ins.Init();
        }
        LoadingMenuManager.Ins.HideLoadingMenu();
    }
}
