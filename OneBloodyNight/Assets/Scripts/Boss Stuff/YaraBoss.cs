using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script drives the Yara-ma-yha-who boss fight.
/// 
/// Version 1.0 (3/24/23), Will Doran
/// </summary>
public class YaraBoss : Boss
{
    /* Exposed Variables */
    [SerializeField]
    private Animator spanimator;

    [SerializeField]
    private SpriteRenderer sprender;

    [Header("Attack Timings")]
    [SerializeField]
    private float timeBeforeBasic;

    [SerializeField]
    private float timeBeforeShockwave;

    [SerializeField]
    private float timeBeforeRegurgitate;

    [SerializeField]
    private float timeBeforePound;

    [SerializeField]
    private float timeBeforeBloodStrike;
    /*~~~~~~~~~~~~~~~~~~~*/

    protected override void Start()
    {
        base.Start();

        StartRandomBehavior();
    }

    protected override void LateUpdate()
    {
        if (walkAnim)
        {
            actualVelocity = rb.velocity.magnitude;
            clampedVelocity = actualVelocity > 1f ? 1f : actualVelocity;
            spanimator.SetFloat("Speed", clampedVelocity);

            //This is clumsy and causes awkward stuttering when moving vertically or near-vertically. Should ideally be replaced
            if (rb.velocity.magnitude > 0.1)
            {
                if (rb.velocity.x < 0)
                {
                    sprender.flipX = true;
                }
                else
                {
                    sprender.flipX = false;
                }
            }
        }
    }

    protected override IEnumerator RandomAttacking()
    {
        int upperLimit = rndCap + currentPhase;
        int check = rnd.Next(0, upperLimit);

        switch (check)
        {
            case 0:
                {
                    //Boosted basic, I've been told to ignore this
                    yield return new WaitForSeconds(timeBeforeBasic * timeModifier);
                    break;
                }

            case 1:
                {
                    //Shockwave Slam
                    yield return new WaitForSeconds(timeBeforeShockwave * timeModifier);
                    break;
                }

            case 2:
                {
                    //Regurgitate
                    yield return new WaitForSeconds(timeBeforeRegurgitate * timeModifier);
                    break;
                }

            case 3:
                {
                    //Ground Pound
                    yield return new WaitForSeconds(timeBeforePound * timeModifier);
                    break;
                }

            case 4:
                {
                    //Blood Pool Strike
                    yield return new WaitForSeconds(timeBeforeBloodStrike * timeModifier);
                    break;
                }

            default:
                goto case 0;
        }
    }
}
