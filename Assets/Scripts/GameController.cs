using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public int Score { get => _score; set => _score = value; }

    //cursors
    [SerializeField] Texture2D shootCursorTexture;
    [SerializeField] Texture2D menuCursorTexture;

    //screens
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject saveScoreScreen;

    //fields
    [SerializeField] GameObject highScoreInputField;

    //private vars
    private string _leaderboardPath = "Assets/leaderboard.txt";
    private CursorMode _cursorMode = CursorMode.Auto;
    private Vector2 _hotSpot = Vector2.zero;
    private int _score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        Cursor.SetCursor(menuCursorTexture, _hotSpot, _cursorMode);
        Application.targetFrameRate = 60;
        SoundController.Instance.MainMusic.Play();
    }

    public void StartGame()
    {
        _score = 0;
        Cursor.SetCursor(shootCursorTexture, _hotSpot, _cursorMode);
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        Cursor.SetCursor(menuCursorTexture, _hotSpot, _cursorMode);
    }

    public void GoToMainMenu()
    {
        _score = 0;
        SceneManager.LoadScene(0);

    }
    public void ShowSaveScoreScreen()
    {
        gameOverScreen.SetActive(false);
        saveScoreScreen.SetActive(true);
    }
    public void SaveScore()
    {
        WriteHighscoreToLeaderboard();
        GoToMainMenu();
    }

    public void WriteHighscoreToLeaderboard()
    {
        string playerName = highScoreInputField.GetComponent<TMP_InputField>().text;
        File.AppendAllText(_leaderboardPath, string.Format("{0}|{1}\n", playerName, Instance.Score));
    }

    private List<Tuple<string, int>> GetLeaderboardFromFile()
    {
        List<Tuple<string, int>> leaderboard = new List<Tuple<string, int>>();
        string line;
        try
        {
            StreamReader sr = new StreamReader(_leaderboardPath);
            while ((line = sr.ReadLine()) != null)
            {
                string player = line.Substring(0, line.LastIndexOf('|'));
                string score = line.Substring(line.LastIndexOf('|') + 1);
                Tuple<string, int> entry = new Tuple<string, int>(player, int.Parse(score));
                leaderboard.Add(entry);
            }
            sr.Close();
            return leaderboard;
        }
        catch (Exception e)
        {
            Debug.Log("Exception reading file: " + e.Message);
            return leaderboard;
        }
    }



}
