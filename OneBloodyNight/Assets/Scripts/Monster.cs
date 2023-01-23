using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    /*~~~~~~~~~~~~~~~~~~~*/

    protected override void Start()
    {
        base.Start();

        canAttack = true;
        canMove = true;
        facingAngle = 0;

        //StartCoroutine(DebugAttackCycle());
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
}