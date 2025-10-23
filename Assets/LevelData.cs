using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Custom/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] public int level;
    [SerializeField] private int pathBranchCount = 0;

    public int GetExtraPathsCount() { return pathBranchCount; }
}
