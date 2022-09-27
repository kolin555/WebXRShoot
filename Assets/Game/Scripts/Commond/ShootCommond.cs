using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootCommond : ICommond
{
    
    public void Execute(IGunActor gunActor)
    {
        gunActor.Shoot();
    }
}
