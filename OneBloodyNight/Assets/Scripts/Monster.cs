using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class for all monster types. Some monsters can be driven just from this, complex ones inherit their own classes
/// 
/// Version 1.0 (1/20/23), William Doran
/// </summary>
public class Monster : GameActor
{
    private int curHitPoints;
    public int CurHitPoints { get { return curHitPoints; } set { curHitPoints = value; } }

    /* Exposed Variables */
    [Tooltip("The Monster's maximum possible hit points (also the hit points it spawns with)")]
    [SerializeField]
    private int maxHitPoints;
    public int MaxHitPoints { get { return maxHitPoints; } set { maxHitPoints = value; } }

    [Tooltip("A reference to the strigoi's basic attack object")]
    [SerializeField]
    private Attack basicAttack;

    [SerializeField]
    private bool debugFollow;
    /*~~~~~~~~~~~~~~~~~~~*/

    protected override void Start()
    {
        base.Start();

        canAttack = true;
        canMove = true;
        facingAngle = 0;

        StartCoroutine(DebugAttackCycle());
    }

    private void FixedUpdate()
    {
        if (debugFollow)
        {
            DebugCharge();
        }
    }

    private IEnumerator DebugAttackCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);

            basicAttack.StartSwing();

            Debug.Log("Werewolf Swing Done");
        }
    }

    private void DebugCharge()
    {

    }
}