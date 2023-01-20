using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : GameActor
{
    private int curHitPoints;

    /* Exposed Variables */
    [Tooltip("The Monster's maximum possible hit points (also the hit points it spawns with)")]
    [SerializeField]
    private int maxHitPoints;
    public int MaxHitPoints { get { return maxHitPoints; } set { maxHitPoints = MaxHitPoints; } }

    [Tooltip("A reference to the strigoi's basic attack object")]
    [SerializeField]
    private Attack basicAttack;
    /*~~~~~~~~~~~~~~~~~~~*/

    protected override void Start()
    {
        base.Start();

        StartCoroutine(DebugAttackCycle());
    }

    private IEnumerator DebugAttackCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);

            basicAttack.StartSwing();
        }
    }
}