using System;
using UnityEngine;
using System.Collections;

public class Player : Entity
{

    public static Player Instance;

    public override void OnEnable()
    {
        
        base.OnEnable();
    }

    public override void OnDisable()
    {
        
        base.OnDisable();
    }

    public void OnDestroy()
    {
        Instance = null;

    }

    public override void Awake()
    {
        Instance = this;

        base.Awake();
    }

    public override void Update()
    {
        base.Update();
    }



}
