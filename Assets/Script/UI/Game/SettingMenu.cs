using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] private Button resumeBtn;
    [SerializeField] private Button tryAgainBtn;
    [SerializeField] private Button levelMenuBtn;

    //==========================================Get Set===========================================
    public event Action OnTryAgainBtnClicked;
    public event Action OnLevelMenuBtnClicked;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.resumeBtn, transform.Find("ResumeBtn"), "LoadResumeBtn");
        this.LoadComponent(ref this.tryAgainBtn, transform.Find("TryAgainBtn"), "LoadTryAgainBtn");
        this.LoadComponent(ref this.levelMenuBtn, transform.Find("LevelMenuBtn"), "LoadLevelMenuBtn");
    }

    private void Start()
    {
        this.resumeBtn.onClick.AddListener(this.ResumeBtnClicked);
        this.tryAgainBtn.onClick.AddListener(this.TryAgainBtnClicked);
        this.levelMenuBtn.onClick.AddListener(this.LevelMenuBtnClicked);
    }

    //===========================================Method===========================================
    private void ResumeBtnClicked()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    private void TryAgainBtnClicked()
    {
        this.OnTryAgainBtnClicked?.Invoke();
    }

    private void LevelMenuBtnClicked()
    {
        this.OnLevelMenuBtnClicked?.Invoke();
    }
}
