using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Game Level")]
public class GameLevelSO : ScriptableObject
{
    public int MoveStepLimit;
    public int LevelIndex;
    public int MaxPoint;
}
