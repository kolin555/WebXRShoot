using System;
using System.Collections;
using System.Collections.Generic;
using Loxodon.Framework.Asynchronous;
using Loxodon.Framework.Bundles;
using Loxodon.Framework.Contexts;
using ShootTower;
using UnityEngine;
using Object = System.Object;

public class LoadAssetSystem : MonoBehaviour
{
#if UNITY_WEBGL
    public static LoadAssetSystem Instance;
    
     private IResources resources;

     private int allNeedLoadAssetsLength=0;
     private int currentLoadAssetIndex = 0;

     private string[] assetsNames;
     private Transform[] assetsPos;

     public float currentProcess=0;

     private List<GameObject> loadedObjs=new List<GameObject>();
     
     IEnumerator Start()
     {
         ApplicationContext context = Context.GetApplicationContext();

         while (this.resources == null)
         {
             this.resources = context.GetService<IResources>();
             yield return null;
         }

         MsgSystem.instance.SendMsg(MsgSystem.resources_ready,null);
         //this.Load(new string[] { "LoxodonFramework/BundleExamples/Models/Red/Red.prefab", "LoxodonFramework/BundleExamples/Models/Green/Green.prefab" });
         //this.StartCoroutine(Load2("LoxodonFramework/BundleExamples/Models/Plane/Plane.prefab"));
     }


     private void Awake()
     {
         Instance = this;
         
     }

        public void LoadLevelAssets(int messageLength,string[] names,Transform[] pos)
        {
            allNeedLoadAssetsLength = messageLength;
            currentLoadAssetIndex = 0;
            currentProcess = 0;
            assetsNames = names;
            assetsPos = pos;

            AssetLoad(assetsNames[currentLoadAssetIndex], assetsPos[currentLoadAssetIndex]);
        }
        
        private void AssetLoad(string name,Transform Pos)
        {
            this.StartCoroutine(LoadGameObjectAsync(name,Pos,AssetLoadCallBack));
        }

        private void AssetLoadCallBack(GameObject obj)
        {
            Debug.Log("完成一次");
            currentLoadAssetIndex += 1;
            currentProcess = ((float)currentLoadAssetIndex / (float)allNeedLoadAssetsLength);
            MsgSystem.instance.SendMsg(MsgSystem.getted_level_message,new object[]{currentProcess});
            //更新UI
            if (currentLoadAssetIndex == allNeedLoadAssetsLength)
            {
                //全部加载完毕
                MsgSystem.instance.SendMsg(MsgSystem.getted_all_assets,new Object[]{loadedObjs});
                
            }
            else
            {
                AssetLoad(assetsNames[currentLoadAssetIndex], assetsPos[currentLoadAssetIndex]);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        void Load(string[] names)
        {
            IProgressResult<float, GameObject[]> result = resources.LoadAssetsAsync<GameObject>(names);
            
            result.Callbackable().OnProgressCallback(p =>
            {
                Debug.LogFormat("Progress:{0}%", p * 100);
            });
            result.Callbackable().OnCallback((r) =>
            {
                try
                {
                    if (r.Exception != null)
                        throw r.Exception;

                    foreach (GameObject template in r.Result)
                    {
                        GameObject.Instantiate(template);
                    }

                }
                catch (Exception e)
                {
                    Debug.LogErrorFormat("Load failure.Error:{0}", e);
                }
            });
        }

        private void LoadBundle()
        {
            //resources.LoadBundle()
        }
        
        IEnumerator LoadGameObjectAsync1(string name,Transform Pos,Action<GameObject> CallBack)
        {
            IProgressResult<float, GameObject> result = resources.LoadAssetAsync<GameObject>(name);

            while (!result.IsDone)
            {
                Debug.LogFormat("Progress:{0}%", result.Progress * 100);
                yield return null;
            }

            try
            {
                if (result.Exception != null)
                    throw result.Exception;
                
                GameObject obj= Instantiate(result.Result,Pos.position,Quaternion.identity);
                //Debug.Log(obj.name);
                obj.gameObject.SetActive(false);
                
                loadedObjs.Add(obj);

                InitBundleShader(obj);
                CallBack(obj);
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("Load failure.Error:{0}", e);
            }
        }
        
        private void LoadGameObject(string name,Transform Pos,Action<GameObject> CallBack)
        {
            Debug.Log(name);
            GameObject gameObj= resources.LoadAsset<GameObject>("Game/Prefabs/ABPrefabs/" + name + ".prefab");
            
            //IProgressResult<float, GameObject> result = resources.LoadAssetAsync<GameObject>("Game/Prefabs/ABPrefabs/"+name+".prefab");
            //GameObject gameObj = o as GameObject;
            if(gameObj != null)
            {
                GameObject obj= Instantiate(gameObj,Pos.position,Quaternion.identity);
                obj.gameObject.SetActive(false);
                
                loadedObjs.Add(obj);

                InitBundleShader(obj);
                CallBack(obj);
            }
            else
            {
                Debug.LogErrorFormat("Load failure.Error");
            }

            /*while (!result.IsDone)
            {
                //Debug.LogFormat("Progress:{0}%", result.Progress * 100);
                yield return null;
            }*/

            /*try
            {
                if (result.Exception != null)
                    throw result.Exception;
                GameObject obj= Instantiate(result.Result,Pos.position,Quaternion.identity);
                //Debug.Log(obj.name);
                obj.gameObject.SetActive(false);
                
                loadedObjs.Add(obj);

                InitBundleShader(obj);
                CallBack(obj);
                //AssetLoadCallBack();
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("Load failure.Error:{0}", e);
            }*/
            

        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
         IEnumerator LoadGameObjectAsync(string name,Transform Pos,Action<GameObject> CallBack)
        {
            Debug.Log(name);
            IProgressResult<float, GameObject> result = resources.LoadAssetAsync<GameObject>("Game/Prefabs/ABPrefabs/"+name+".prefab");
            
            
            while (!result.IsDone)
            {
                //Debug.LogFormat("Progress:{0}%", result.Progress * 100);
                yield return null;
            }

            try
            {
                if (result.Exception != null)
                    throw result.Exception;
                GameObject obj= Instantiate(result.Result,Pos.position,Quaternion.identity);
                //Debug.Log(obj.name);
                obj.gameObject.SetActive(false);
                
                loadedObjs.Add(obj);
#if UNITY_EDITOR
                InitBundleShader(obj);
#endif
                CallBack(obj);
                //AssetLoadCallBack();
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("Load failure.Error:{0}", e);
            }
            

        }


        private void InitBundleShader(GameObject bundleInstance)
        {
            var AllRenderer = bundleInstance.transform.GetComponentsInChildren<Renderer>(true);
            for (int i = 0; i < AllRenderer.Length; i++)
            {
                var material = Shader.Find(AllRenderer[i].material.shader.name);
                AllRenderer[i].material.shader = null;
                AllRenderer[i].material.shader = material;
            }
            /*var renderer = bundleInstance.GetComponent<Renderer>();
            if (renderer != null)
            {
                var material = Shader.Find(renderer.material.shader.name);
                renderer.material.shader = null;
                renderer.material.shader = material;
            }*/
        }
#endif
}
