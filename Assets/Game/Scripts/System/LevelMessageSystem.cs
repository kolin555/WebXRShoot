using System;
using System.Collections;
using System.Collections.Generic;
using ShootTower;
using UnityEngine;


//存储每关信息
public class LevelMessageSystem : MonoBehaviour
{
    //public Transform[] towerPos;

    public static LevelMessageSystem Instance;
    public GenericDictionary<int,Transform[]> towerPos;
    public LevelMessage[] levelMessage;
    
    
    
    public int GetCurrentLevelTowerNum(int level)
    {
        return levelMessage[level].towerName.Length;
    }

    public LevelMessage GetCurrentLevelMessage(int level)
    {
        return levelMessage[level];
    }
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CheckTowerPos();
    }


    public Transform[] GetTowerPos(int level)
    {
        //var level = GameSystem.Instance.GetCurrentLevel();
        return towerPos[level];
    }

    //检查塔的数量与存储位置数量是否一致
    private void CheckTowerPos()
    {
        var level = GameSystem.Instance.GetCurrentLevel();
        var towerNum = levelMessage[level].towerName.Length;

        var towerPosNum = towerPos[level].Length;

        if (towerNum != towerPosNum)
        {
            Debug.LogError("塔的数量与位置数量不一致");
        }
    }
    
}
