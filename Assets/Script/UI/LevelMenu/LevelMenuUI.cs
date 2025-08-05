using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelMenuUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static LevelMenuUI instance;
    public static LevelMenuUI Instance => instance;

    [SerializeField] private Transform levelBtnsContainer;
    [SerializeField] private List<Button> levelBtns = new();
    [SerializeField] private Button backBtn;

    //==========================================Get Set===========================================
    public event Action<int> OnLevelBtnClicked;
    public event Action OnBackBtnClicked;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.levelBtnsContainer, transform.Find("Scroll").Find("Grid"), 
            "LoadLevelButtonsContainer");
        this.LoadComponent(ref this.backBtn, transform.Find("BackBtn"), "LoadBackBtn()");
    }

    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("One LevelMenuUI only (this)", gameObject);
            Debug.LogError("One LevelMenuUI only (instance)", instance.gameObject);
            return;
        }

        instance = this;
        base.Awake();

        GameObject unlockedLevelBtn = Resources.Load<GameObject>("Prefab/LevelBtn");
        GameObject lockedLevelBtn = Resources.Load<GameObject>("Prefab/LockedLevelBtn");
        foreach (LevelData levelData in GameManager.Instance.LevelsData.levels)
        {
            if (levelData.isUnlocked)
            {
                GameObject levelBtn = Instantiate(unlockedLevelBtn, this.levelBtnsContainer);
                Button btn = levelBtn.GetComponent<Button>();
                this.levelBtns.Add(btn);
                btn.onClick.AddListener(() => OnLevelBtnClicked?.Invoke(this.levelBtns.IndexOf(btn)));
                btn.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = (this.levelBtns.IndexOf(btn) +1).ToString();
            }
            else
            {
                GameObject levelBtn = Instantiate(lockedLevelBtn, this.levelBtnsContainer);
                Button btn = levelBtn.GetComponent<Button>();
                btn.onClick.AddListener(() => OnLevelBtnClicked?.Invoke(this.levelBtns.IndexOf(btn)));
                btn.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = (this.levelBtns.IndexOf(btn) + 1).ToString();
            }
        }

        this.backBtn.onClick.AddListener(() => OnBackBtnClicked?.Invoke());
    }
}
