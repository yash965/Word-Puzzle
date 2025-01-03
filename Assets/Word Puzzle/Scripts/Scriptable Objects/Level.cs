using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Level : ScriptableObject
{
    public string[] levels;
    public int levelIndex;

    private void OnEnable()
    {
        levelIndex = 0;
    }
}
