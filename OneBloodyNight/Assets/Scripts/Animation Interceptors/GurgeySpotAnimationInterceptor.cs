using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GurgeySpotAnimationInterceptor : GameActor
{
    protected override void Awake() { } //hides an annoying and unnecessary assert

    private void OnEnable()
    {
        RemoteCondition = false;
    }

    public void Donezo()
    {
        RemoteCondition = true;
    }
}
