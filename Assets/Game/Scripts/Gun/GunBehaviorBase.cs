using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GunActionType
{
    Shoot,
    GunTriggerReturn
}

public class GunBehaviorBase : MonoBehaviour,IGunActor
{

    public float BalletSpeed;
    public Transform Muzzle;
    


    public ICommond ShootCommond;
    public ICommond GunTriggerReturnCommond;
    public ICommond EmptyCommond;

    private ICommond CurrentCommond;

    public Rigidbody rb;

    public bool canShoot=true;
    
    private void Awake()
    {
        ShootCommond=new ShootCommond();
        GunTriggerReturnCommond=new GunTriggerReturnCommond();
        EmptyCommond=new EmptyCommand();
        CurrentCommond = EmptyCommond;
    }

    /*
    private void Update()
    {
        //ICommond commond = GetCommond();
        CurrentCommond.Execute(this);
    }
    */

    public void GetCommond(GunActionType gunActionType)
    {

        CurrentCommond = EmptyCommond;
        switch (gunActionType)
        {
            case GunActionType.Shoot:
                CurrentCommond = ShootCommond;
                break;
            case GunActionType.GunTriggerReturn:
                CurrentCommond = GunTriggerReturnCommond;
                break;
        }
        CurrentCommond.Execute(this);
        
    }
    public void Shoot()
    {
        if (!canShoot)
        {
            return;
        }
        GameObject ballet = BalletPool.Instance.GetBallet();
        ballet.transform.position = Muzzle.position;
        ballet.GetComponent<Rigidbody>().AddForce(Muzzle.forward*BalletSpeed,ForceMode.Force);
        
        canShoot = false;
    }

    public void GunTriggerReturn()
    {
        canShoot = true;
    }
}
