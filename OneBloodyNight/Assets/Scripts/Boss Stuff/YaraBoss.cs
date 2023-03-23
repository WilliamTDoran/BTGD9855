using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YaraBoss : Boss
{
    protected override void Start()
    {
        base.Start();

        StartRandomBehavior();
    }
}
