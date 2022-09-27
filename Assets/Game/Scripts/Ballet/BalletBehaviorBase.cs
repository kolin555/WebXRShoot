using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class BalletBehaviorBase : MonoBehaviour
{
    public float RecycleTime;

    public float currentTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= RecycleTime)
        {
            BalletPool.Instance.RecycleBallet(this.gameObject);
            currentTime = 0;
        }
    }
}
