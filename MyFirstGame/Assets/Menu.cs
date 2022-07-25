using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Button playerButton;


    public Text firstScoreText;
    public Text secondScoreText;
    public Text thirdScoreText;
    public Text fourthScoreText;
    public PlayerScores playerScores;

    void SortScores()
    {
        playerScores.scores.Sort();
        playerScores.scores.Reverse();
        //playerScores.scores.ForEach((int i) => print(i));
    }

    void Start()
    {
        playerButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Scenes/SampleScene");
        });



        string scoresPackage = PlayerPrefs.GetString("BestScores", "None");
        playerScores = scoresPackage != "None" ? JsonUtility.FromJson<PlayerScores>(scoresPackage) : new PlayerScores();
        SortScores();
        for (int i = 0; i < playerScores.scores.Count; i++)
        {
            switch (i)
            {
                case 0:
                    firstScoreText.text = "Score: " + playerScores.scores[i].ToString();
                    break;
                case 1:
                    secondScoreText.text = "Score: " + playerScores.scores[i].ToString();
                    break;
                case 2:
                    thirdScoreText.text = "Score: " + playerScores.scores[i].ToString();
                    break;
                case 3:
                    fourthScoreText.text = "Score: " + playerScores.scores[i].ToString();
                    break;
            }
        }
    }
}


[System.Serializable]
public class PlayerScores
{
    public List<int> scores;

    public PlayerScores(List<int> scores = null)
    {
        this.scores = scores != null ? scores : new List<int> { 0, 0, 0, 0, };
    }
}
