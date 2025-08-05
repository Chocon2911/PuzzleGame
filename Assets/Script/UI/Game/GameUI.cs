using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static GameUI instance;
    public static GameUI Instance => instance;

    [SerializeField] private Button settingBtn;
    [SerializeField] private TextMeshProUGUI leftMoveStepTxt;
    [SerializeField] private SettingMenu settingMenu;
    [SerializeField] private GameOverMenu gameOverMenu;
    [SerializeField] private WinGameMenu winGameMenu;

    //==========================================Get Set===========================================
    public SettingMenu SettingMenu => this.settingMenu;
    public GameOverMenu GameOverMenu => this.gameOverMenu;
    public WinGameMenu WinGameMenu => this.winGameMenu;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.settingBtn, transform.Find("SettingBtn"), "LoadSettingBtn()");
        this.LoadComponent(ref this.leftMoveStepTxt, transform.Find("LeftMoveStepTxt"), "LoadLeftMoveStepTxt()");
        this.LoadComponent(ref this.settingMenu, transform.Find("SettingMenu"), "LoadSettingMenu()");
        this.LoadComponent(ref this.gameOverMenu, transform.Find("GameOverMenu"), "LoadGameOverMenu()");
        this.LoadComponent(ref this.winGameMenu, transform.Find("WinGameMenu"), "LoadWinGameMenu()");
    }

    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("One GameUI only (this)", gameObject);
            Debug.LogError("One GameUI only (instance)", instance.gameObject);
            return;
        }

        instance = this;
        base.Awake();
    }

    private void Start()
    {
        this.OnLeftMoveStepChanged();
        LevelManager.Instance.OnMoveStepLeftChanged += this.OnLeftMoveStepChanged;
        LevelManager.Instance.OnIncreasePoint += this.OnIncreasePoint;
        this.settingBtn.onClick.AddListener(this.SettingBtnClicked);
    }

    //===========================================Method===========================================
    private void SettingBtnClicked()
    {
        if (this.settingMenu.gameObject.activeSelf)
        {
            this.settingMenu.gameObject.SetActive(false);
            this.ResumeGame();
        }
        else
        {
            this.settingMenu.gameObject.SetActive(true);
            this.PauseGame();
        }
    }

    private void OnLeftMoveStepChanged()
    {
        this.leftMoveStepTxt.text = "Left Move Step: " + LevelManager.Instance.MoveStepLeft.ToString();

        if (LevelManager.Instance.MoveStepLeft > 0) return;
        this.gameOverMenu.gameObject.SetActive(true);
        this.PauseGame();
    }

    private void OnIncreasePoint()
    {
        if (LevelManager.Instance.CurrPoint < LevelManager.Instance.MaxPoint) return;
        this.WinGameMenu.gameObject.SetActive(true);
        this.PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
