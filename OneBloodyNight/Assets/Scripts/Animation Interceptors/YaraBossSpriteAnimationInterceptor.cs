using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class YaraBossSpriteAnimationInterceptor : MonoBehaviour
{
    [SerializeField] private YaraBoss root;
    [SerializeField] private GurgeyProjectile gurgectile;

    public void FireRocks()
    {
        root.ShockwaveFire();
    }

    public void EndSlam()
    {
        root.ShockSlamRunning = false;
    }

    public void GurgeyItUp()
    {
        gurgectile.gameObject.SetActive(true);
        gurgectile.Shoot();
    }

    public void EndGurgey()
    {
        root.GurgeyRunning = false;
    }
}
