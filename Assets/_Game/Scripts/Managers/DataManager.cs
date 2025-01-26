using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    private int currentLevel = 1;

    public int CurrentLevel { get => currentLevel; set => currentLevel = value; }

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}
