using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    private static LevelManager instance;
    public static LevelManager Instance => instance;

    [Header("===Component===")]
    [SerializeField] private GameLevelSO so;

    [Space(25)]

    [Header("===Attribute===")]
    [SerializeField] private int moveStepLeft;
    [SerializeField] private int currPoint;

    [Space(25)]

    [Header("===Moving Block===")]
    [SerializeField] private Transform movingBlockContainer;
    [SerializeField] private Block movingBlock;

    //==========================================Get Set===========================================
    //===Attribute===
    public int MoveStepLeft => this.moveStepLeft;
    public int CurrLevelIndex => this.so.LevelIndex;
    public int CurrPoint => this.currPoint;
    public int MaxPoint => this.so.MaxPoint;

    //===Event===
    public event Action OnIncreasePoint;
    public event Action OnMoveStepLeftChanged;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.movingBlockContainer, transform.Find("MovingBlockContainer"), 
            "LoadMovingBlockContainer");
    }

    protected override void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("One LevelManager only (this)", gameObject);
            Debug.LogError("One LevelManager only (instance)", instance.gameObject);
            return;
        }

        instance = this;
        base.Awake();
    }

    private void Start()
    {
        this.moveStepLeft = this.so.MoveStepLimit;
        this.currPoint = 0;
    }

    //===========================================Method===========================================
    public void SetMovingBlock(Block block)
    {
        this.movingBlock = block;
        block.transform.SetParent(this.movingBlockContainer);
    }

    public bool CanMoveBlock()
    {
        return this.movingBlock == null;
    }

    public void EmptyMovingBlock()
    {
        this.movingBlock = null;
    }

    public void IncreasePoint(Block block)
    {
        this.currPoint += block.Point;
        this.OnIncreasePoint?.Invoke();

    }

    public void DecreaseMoveStepLeft()
    {
        this.moveStepLeft--;
        this.OnMoveStepLeftChanged?.Invoke();
    }
}
