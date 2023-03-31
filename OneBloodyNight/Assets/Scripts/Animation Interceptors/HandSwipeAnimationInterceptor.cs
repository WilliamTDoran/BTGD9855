using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSwipeAnimationInterceptor : GameActor
{
    protected override void Awake() { } //hides an annoying and unnecessary assert

    public void DonezelWashington()
    {
        RemoteCondition = true;
    }
}
