using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaincloudAnimationInterceptor : MonoBehaviour
{
    [SerializeField] private Raincloud root;

    public void CallToStart()
    {
        root.StartRainDamage();
    }

    public void CallToEnd()
    {
        root.StopRainDamage();
    }

    public void Close()
    {
        root.gameObject.SetActive(false);
    }
}
