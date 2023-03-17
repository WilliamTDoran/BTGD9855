using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : GameActor
{
    private Phase activePhase;

    /* Exposed Variables */
    [Header("Boss")]
    [Tooltip("The number of stages, in order from first to last, with the hp percent threshold for when the stage ENDS")]
    [SerializeField]
    private Phase[] phases;

    [Tooltip("Current phase (Only change manually for testing, always change back afterward)")]
    [SerializeField]
    private int currentPhase = 0;
    /*~~~~~~~~~~~~~~~~~~~*/

    protected override void Awake()
    {
        base.Awake();

        CurHitPoints = MaxHitPoints;

        Transition(currentPhase);
    }

    protected override void Update()
    {
        base.Update();

        if (CurHitPoints <= 0)
        {
            Destroy(gameObject);
        }
    }

    internal void Transition(int targetPhase)
    {
        activePhase = phases[targetPhase];
        currentPhase = targetPhase;
    }
}
