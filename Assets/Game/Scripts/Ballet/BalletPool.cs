using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalletPool : MonoBehaviour
{
    public static BalletPool Instance;
    private  Queue<GameObject> Ballets=new Queue<GameObject>();

    public GameObject Ballet;


    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            var ballet= GameObject.Instantiate(this.Ballet,this.transform);
            RecycleBallet(ballet);
        }
    }

    public void RecycleBallet(GameObject ballet)
    {
        ballet.GetComponent<Rigidbody>().velocity=Vector3.zero;
        ballet.SetActive(false);
        Ballets.Enqueue(ballet);
    }
    
    public GameObject GetBallet()
    {
        if (Ballets.Count > 0)
        {
            GameObject Cube=Ballets.Dequeue();
            Cube.SetActive(true);
            
            return Cube;
        }
        return GameObject.Instantiate(this.Ballet,this.transform);
    }

}
