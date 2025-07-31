using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Block : HuyMonoBehaviour, Touchable
{
    public const float BlockGap = 10;

    //==========================================Variable==========================================
    //===Component===
    [SerializeField] private BoxCollider bodyCol;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BlockSO so;

    //===Attribute===
    [SerializeField] private Vector3 tempCollidedBlockPos;
    [SerializeField] private float tempSpeedUpTime;
    [SerializeField] private bool isHit = false;
    [SerializeField] private bool isMovingWithDir = false;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Block")) this.CollideWithBlock(collision);
    }

    private void Update()
    {
        this.Moving();
        this.Despawning();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        this.tempSpeedUpTime = 0;
        this.isHit = false;
        this.isMovingWithDir = false;
    }

    //============================================Move============================================
    private void Moving()
    {
        if (this.isHit) this.MoveBackward();
        else if (this.isMovingWithDir) this.MoveWithDir();
    }

    private void MoveWithDir()
    {
        this.tempSpeedUpTime += Time.deltaTime;
        this.rb.velocity = this.so.MoveDir * this.so.MoveSpeed * (this.tempSpeedUpTime / this.so.speedUpTime);
    }

    private void MoveBackward()
    {
        Vector3.Lerp(transform.position, this.tempCollidedBlockPos, this.so.MoveBackwardSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, this.tempCollidedBlockPos) < BlockGap) return;
        this.isHit = false;
    }

    //==========================================Despawn===========================================
    private void Despawning()
    {
        if (!this.isMovingWithDir) return;
        float distance = Vector3.Distance(transform.position, Vector3.zero);

        if (distance < this.so.DespawnDistance) return;
        this.gameObject.SetActive(false);
    }

    //==========================================Collide===========================================
    private void CollideWithBlock(Collision collision)
    {
        this.tempCollidedBlockPos = collision.transform.position;
        this.isMovingWithDir = false;
        this.isHit = true;
    }

    //=========================================Touchable==========================================
    void Touchable.Touch()
    {
        this.isMovingWithDir = true;
    }
}
