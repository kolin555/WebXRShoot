using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTriggerReturnCommond : ICommond
{
    public void Execute(IGunActor gunActor)
    {
        gunActor.GunTriggerReturn();
    }
}
