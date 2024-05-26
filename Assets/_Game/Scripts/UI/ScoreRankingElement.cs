using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreRankingElement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI emailText;
    [SerializeField] private TextMeshProUGUI killText;

    public void OnInit(string email, int kill)
    {
        emailText.text = email;
        killText.text = $"{kill}";
    }
}
