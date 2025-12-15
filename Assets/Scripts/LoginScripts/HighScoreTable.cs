using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewBehaviourScript : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate; 
    private List<Transform> highscoreEntryTransformList;
    private void Awake()
    {
        entryContainer = transform.Find("HighScoreEntryContainer");
        entryTemplate = entryContainer.Find("HighScoreTemplate");
        entryTemplate.gameObject.SetActive(false);
        PlayerPrefs.DeleteKey("highscoreTable");
        AddHighscoreEntry(1000, "AD");
        GetLeaderBoard();
        
    }
    private void CreateHighScoreEntryTransform(HighscoreEntry highscoreEntry,Transform container,List<Transform> transformList)
    {
        float templateHeight = 110f;
        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();
        entryRectTransform.anchoredPosition = new Vector2(0f, -templateHeight * transformList.Count);
        entryTransform.gameObject.SetActive(true);
        int rank = transformList.Count + 1;
        string rankString;

        switch (rank)
        {
            default: rankString = rank + "TH"; break;
            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;

        }
        entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().text = rankString;
        int score = highscoreEntry.score;
        entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = score.ToString();
        string name = highscoreEntry.name;
        entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().text = name;
        transformList.Add(entryTransform);
        entryRectTransform.GetComponent<Image>().enabled = rank % 2 == 1;
        if(rank == 1)
        {
            entryTransform.Find("nameText").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("ScoreText").GetComponent<TextMeshProUGUI>().color = Color.green;
            entryTransform.Find("posText").GetComponent<TextMeshProUGUI>().color = Color.green;
        }
    }
    private void AddHighscoreEntry(int score, string name)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score , name = name};
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        if (string.IsNullOrEmpty(jsonString))
        {
            highscores = new Highscores { highscoreEntryList = new List<HighscoreEntry>()};
        }
        highscores.highscoreEntryList.Add(highscoreEntry);
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save(); 
    }
    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }
    [System.Serializable]
    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }
    private Highscores GetHighscore()
    {
        string json = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(json);
        return highscores;
    }
    private void GetLeaderBoard()
    {
        Highscores highscores = GetHighscore();
        highscores.highscoreEntryList.Sort((a, b) => b.score.CompareTo(a.score));
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscores.highscoreEntryList)
        {
            CreateHighScoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
    }
}
