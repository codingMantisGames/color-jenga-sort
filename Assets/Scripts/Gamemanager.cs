using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using SupersonicWisdomSDK;

public class Gamemanager : MonoBehaviour
{
    #region VARIABLES
    public static Gamemanager instance;
    [SerializeField] private List<Level> levels;
    [HideInInspector] public Level level;
    public float rotationThreshold = 1.0f;

    [Header("UI - Panels")]
    [SerializeField] private PanelAnimation startPanel;
    [SerializeField] private PanelAnimation inGamePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject restartPanel;
    [SerializeField] private GameObject giftPanel;

    [Header("UI - References")]
    [SerializeField] private TMP_Text lvlLabel;
    [SerializeField] private TMP_Text winTextLabel;
    [SerializeField] private TMP_Text loseTextLabel;
    [SerializeField] private TMP_Text coinsEarnedWinTextLabel;
    [SerializeField] private TMP_Text coinsEarnedLoseTextLabel;
    [SerializeField] private TMP_Text coinsLabel;
    [SerializeField] private GameObject gift;
    [SerializeField] private AudioSource buttonAudio;
    [SerializeField] private GameObject particleFX;

    [Header("UI - Others")]
    [SerializeField] private string[] winMessages;
    [SerializeField] private string[] loseMessages;

    [Header("UndoCoinCounter")]
    [SerializeField] private Button undoButton;
    [SerializeField] private TMP_Text undoCoinCounter;
    private int undoCoinNeeded;

    bool isGameStared = false;
    bool isGameOver = false;
    bool isGameWin = false;
    bool haveGift = false;
    public bool canVibrate;
    bool isSoundOn;
    #endregion

    #region UNITY FUNCTIONS
    private void Awake()
    {
        instance = this;

        Application.targetFrameRate = 60;

        isGameStared = false;

        undoCoinNeeded = 60;
    }
    void Start()
    {
        int lvl = PlayerPrefs.GetInt("Level");
        lvlLabel.text = "Level " + (lvl + 1);

        if (lvl >= levels.Count)
        {
            lvl = Random.Range(2, levels.Count - 1);
        }
        level = levels[lvl];

        level.gameObject.SetActive(true);

        coinsLabel.text = "<sprite=0> " + Coin;

        undoCoinCounter.text = "<sprite=0> " + undoCoinNeeded;

        if (undoCoinNeeded > Coin)
            undoButton.interactable = false;
        else
            undoButton.interactable = true;


    }
    void Update()
    {
        if (!isGameStared)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isGameStared = true;

                StartGame();

                buttonAudio.Play();
                if (canVibrate)
                    Taptic.Light();
            }
        }

        if (isGameOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadSceneAsync(1);

                buttonAudio.Play();
                if (canVibrate)
                    Taptic.Light();
            }
        }

        if (isGameWin)
        {
            if (!haveGift && Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadSceneAsync(1);

                buttonAudio.Play();
                if (canVibrate)
                    Taptic.Light();

            }
            else if (haveGift && Input.GetMouseButtonDown(0))
            {
                winPanel.SetActive(false);

                giftPanel.SetActive(true);

                buttonAudio.Play();
                if (canVibrate)
                    Taptic.Light();
            }
        }
    }
    #endregion

    #region FUNCTIONS
    public void CheckForGameOver(Transform block)
    {
        Debug.LogWarning("Check for gameover!");

        float rotationX = block.eulerAngles.x;
        float rotationZ = block.eulerAngles.z;

        rotationX = NormalizeAngle(rotationX);
        rotationZ = NormalizeAngle(rotationZ);

        if (Mathf.Abs(rotationX) > rotationThreshold || Mathf.Abs(rotationZ) > rotationThreshold)
        {
            Debug.LogWarning("Gameover!");
            GameOver();
        }
    }
    public void CheckForGameWin()
    {
        if (GetActiveChildCount(level.jengaHolder) == 0)
        {
            Debug.LogWarning("Game Win!");

            GameWin();
        }
    }
    bool IsAngleAccepted(float val)
    {
        if (val > 359f && val < 1f)
            return true;
        return false;
    }
    float NormalizeAngle(float val)
    {
        while (val > 180) val -= 360;
        while (val < -180) val += 360;
        return val;
    }
    public void StartGame()
    {
        startPanel.Hide();
        inGamePanel.transform.parent.gameObject.SetActive(true);
        inGamePanel.Show();

        try
        {
            int lvl = PlayerPrefs.GetInt("Level");
            SupersonicWisdom.Api.NotifyLevelStarted(ESwLevelType.Regular, lvl + 1, null);
        }
        catch
        {
            Debug.LogWarning("SDK not initialized!");
        }
    }
    public void Undo()
    {
        float val = Coin;
        Coin -= undoCoinNeeded;
        undoCoinNeeded += 40;
        undoCoinCounter.text = "<sprite=0> " + undoCoinNeeded;


        DOTween.To(() => val, x => val = x, Coin, 1).OnUpdate(() =>
            {
                coinsLabel.text = "<sprite=0> " + (int)val;
            });

        if (undoCoinNeeded > Coin)
            undoButton.interactable = false;
        else
            undoButton.interactable = true;

    }
    public void ShowReloadPanel()
    {
        inGamePanel.transform.parent.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(false);

        restartPanel.SetActive(true);
    }
    public void ReloadGameYes()
    {
        try
        {
            int lvl = PlayerPrefs.GetInt("Level");
            SupersonicWisdom.Api.NotifyLevelFailed(ESwLevelType.Regular, lvl + 1, null);
        }
        catch
        {
            Debug.LogWarning("SDK not initialized!");
        }

        Invoke("LoadNextScene", 0.5f);
    }
    void LoadNextScene()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void ReloadGameNo()
    {
        inGamePanel.transform.parent.gameObject.SetActive(true);
        startPanel.gameObject.SetActive(true);

        restartPanel.SetActive(false);
    }
    public void GameOver()
    {
        if (isGameOver)
            return;

        losePanel.SetActive(true);
        inGamePanel.transform.parent.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(false);

        if (CameraMovementController.instance)
            CameraMovementController.instance.isEnabled = false;

        loseTextLabel.text = loseMessages[Random.Range(0, loseMessages.Length)];

        isGameOver = true;

        coinsEarnedLoseTextLabel.text = "+ <sprite=0> " + level.coinsIfLose;

        Coin += level.coinsIfLose;

        try
        {
            int lvl = PlayerPrefs.GetInt("Level");
            SupersonicWisdom.Api.NotifyLevelFailed(ESwLevelType.Regular, lvl + 1, null);
        }
        catch
        {
            Debug.LogWarning("SDK not initialized!");
        }
    }
    public int GetActiveChildCount(Transform parent)
    {
        int activeChildCount = 0;
        foreach (Transform child in parent)
        {
            if (child.gameObject.activeSelf)
            {
                activeChildCount++;
            }
        }
        Debug.LogWarning(activeChildCount);
        return activeChildCount;
    }
    public void GameWin()
    {
        isGameWin = true;

        particleFX.SetActive(true);

        winPanel.SetActive(true);
        inGamePanel.transform.parent.gameObject.SetActive(false);
        startPanel.gameObject.SetActive(false);

        winTextLabel.text = winMessages[Random.Range(0, winMessages.Length)];

        coinsEarnedWinTextLabel.text = "+ <sprite=0> " + level.coinsIfWin;

        Coin += level.coinsIfWin;

        int lvl = PlayerPrefs.GetInt("Level");
        try
        {
            SupersonicWisdom.Api.NotifyLevelCompleted(ESwLevelType.Regular, lvl + 1, null);
        }
        catch
        {
            Debug.LogWarning("SDK not initialized!");
        }

        PlayerPrefs.SetInt("Level", lvl + 1);
        PlayerPrefs.Save();

    }
    public void RemoveGift()
    {
        haveGift = false;

        Coin += 150;
    }
    public void AddGift()
    {
        haveGift = true;

        gift.SetActive(true);
    }
    public int Coin
    {
        set
        {
            PlayerPrefs.SetInt("Coin", value);
            PlayerPrefs.Save();
        }
        get
        {
            return PlayerPrefs.GetInt("Coin");
        }
    }
    #endregion
}
