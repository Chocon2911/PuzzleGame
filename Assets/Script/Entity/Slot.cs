using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Slot : HuyMonoBehaviour
{
    //==========================================Variable==========================================
    [SerializeField] private Block block;
    [SerializeField] private SphereCollider bodyCol;
    [SerializeField] private Transform model;
    [SerializeField] private BlockType spawnBockType;

    //==========================================Get Set===========================================
    public Block Block { get => block; set => block = value; }
    public BlockType spawnBlockType { get => spawnBockType; set => spawnBockType = value; }

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadChildComponent(ref this.block, transform, "LoadBlock()");
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
        this.LoadComponent(ref this.model, transform.Find("Model"), "LoadModel()");

        if (this.block != null)
        {
            this.block.transform.localPosition = Vector3.zero;
            this.block.CurrSlot = this;
            this.block.LoadComponents();
        }
        if (this.bodyCol != null)
        {
            this.bodyCol.isTrigger = true;
        }
    }

    //===========================================Method===========================================
    public void Appear()
    {
        this.model.gameObject.SetActive(true);
    }

    public void Disappear()
    {
        this.model.gameObject.SetActive(false);
    }

    public void UpdateBlock(Block block)
    {
        if (this.block != null) this.block.CurrSlot = null;
        if (block != null)
        {
            block.transform.SetParent(transform);
            block.CurrSlot = this;
        }
        this.block = block;
    }

    public void SpawnBlock()
    {
        if (this.block != null)
        {
            DestroyImmediate(this.block.gameObject);
        }

        switch (this.spawnBockType)
        {
            case BlockType.Forward:
                this.SpawnBlock("Forward_Block");
                break;
            case BlockType.Down:
                this.SpawnBlock("Down_Block");
                break;
            case BlockType.Right:
                this.SpawnBlock("Right_Block");
                break;
            case BlockType.Left:
                this.SpawnBlock("Left_Block");
                break;
        }
    }

    private void SpawnBlock(string blockName)
    {
        Block block = Resources.Load<Block>("Prefab/" + blockName);
        Block newBlock = Instantiate(block.gameObject, transform.position, transform.rotation).GetComponent<Block>();
        this.UpdateBlock(newBlock);
    }
}
