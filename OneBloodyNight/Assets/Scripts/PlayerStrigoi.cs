using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This class drives the Strigoi character. 
/// It has a basic attack which is a quick slash,
/// an activated ability which is invisibility,
/// and a passive effect which is an increasing power as it fights.
/// 
/// Version 1.0 (1/20/23), Will Doran
/// Version 1.1 (2/3/23),  Will Doran
/// </summary>
public class PlayerStrigoi : Player
{
    private IEnumerator berserkCoroutine; //runs bloodthirst
    private IEnumerator invisibilityCoroutine; //runs invisibility duration
    private IEnumerator batSwarmCoroutine; //runs swarm

    private float berserkCounter; //current berserk time, reset on call
    private float berserkUptime; //true berserk time, used for calculating incremental buffs

    private int basicAttackBaseDamage; //used for returning to default after berserk
    private float baseSpeed; //used for returning to default after berserk
    public float BaseSpeed { get { return baseSpeed; } set { baseSpeed = value; } }

    /* Exposed Variables */
    [Header("Strigoi Exclusive")]
    [Header("References")]
    [Tooltip("A reference to the strigoi's basic attack object")]
    [SerializeField]
    private Attack basicAttack;

    [Tooltip("The standard material for default Strigoi appearance")]
    [SerializeField]
    private Material basicMaterial;

    [Tooltip("The material for when the Strigoi is invisible")]
    [SerializeField]
    private Material invisibleMaterial;

    [Header("Debug")]
    [SerializeField]
    protected TextMeshProUGUI berserkUpDebugText;

    [SerializeField]
    protected TextMeshProUGUI berserkCounterDebugText;

    [SerializeField]
    protected TextMeshProUGUI berserkUptimeDebugText;

    [SerializeField]
    protected TextMeshProUGUI berserkDamageDebugText;

    [SerializeField]
    protected TextMeshProUGUI berserkSpeedDebugText;

    [Header("Statistics")]
    [Header("Bloodthirst")]
    [Tooltip("How long before berserk expires")]
    [SerializeField]
    private float berserkMaxTime = 3.0f;

    [Tooltip("How much the basic attack's damage increases every second while berserk")]
    [SerializeField]
    private int damageUpPerSecond = 1;

    [Tooltip("How much the Strigoi's movement speed increases every second while berserk")]
    [SerializeField]
    private int speedUpPerSecond = 1;

    [Header("Invisibility")]
    [Tooltip("Duration of invisibility in seconds")]
    [SerializeField]
    private float invisibilityDuration = 3;
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
        berserkSpeedDebugText.text = speed + "";
    }

    protected override void Start()
    {
        base.Start();

        basicAttackBaseDamage = basicAttack.Damage;
        baseSpeed = speed;
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

    protected override void AdvancedFire(ref int bullet)
    {
        base.AdvancedFire(ref bullet);

        switch(bullet)
        {
            case 0:
                break;

            case 1:
                bullet = 0;
                Invisibility();
                break;

            case 2:
                bullet = 0;
                BatSwarm();
                break;

            default:
                break;
        }
    }

    private void Invisibility()
    {
        Bloodmeter.instance.changeBlood(-abilityOneCost);

        StartInvisible();
    }

    private void BatSwarm()
    {
        throw new NotImplementedException();
    }

    private IEnumerator Berserk(float maxTime)
    {
        while (berserkCounter > 0)
        {
            yield return new WaitForEndOfFrame();
            berserkCounter -= Time.deltaTime;
            berserkUptime += Time.deltaTime;

            basicAttack.Damage = basicAttackBaseDamage + (int)(berserkUptime * damageUpPerSecond);
            speed = baseSpeed + (int)(berserkUptime * speedUpPerSecond);

            berserkUpDebugText.text = "True";
            berserkCounterDebugText.text = berserkCounter + "";
            berserkUptimeDebugText.text = berserkUptime + "";
        }

        basicAttack.Damage = basicAttackBaseDamage;
        speed = baseSpeed;
        berserkUptime = 0;

        berserkUpDebugText.text = "False";
        berserkCounterDebugText.text = "0.0";
        berserkUptimeDebugText.text = "0.0";
    }

    private IEnumerator Invisible()
    {
        spriteRenderer.material = invisibleMaterial;

        yield return new WaitForSeconds(invisibilityDuration);

        spriteRenderer.material = basicMaterial;
    }

    private IEnumerator Swarmed()
    {
        throw new NotImplementedException();
    }


    
    private void StartInvisible()
    {
        invisibilityCoroutine = Invisible();
        StartCoroutine(invisibilityCoroutine);
    }

    private void StopInvisible()
    {
        StopCoroutine(invisibilityCoroutine);
        invisibilityCoroutine = null;

        spriteRenderer.material = basicMaterial;
    }

    private void StartBatSwarm()
    {
        batSwarmCoroutine = Swarmed();
        StartCoroutine(batSwarmCoroutine);
    }

    private void StopBatSwarm()
    {
        StopCoroutine(batSwarmCoroutine);
        batSwarmCoroutine = null;
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
