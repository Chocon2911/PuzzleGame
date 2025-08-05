using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum SceneType
{
    START = 0,
    Level_MENU = 1,
    LEVEL_1 = 2,
    LEVEL_2 = 3,
    LEVEL_3 = 4,
    LEVEL_4 = 5,
    LEVEL_5 = 6,
    LEVEL_6 = 7,
    LEVEL_7 = 8,
    LEVEL_8 = 9,
    LEVEL_9 = 10,
    LEVEL_10 = 11,
    LEVEL_11 = 12,
    LEVEL_12 = 13,
    LEVEL_13 = 14,
    LEVEL_14 = 15,
    LEVEL_15 = 16,
    LEVEL_16 = 17,
    LEVEL_17 = 18,
    LEVEL_18 = 19,
    LEVEL_19 = 20,
    LEVEL_20 = 21
}

public class GameManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static GameManager instance;
    public static GameManager Instance => instance;

    [Header("===Levels Data===")]
    [SerializeField] private LevelsData levelsData;
    [SerializeField] private string levelsDataPath = "LevelsData.json";
    [SerializeField] private int maxLevel;

    //==========================================Get Set===========================================
    public LevelsData LevelsData => this.levelsData;

    //===========================================Unity============================================
    protected override void Awake()
    {
        if (instance != null)
        {
            //Debug.LogError("One GameManager only (this)", gameObject);
            //Debug.LogError("One GameManager only (instance)", instance.gameObject);
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        base.Awake();
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        this.OnStartSceneLoaded();
    }

    //==========================================Support===========================================
    private void LoadScene(SceneType sceneType)
    {
        Time.timeScale = 1f; 
        if (sceneType == SceneType.START) this.LoadSceneWithEvent(sceneType, this.OnStartSceneLoaded);
        else if (sceneType == SceneType.Level_MENU) this.LoadSceneWithEvent(sceneType, this.OnLevelMenuSceneLoaded);
        else if ((int)sceneType >= (int)SceneType.LEVEL_1) this.LoadSceneWithEvent(sceneType, this.OnGameSceneLoaded);
        else SceneManager.LoadScene((int)sceneType);
    }

    private void LoadSceneWithEvent(SceneType sceneType, UnityAction onSceneLoaded)
    {
        UnityAction<Scene, LoadSceneMode> callback = null;

        callback = (scene, mode) =>
        {
            if (scene.buildIndex == (int)sceneType)
            {
                onSceneLoaded?.Invoke();
                SceneManager.sceneLoaded -= callback;
            }
        };

        SceneManager.sceneLoaded += callback;
        SceneManager.LoadScene((int)sceneType);
    }

    //========================================Levels Data=========================================
    private void LoadLevelsData()
    {
        string path = Path.Combine(Application.persistentDataPath, levelsDataPath);
        if (!File.Exists(path))
        {
            LevelData firstLevelData = new(0, true);
            this.levelsData.levels.Add(firstLevelData);
            for (int i = 1; i < maxLevel; i++)
            {
                LevelData levelData = new(i + 1, false);
                this.levelsData.levels.Add(levelData);
            }
            string json = JsonUtility.ToJson(levelsData, true);
            Directory.CreateDirectory(Application.persistentDataPath);
            File.WriteAllText(path, json);
            Debug.Log("Created new LevelsData.json");
        }
        else
        {
            string json = File.ReadAllText(path);
            levelsData = JsonUtility.FromJson<LevelsData>(json);
            Debug.Log("Loaded from LevelsData.json");
        }
    }

    private void SaveLevelsData()
    {
        string json = JsonUtility.ToJson(levelsData, true);
        string path = Path.Combine(Application.persistentDataPath, this.levelsDataPath);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllText(path, json);
        Debug.Log("Saved to LevelsData.json");
    }

    //========================================Start Scene=========================================
    private void OnStartSceneLoaded()
    {
        this.LoadLevelsData();
        StartUI.Instance.OnPlayBtnClicked += this.OnPlayBtnClicked;
        StartUI.Instance.OnQuitBtnClicked += this.OnQuitBtnClicked;
    }

    private void OnPlayBtnClicked()
    {
        this.LoadScene(SceneType.Level_MENU);
    }

    private void OnQuitBtnClicked()
    {
        this.SaveLevelsData();
        Application.Quit();
    }

    //======================================Level Menu Scene======================================
    private void OnLevelMenuSceneLoaded()
    {
        LevelMenuUI.Instance.OnBackBtnClicked += this.OnBackToStartBtnClicked;
        LevelMenuUI.Instance.OnLevelBtnClicked += this.OnLevelBtnClicked;
    }

    private void OnBackToStartBtnClicked()
    {
        this.LoadScene(SceneType.START);
    }

    private void OnLevelBtnClicked(int levelIndex)
    {
        Debug.Log(levelIndex);
        if (!this.levelsData.levels[levelIndex].isUnlocked) return;
        this.LoadScene(SceneType.LEVEL_1 + levelIndex);
    }

    //=========================================Game Scene=========================================
    private void OnGameSceneLoaded()
    {
        //===Setting Menu===
        GameUI.Instance.SettingMenu.OnLevelMenuBtnClicked += this.OnPlayBtnClicked;
        GameUI.Instance.SettingMenu.OnTryAgainBtnClicked += this.OnTryAgainBtnClicked;

        //===Game Over Menu===
        GameUI.Instance.GameOverMenu.OnLevelMenuBtnClicked += this.OnPlayBtnClicked;
        GameUI.Instance.GameOverMenu.OnTryAgainBtnClicked += this.OnTryAgainBtnClicked;

        //===Win Game Menu===
        GameUI.Instance.WinGameMenu.OnLevelMenuBtnClicked += this.OnPlayBtnClicked;
        GameUI.Instance.WinGameMenu.OnTryAgainBtnClicked += this.OnTryAgainBtnClicked;
        GameUI.Instance.WinGameMenu.OnNextLevelBtnClicked += this.OnNextLevelBtnClicked;
    }

    private void OnTryAgainBtnClicked()
    {
        this.LoadScene(SceneType.LEVEL_1 + LevelManager.Instance.CurrLevelIndex - 1);
    }

    private void OnNextLevelBtnClicked()
    {
        this.levelsData.levels[LevelManager.Instance.CurrLevelIndex].isUnlocked = true;
        this.SaveLevelsData();
        int nextLevelIndex = (int)SceneType.LEVEL_1 + LevelManager.Instance.CurrLevelIndex;
        if (nextLevelIndex > (int)SceneType.LEVEL_20) this.LoadScene(SceneType.LEVEL_1);
        else this.LoadScene(SceneType.LEVEL_1 + LevelManager.Instance.CurrLevelIndex);
    }
}
