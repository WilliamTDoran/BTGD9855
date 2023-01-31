using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class drives the Strigoi character. 
/// It has a basic attack which is a quick slash,
/// an activated ability which is invisibility,
/// and a passive effect which is an increasing power as it fights.
/// </summary>
public class PlayerStrigoi : Player
{
    private float berserkCounter; //current berserk time, reset on call
    private float berserkUptime; //true berserk time, used for calculating incremental buffs
    private IEnumerator berserkCoroutine; //runs berserk time going down

    /* Exposed Variables */
    [Header("Strigoi Exclusive")]
    [Header("References")]
    [Tooltip("A reference to the strigoi's basic attack object")]
    [SerializeField]
    private Attack basicAttack;

    [Header("Statistics")]
    [Tooltip("How long before berserk expires")]
    [SerializeField]
    private float berserkMaxTime = 10.0f;
    /*~~~~~~~~~~~~~~~~~~~*/

    /// <summary>
    /// Standard update, inherits from Player and GameActor
    /// Drives attack controls
    /// </summary>
    protected override void Update()
    {
        base.Update();

        if (basicAttackDown && canAttack)
        {
            basicAttack.StartSwing();
        }

        canAttackDebugText.text = canAttack + "";
    }

    private IEnumerator Berserk(float maxTime)
    {
        yield return new WaitForSeconds(maxTime);
    }

    private void StartBerserk()
    {
        berserkCoroutine = Berserk(berserkMaxTime);
        StartCoroutine(berserkCoroutine);
    }

    private void StopBerserk()
    {
        StopCoroutine(berserkCoroutine);
        berserkCoroutine = null;
    }
}
