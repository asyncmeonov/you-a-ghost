using System.Collections.Generic;
using System.IO;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }
    public int Score { get => _score; set => _score = value; }

    //audio
    [SerializeField] AudioMixer _mixer;

    //cursors
    [SerializeField] Texture2D shootCursorTexture;
    [SerializeField] Texture2D menuCursorTexture;

    //screens in game
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject saveScoreScreen;

    //screens in main menu
    [SerializeField] GameObject mainMenuScreen;
    [SerializeField] GameObject settingsScreen;
    [SerializeField] GameObject leaderboardScreen;

    //fields
    [SerializeField] GameObject[] currentGameScoreDisplayFields;
    [SerializeField] GameObject highScoreInputField;
    [SerializeField] GameObject highScoreDisplayField;

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
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1: Cursor.SetCursor(shootCursorTexture, _hotSpot, _cursorMode); break;
            default: Cursor.SetCursor(menuCursorTexture, _hotSpot, _cursorMode); break;
        }
        Application.targetFrameRate = 60;
        SoundController.Instance.MainMusic.Play();
    }

    public void StartGame()
    {
        _score = 0;
        SceneManager.LoadScene(1);
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        Cursor.SetCursor(menuCursorTexture, _hotSpot, _cursorMode);
        foreach (var field in currentGameScoreDisplayFields)
        {
            field.GetComponent<TextMeshProUGUI>().text = Score.ToString().PadLeft(4, '0');
        }
    }

    public void GoToMainMenu()
    {

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            CloseAllInMainMenu();
            mainMenuScreen.SetActive(true);
        }
        else
        {
            _score = 0;
            SceneManager.LoadScene(0);
        }


    }

    public void ShowSettingsMenu()
    {
        CloseAllInMainMenu();
        settingsScreen.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowLeaderboardScreen()
    {
        CloseAllInMainMenu();
        TextMeshProUGUI leaderboardList = highScoreDisplayField.GetComponent<TextMeshProUGUI>();
        List<Tuple<string, int>> leaderboard = GetLeaderboardFromFile();
        List<Tuple<string, int>> topTen = leaderboard.OrderBy(s => s.Item2).Reverse().Take(10).ToList();

        leaderboardList.text = "";

        foreach (var item in topTen)
        {
            string row = item.Item2.ToString().PadLeft(4, '0') + " - " + item.Item1 + "\n";
            leaderboardList.text += row;
        }

        leaderboardScreen.SetActive(true);
    }

    public void ShowSaveScoreScreen()
    {
        CloseAllInGame();
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

    public void SetMusicVolume(float value)
    {
        _mixer.SetFloat("MusicVol", Mathf.Log10(value) * 20);
    }

    public void SetSoundEffectsVolume(float value)
    {
        _mixer.SetFloat("SFXVol", Mathf.Log10(value) * 20);
        SoundController.Instance.PlayerGunShot.Play();
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

    private void CloseAllInMainMenu()
    {
        mainMenuScreen.SetActive(false);
        settingsScreen.SetActive(false);
        leaderboardScreen?.SetActive(false);
    }

    private void CloseAllInGame()
    {
        gameOverScreen?.SetActive(false);
        saveScoreScreen?.SetActive(false);
    }



}
