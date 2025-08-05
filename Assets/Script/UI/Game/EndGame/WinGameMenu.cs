using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinGameMenu : EndGameMenu
{
    //==========================================Variable==========================================
    [SerializeField] private Button nextLevelBtn;

    //===========================================Event============================================
    public event Action OnNextLevelBtnClicked;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.nextLevelBtn, transform.Find("NextLevelBtn"), "LoadNextLevelbtn");
    }

    private void Start()
    {
        this.nextLevelBtn.onClick.AddListener(this.NextLevelBtnClicked);
    }

    //===========================================Method===========================================
    private void NextLevelBtnClicked()
    {
        this.OnNextLevelBtnClicked?.Invoke();
    }
}
