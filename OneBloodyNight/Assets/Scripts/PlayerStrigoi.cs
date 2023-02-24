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
/// Version 2.0 (2/7/23),  Will Doran
/// </summary>
public class PlayerStrigoi : Player
{
    private string basicCode = "basic"; //see Attack comments for attackerGrantedCode
    private string batCode = "bat";

    private float actualVelocity;
    private float clampedVelocity;

    private IEnumerator berserkCoroutine; //runs bloodthirst
    private IEnumerator invisibilityCoroutine; //runs invisibility duration
    private IEnumerator batSwarmCoroutine; //runs swarm

    private float berserkCounter; //current berserk time, reset on call
    private float berserkUptime; //true berserk time, used for calculating incremental buffs

    private int swarmCounter = 0; //current number of swarm ticks used

    private int basicAttackBaseDamage; //used for returning to default after berserk
    private float baseSpeed; //used for returning to default after berserk
    public float BaseSpeed { get { return baseSpeed; } set { baseSpeed = value; } }

    /* Exposed Variables */
    [Header("Strigoi Exclusive")]
    [Header("References")]
    [Tooltip("A reference to the strigoi's basic attack object")]
    [SerializeField]
    private Attack basicAttack;

    [Tooltip("A reference to the attack hitbox for the bat swarm")]
    [SerializeField]
    private Attack swarmAttack;

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

    [Header("Bat Swarm")]
    [Tooltip("Number of swarm ticks before expiry")]
    private int batSwarmDuration = 20;
    /*~~~~~~~~~~~~~~~~~~~*/

    /// <summary>
    /// Standard Start function. Sets some values needed for berserk
    /// </summary>
    protected override void Start()
    {
        base.Start();

        basicAttackBaseDamage = basicAttack.Damage;
        baseSpeed = speed;
    }

    /// <summary>
    /// Standard update, inherits from Player and GameActor
    /// Drives attack controls
    /// </summary>
    protected override void Update()
    {
        base.Update();

        if (basicAttackDown && canAttack && !stunned && !GameManager.instance.GCD(false)) //bit messy all these checks, but it gets the job done and makes it actually pretty airtight
        {
            basicAttack.StartSwing(basicCode);
            canAttack = false;
            if (invisibilityCoroutine != null) StopInvisible(); //attacking cancels invisibility
        }

        canAttackDebugText.text = canAttack + "";
        berserkDamageDebugText.text = basicAttack.Damage + "";
        berserkSpeedDebugText.text = speed + "";
    }

    private void LateUpdate()
    {
        actualVelocity = rb.velocity.magnitude;
        clampedVelocity = actualVelocity > 10f ? 10f : actualVelocity;
        animator.SetFloat("Speed", clampedVelocity);

        if (facingAngle + 90f >= 180)
        {
            render.flipX = true;
        }
        else
        {
            render.flipX = false;
        }
    }

    /// <summary>
    /// Launches advanced fire when called. Takes an int parameter that represents which, if any, of the strigoi's special attacks are loaded.
    /// This gets a bit redundant since each player type needs essentially this exact code, but just calling different functions, but for only 3 player types it's not the worst.
    /// </summary>
    /// <param name="bullet">Which advanced ability is loaded. 0 for none, 1 for invisibility, 2 for bat swarm.</param>
    protected override void AdvancedFire(ref int bullet)
    {
        base.AdvancedFire(ref bullet);

        switch(bullet)
        {
            case 0: //no bullet is loaded
                break;

            case 1: //invisibility is loaded
                if (!stunned && !GameManager.instance.GCD(true))
                {
                    bullet = 0; //resets the bullet after firing. if you can't fire, the bullet stays loaded. this might change in future
                    Invisibility();
                }
                break;

            case 2: //bat swarm is loaded
                if (!stunned && !GameManager.instance.GCD(true))
                {
                    bullet = 0; //resets the bullet after firing. if you can't fire, the bullet stays loaded. this might change in future
                    BatSwarm();
                }
                break;

            default: //this should never happen
                break;
        }
    }

    /// <summary>
    /// Drains the appropriate amount of blood then calls up the invisibility coroutine.
    /// </summary>
    private void Invisibility()
    {
        Bloodmeter.instance.changeBlood(-abilityOneCost);

        StartInvisible();
    }

    private void BatSwarm()
    {
        Bloodmeter.instance.changeBlood(-abilityTwoCost);

        swarmCounter = 0;
        swarmAttack.ForceStill = true;
        swarmAttack.StartSwing(batCode);
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

    internal override void OnAttackEnd(string code)
    {
        base.OnAttackEnd(code);

        if (code == basicCode)
        {
            canAttack = true;
        }
        else if (code == batCode)
        {
            swarmAttack.ForceStill = false;

            if (swarmCounter <= batSwarmDuration)
            {
                swarmCounter++;
                swarmAttack.StartSwing(batCode);
            }
        }
    }

    internal override void OnSuccessfulAttack(string code)
    {
        base.OnSuccessfulAttack(code);

        berserkCounter = berserkMaxTime;

        if (berserkUptime <= 0)
        {
            StartBerserk();
        }
    }


    private IEnumerator Invisible()
    {
        spriteRenderer.material = invisibleMaterial;
        Visible = false;

        yield return new WaitForSeconds(invisibilityDuration);

        Visible = true;
        spriteRenderer.material = basicMaterial;
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

        Visible = true;
        spriteRenderer.material = basicMaterial;
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
