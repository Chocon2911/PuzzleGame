using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Block")]
public class BlockSO : ScriptableObject
{
    public float MoveBackwardSpeed;
    public float MoveSpeed;
    public float speedUpTime;
    public Vector3 MoveDir;
    public float DespawnDistance;
}
