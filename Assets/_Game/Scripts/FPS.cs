

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS : MonoBehaviour
{
    [Header("Frame Seetings")]
    public int TargetFrameRate = 60;

    public float updateInterval = 0.5f;

    float accum = 0;
    int frames = 0;
    float timeLeft = 0;
    float fps = 0;

    GUIStyle textStyle = new GUIStyle();

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = TargetFrameRate;

    }
#if UNITY_EDITOR
    private void Start()
    {
        timeLeft = updateInterval;

        textStyle.fontSize = 60;
        textStyle.fontStyle = FontStyle.Bold;
        textStyle.normal.textColor = Color.white;
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        if (timeLeft <= 0f)
        {
            fps = accum / frames;
            timeLeft = updateInterval;
            accum = 0;
            frames = 0;
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 100, 25), fps.ToString("F2") + " FPS", textStyle);
    }
#endif
}

