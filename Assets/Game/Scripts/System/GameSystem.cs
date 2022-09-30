using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

namespace ShootTower
{
    public class GameSystem : MonoBehaviour
    {
        private int level=0;

        public int Level => level;


        public static GameSystem Instance;

        

        public GameObject LoadUI;
        
        
        private void Awake()
        {
            Instance = this;
        }

        public int GetCurrentLevel()
        {
            return level;
        }



        private void Start()
        {
            //GameStart();
            MsgSystem.instance.RegistMsgAction(MsgSystem.getted_all_assets,LoadLevelAssetsOver);
            MsgSystem.instance.RegistMsgAction(MsgSystem.resources_ready, LoadLevelAsset);
            //LoadLevelAsset();
            TryOpenLoadUI();
            
            
        }
        
        

        private void TryOpenLoadUI()
        {
            //判断是否加载完毕
            if (LoadAssetSystem.Instance != null)
            {
                if (Mathf.Approximately(LoadAssetSystem.Instance.currentProcess,1))
                {
                    //加载完毕 如果开启了UI则关闭
                    if (LoadUI.activeSelf)
                    {
                        LoadUI.SetActive(false);
                    }
                }
                else
                {
                    //没有加载完毕，开启UI
                    LoadUI.SetActive(true);
                }
            }
            
        }

        private void LoadLevelAssetsOver(System.Object[] objects)
        {
            List<GameObject> objs = (List<GameObject>) objects[0];
            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].gameObject.SetActive(true);
            }
            
            //TryGoNextLevel();
        }

        public void GoNextLevel()
        {
            level += 1;
            LoadLevelAsset(null);
            TryOpenLoadUI();
        }
        
        
        
        private void LoadLevelAsset(System.Object[] objects)
        {
            
            if (LevelMessageSystem.Instance != null)
            {
                var levelMessageSystem = LevelMessageSystem.Instance;
                var towerNames = levelMessageSystem.GetCurrentLevelMessage(level).towerName;
                var towerPos = levelMessageSystem.GetTowerPos(level);
                var messageLength = levelMessageSystem.GetCurrentLevelTowerNum(level);
                LoadAssetSystem.Instance.LoadLevelAssets(messageLength,towerNames,towerPos);
            }
            else
            {
                Debug.LogError("LevelMessageSystem为空");
            }

            
        }


        
    }

}
