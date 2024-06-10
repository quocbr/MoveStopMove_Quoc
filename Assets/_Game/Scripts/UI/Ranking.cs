using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Ranking : UICanvas
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button homeButton;

    [SerializeField] private List<ScoreRankingElement> rankingElements;

    private void Awake()
    {
        backButton.onClick.AddListener(OnMyBackButtonClickHandle);
        homeButton.onClick.AddListener(OnMyHomeButtonClickHandle);
    }

    public override void Open()
    {
        base.Open();
        ScoreboardButton();
    }

    public void ScoreboardButton()
    {
        StartCoroutine(LoadScoreboardData());
    }

    private void ResetListScore()
    {
        for(int i = 0; i < rankingElements.Count; i++)
        {
            rankingElements[i].gameObject.SetActive(false);
        }
    }

    private void OnMyHomeButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        UIManager.Ins.OpenUI<MainMenu>();
        Close(0);
    }

    private void OnMyBackButtonClickHandle()
    {
        SoundManager.Ins.PlaySFX(Constant.SFXSound.BUTTON_CLICK);
        UIManager.Ins.OpenUI<MainMenu>();
        Close(0);
    }

    private IEnumerator LoadScoreboardData()
    {
        ResetListScore();
        //Get all the users data ordered by kills amount
        Task<DataSnapshot> DBTask = AuthFirebase.Ins.DBreference.OrderByChild("countKill").LimitToLast(6).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);
        int index = -1;
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            index = 0;
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string email = childSnapshot.Child("email").Value.ToString();
                int kills = int.Parse(childSnapshot.Child("countKill").Value.ToString());
                int currentLevel = int.Parse(childSnapshot.Child("currentLevel").Value.ToString());

                //Instantiate new scoreboard elements
                //GameObject scoreboardElement = Instantiate(scoreElement, scoreboardContent);
                //scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, kills, deaths, xp);
                rankingElements[index].OnInit(email, kills);
                
                index++;
                Debug.Log(index);
            }

            //Go to scoareboard screen
            //UIManager.Ins.ScoreboardScreen();;
            if(index != -1)
            {
                for(int i = 0; i < index; i++)
                {
                    rankingElements[i].gameObject.SetActive(true);
                }
            }
            
        }
    }
}
