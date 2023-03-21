using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : GameActor
{
    /* Exposed Variables */
    [Header("Boss")]
    [Tooltip("The number of phases. The value of each should be the health percent when the boss will ENTER the phase")]
    [SerializeField]
    protected float[] phases;

    [Tooltip("Current phase (Only change manually for testing, always change back afterward)")]
    [SerializeField]
    protected int currentPhase = 0;
    /*~~~~~~~~~~~~~~~~~~~*/

    protected override void Awake()
    {
        base.Awake();

        CurHitPoints = MaxHitPoints;
    }

    protected override void Update()
    {
        base.Update();

        if (CurHitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }
}
