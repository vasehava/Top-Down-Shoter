using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TopScoreTable : MonoBehaviour
{
    public List<Text> topScoreListUI;
    private List<int> topScoreList;
	// Use this for initialization
	void Start () {
        topScoreList = new List<int>();

		if(!PlayerPrefs.HasKey("TopScoreTable"))
        {
            PlayerPrefs.SetString("TopScoreTable", "0,0,0,0,0,0,0");
            PlayerPrefs.Save();
        }

        string[] topScore = PlayerPrefs.GetString("TopScoreTable").Split(',');
        LoadScoreTable(topScore);
        for (int i = 0; i < 5; i++)
            topScoreListUI[i].text = (i + 1).ToString() + ". " + topScoreList[i].ToString();

    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.D))
            PlayerPrefs.DeleteKey("TopScoreTable");
	}

    public void AddScore(string score)
    {
        Debug.Log("add score: " + score);
        string[] strs = PlayerPrefs.GetString("TopScoreTable").Split(',');
        foreach(string s in strs)
        {
            if(s == score)
            {
                string[] _topScore = PlayerPrefs.GetString("TopScoreTable").Split(',');
                LoadScoreTable(_topScore);
                return;
            }
        }
        PlayerPrefs.SetString("TopScoreTable", PlayerPrefs.GetString("TopScoreTable") + "," + score);
        PlayerPrefs.Save();
        //string[] topScore = PlayerPrefs.GetString("TopScoreTable").Split(',');
        //LoadScoreTable(topScore);

    }
    public void OnDeath(int score)
    {
        topScoreList = new List<int>();
        string[] topScore = PlayerPrefs.GetString("TopScoreTable").Split(',');
        LoadScoreTable(topScore);
        Debug.Log(topScoreList.Count + " count");
        int numberInTop = topScoreList.IndexOf(score) + 1;
        topScoreListUI[5].gameObject.SetActive(true);
        topScoreListUI[6].text = numberInTop.ToString() + ". " + score.ToString();
        topScoreListUI[6].gameObject.SetActive(true);
    }
    private void LoadScoreTable(string[] table)
    {
        topScoreList = new List<int>();
        foreach (string s in table)
        {
            int result = 0;
            int.TryParse(s, out result);
            topScoreList.Add(result);
        }
        topScoreList.Sort();
        topScoreList.Reverse();
    }
}
