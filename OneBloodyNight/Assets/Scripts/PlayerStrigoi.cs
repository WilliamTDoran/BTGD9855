using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private int basicAttackBaseDamage; //used for returning to default after berserk

    /* Exposed Variables */
    [Header("Strigoi Exclusive")]
    [Header("References")]
    [Tooltip("A reference to the strigoi's basic attack object")]
    [SerializeField]
    private Attack basicAttack;

    [Header("Statistics")]
    [Tooltip("How long before berserk expires")]
    [SerializeField]
    private float berserkMaxTime = 3.0f;

    [Tooltip("How much the basic attack's damage increases every second while berserk")]
    [SerializeField]
    private int damageUpPerSecond = 1;

    [Header("Debug")]
    [SerializeField]
    protected TextMeshProUGUI berserkUpDebugText;

    [SerializeField]
    protected TextMeshProUGUI berserkCounterDebugText;

    [SerializeField]
    protected TextMeshProUGUI berserkUptimeDebugText;

    [SerializeField]
    protected TextMeshProUGUI berserkDamageDebugText;
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
        berserkDamageDebugText.text = basicAttack.Damage + "";
    }

    protected override void Start()
    {
        base.Start();

        basicAttackBaseDamage = basicAttack.Damage;
    }

    internal override void OnSuccessfulAttack()
    {
        base.OnSuccessfulAttack();

        berserkCounter = berserkMaxTime;

        if (berserkUptime <= 0)
        {
            StartBerserk();
        }
    }

    private IEnumerator Berserk(float maxTime)
    {
        while (berserkCounter > 0)
        {
            yield return new WaitForEndOfFrame();
            berserkCounter -= Time.deltaTime;
            berserkUptime += Time.deltaTime;

            basicAttack.Damage = basicAttackBaseDamage + (int)(berserkUptime * damageUpPerSecond);

            berserkUpDebugText.text = "True";
            berserkCounterDebugText.text = berserkCounter + "";
            berserkUptimeDebugText.text = berserkUptime + "";
        }

        basicAttack.Damage = basicAttackBaseDamage;
        berserkUptime = 0;

        berserkUpDebugText.text = "False";
        berserkCounterDebugText.text = "0.0";
        berserkUptimeDebugText.text = "0.0";
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
