using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static StartUI instance;
    public static StartUI Instance => instance;

    [SerializeField] private Button playBtn;
    [SerializeField] private Button quitBtn;

    //==========================================Get Set===========================================
    public event Action OnPlayBtnClicked;
    public event Action OnQuitBtnClicked;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.playBtn, transform.Find("PlayBtn"), "LoadPlayBtn");
        this.LoadComponent(ref this.quitBtn, transform.Find("QuitBtn"), "LoadQuitBtn");
    }

    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("One StartUI only (this)", gameObject);
            Debug.LogError("One StartUI only (instance)", instance.gameObject);
            return;
        }

        instance = this;
        base.Awake();
    }

    private void Start()
    {
        this.playBtn.onClick.AddListener(this.PlayBtnClicked);
        this.quitBtn.onClick.AddListener(this.QuitBtnClicked);
    }

    //===========================================Method===========================================
    private void PlayBtnClicked()
    {
        this.OnPlayBtnClicked?.Invoke();
    }

    private void QuitBtnClicked()
    {
        this.OnQuitBtnClicked?.Invoke();
    }
}
