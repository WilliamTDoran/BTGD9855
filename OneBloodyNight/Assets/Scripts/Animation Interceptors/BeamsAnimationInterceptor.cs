using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamsAnimationInterceptor : MonoBehaviour
{
    [SerializeField] private Attack root;

    public void ResetDamage()
    {
        root.EndSwing();
    }

    public void StartActivity()
    {
        root.Col.enabled = true;
    }

    public void EndActivity()
    {
        root.Col.enabled = false;
    }
}
