using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockState
{
    Idle = 0,
    MovingWithDir = 1,
    MovingBackward = 2,
}

public enum BlockType
{
    None = 0,
    Forward = 1,
    Down = 2,
    Right = 3,
    Left = 4,
}

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class Block : HuyMonoBehaviour, Touchable
{
    public const float BlockGap = 1.5f;

    //==========================================Variable==========================================
    [Header("===Component===")]
    [SerializeField] private BoxCollider bodyCol;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BlockSO so;
    [SerializeField] private Slot currSlot;

    [Space(25)]

    [Header("===Attribute===")]
    [SerializeField] private BlockState state = BlockState.Idle;

    //==========================================Get Set===========================================
    public Slot CurrSlot { get => this.currSlot; set => this.currSlot = value; }
    public int Point => this.so.point;

    //===========================================Unity============================================
    public override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadComponent(ref this.rb, transform, "LoadRb()");
        this.LoadComponent(ref this.bodyCol, transform, "LoadBodyCol()");
        this.rb.isKinematic = false;
    }

    protected override void Awake()
    {
        base.Awake();
        this.LoadComponent(ref this.currSlot, transform.parent, "LoadSlot()");
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
        this.rb.velocity = Vector3.zero;
        this.state = BlockState.Idle;
    }

    //============================================Move============================================
    private void Moving()
    {
        if (this.state == BlockState.MovingBackward) this.MoveBackward();
        else if (this.state == BlockState.MovingWithDir) this.MoveWithDir();
    }

    private void MoveWithDir()
    {
        this.rb.AddForce(this.so.MoveDir * this.so.MoveSpeed * Time.deltaTime, ForceMode.Impulse);
    }

    private void MoveBackward()
    {
        if (this.currSlot == null)
        {
            Vector3 offset = new Vector3(
                transform.localScale.x / 1.5f * -this.so.MoveDir.x,
                transform.localScale.y / 1.5f * -this.so.MoveDir.y + 0.2f,
                transform.localScale.z / 1.5f * -this.so.MoveDir.z);
            Vector3 rayOrigin = transform.position + offset;
            Vector3 rayDirection = Vector3.down;
            float rayDistance = 100f;

            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, rayDistance))
            {
                Slot slot = hitInfo.collider.GetComponent<Slot>();
                if (slot != null)
                {
                    slot.UpdateBlock(this);
                    Debug.Log("Found Slot: " + slot.name);
                }
                else
                {
                    Debug.Log("Collider hit, not found Slot component: " + hitInfo.collider.name);
                }

            }
            else
            {
                Debug.Log("Hit nothing");
            }

            Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);
        }

        if (transform.localPosition.x > -0.01f && transform.localPosition.y > -0.01f
            && transform.localPosition.z > -0.01f && transform.localPosition.x < 0.01f
            && transform.localPosition.y < 0.01f && transform.localPosition.z < 0.01f)
        {
            this.rb.velocity = Vector3.zero;
            LevelManager.Instance.EmptyMovingBlock();
            this.state = BlockState.Idle;
            return;
        }

        if (this.rb.velocity.x > 0.01f && this.rb.velocity.y > 0.01f && this.rb.velocity.z > 0.01f)
        {
            Vector3 moveDir = (this.currSlot.transform.position - transform.position).normalized;
            this.rb.AddForce(moveDir * this.so.MoveBackwardSpeed * Time.deltaTime, ForceMode.Impulse);
        }
        else
        {
            this.rb.velocity = Vector3.zero;
            Vector3 newPos = Vector3.Lerp(transform.position, this.currSlot.transform.position, this.so.MoveSpeed * Time.deltaTime);
            transform.position = newPos;
        }
        
        this.state = BlockState.MovingBackward;
    }

    //==========================================Despawn===========================================
    private void Despawning()
    {
        if (this.state  != BlockState.MovingWithDir) return;
        float distance = Vector3.Distance(transform.position, Vector3.zero);

        if (distance < this.so.DespawnDistance) return;
        if (this.currSlot != null)
        {
            this.currSlot.UpdateBlock(null);
            this.currSlot = null;
        }
        this.state = BlockState.Idle;
        LevelManager.Instance.IncreasePoint(this);
        Destroy(gameObject);
    }

    //==========================================Collide===========================================
    private void CollideWithBlock(Collision collision)
    {
        this.state = BlockState.MovingBackward;

        if (this.currSlot != null) return;
        Vector3 offset = new Vector3(
            transform.localScale.x / 2 * -this.so.MoveDir.x,
            transform.localScale.y / 2 * -this.so.MoveDir.y,
            transform.localScale.z / 2 * -this.so.MoveDir.z);
        Vector3 rayOrigin = transform.position + offset;
        Vector3 rayDirection = Vector3.down;
        float rayDistance = 100f;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, rayDistance))
        {
            Slot slot = hitInfo.collider.GetComponent<Slot>();
            if (slot != null)
            {
                slot.UpdateBlock(this);
                Debug.Log("Found Slot: " + slot.name);
            }
            else
            {
                Debug.Log("Collider hit, not found Slot component: " + hitInfo.collider.name);
            }

        }
        else
        {
            Debug.Log("Hit nothing");
        }
        Debug.DrawRay(rayOrigin, rayDirection * rayDistance, Color.red);
    }

    //=========================================Touchable==========================================
    void Touchable.Touch()
    {
        if (this.state != BlockState.Idle) return;
        this.state = BlockState.MovingWithDir;
        this.currSlot.UpdateBlock(null);
        LevelManager.Instance.SetMovingBlock(this);
        LevelManager.Instance.DecreaseMoveStepLeft();
    }
}
