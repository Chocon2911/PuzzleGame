using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public int level;
    public bool isUnlocked;

    public LevelData(int level, bool isUnlocked)
    {
        this.level = level;
        this.isUnlocked = isUnlocked;
    }
}
