using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelsData
{
    public List<LevelData> levels = new();

    public LevelsData(List<LevelData> levels)
    {
        this.levels = levels;
    }
}
