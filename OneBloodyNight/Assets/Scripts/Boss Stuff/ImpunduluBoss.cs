using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpunduluBoss : Boss
{
    [SerializeField]
    private Projectile[] feathers;

    protected override void Start()
    {
        base.Start();

        facingAngle = 180;

        feathers[0].gameObject.SetActive(true);
        feathers[0].Fire();
    }
}
