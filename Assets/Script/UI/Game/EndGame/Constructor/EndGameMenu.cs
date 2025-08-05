using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EndGameMenu : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] private Button levelMenuBtn;
    [SerializeField] private Button tryAgainBtn;

    //===========================================Event============================================
    public event Action OnLevelMenuBtnClicked;
    public event Action OnTryAgainBtnClicked;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.levelMenuBtn, transform.Find("LevelMenuBtn"), "LoadLevelMenuBtn");
        this.LoadComponent(ref this.tryAgainBtn, transform.Find("TryAgainBtn"), "LoadTryAgainBtn");
    }

    protected override void Awake()
    {
        base.Awake();
        this.levelMenuBtn.onClick.AddListener(this.LevelMenuBtnClicked);
        this.tryAgainBtn.onClick.AddListener(this.TryAgainBtnClicked);
    }

    //===========================================Method===========================================
    private void LevelMenuBtnClicked()
    {
        this.OnLevelMenuBtnClicked?.Invoke();
    }

    private void TryAgainBtnClicked()
    {
        this.OnTryAgainBtnClicked?.Invoke();
    }
}
