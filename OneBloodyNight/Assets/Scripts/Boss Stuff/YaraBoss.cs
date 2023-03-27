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
    private IEnumerator shockwaveSlamCoroutine;
    private IEnumerator gurgeyCoroutine;
    private IEnumerator poundRockslideCoroutine;

    private bool shockSlamRunning = false;
    public bool ShockSlamRunning { set { shockSlamRunning = value; } }

    private bool gurgeyRunning = false;
    public bool GurgeyRunning { set { gurgeyRunning = value; } }

    /* Exposed Variables */
    [Tooltip("All 8 rocks for shockwave slam")]
    [SerializeField]
    private Shockrock[] shockrocks;

    [Tooltip("Reference to the regurgitate projectile")]
    [SerializeField]
    private GurgeyProjectile gurgectile;

    [Tooltip("Reference to the regurgitate spot")]
    [SerializeField]
    private RemoteAttack gurgeySpot;

    [Tooltip("Reference to the regurgitate spot sprite animator (easier to do it all in one place)")]
    [SerializeField]
    private Animator gurgeyAnimator;

    [Tooltip("As many yaralings as you want to be able to exist at once")]
    [SerializeField]
    private Yaraling[] lings;

    [Tooltip("The rock basket patterns for groundpound. MUST BE THE SAME NUMBER OF ENTRIES AS THERE ARE MAXTIMES AND ROCKBASKETS")]
    [SerializeField]
    private GameObject[] rockBaskets;

    [Tooltip("The minimum time possible between each rock's appearance during ground pound. MUST BE THE SAME NUMBER OF ENTRIES AS THERE ARE MAXTIMES AND ROCKBASKETS")]
    [SerializeField]
    private float[] poundRockMinTimes;

    [Tooltip("The maximum time possible between each rock's appearance during ground pound. MUST BE THE SAME NUMBER OF ENTRIES AS THERE ARE MINTIMES AND ROCKBASKETS")]
    [SerializeField]
    private float[] poundRockMaxTimes;

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

    [Tooltip("This one should be high, comparatively")]
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

    internal override void OnReceiveHit()
    {
        base.OnReceiveHit();
        spanimator.SetTrigger("Owie");
    }

    protected override IEnumerator RandomAttacking()
    {
        int upperLimit = rndCap + currentPhase;
        int check = rnd.Next(0, upperLimit);

        switch (3)
        {
            case 0:
                {
                    goto case 1;
                    //Boosted basic, I've been told to ignore this
                    yield return new WaitForSeconds(timeBeforeBasic * timeModifier);
                    break;
                }

            case 1:
                {
                    //Shockwave Slam
                    yield return new WaitForSeconds(timeBeforeShockwave * timeModifier);
                    StartShockwaveSlam();
                    break;
                }

            case 2:
                {
                    //Regurgitate
                    yield return new WaitForSeconds(timeBeforeRegurgitate * timeModifier);
                    StartGurgey();
                    break;
                }

            case 3:
                {
                    //Ground Pound
                    yield return new WaitForSeconds(timeBeforePound * timeModifier);
                    StopRandomBehavior();
                    StartPoundRockslide(rockBaskets[0], poundRockMinTimes[0], poundRockMaxTimes[0]);
                    StartRandomBehavior();
                    break;
                }

            case 4:
                {
                    goto case 1;
                    //Blood Pool Strike
                    yield return new WaitForSeconds(timeBeforeBloodStrike * timeModifier);
                    break;
                }

            default:
                goto case 0;
        }
    }

    private IEnumerator ShockwaveSlam()
    {
        StopRandomBehavior();
        canMove = false;
        immune = true;
        shockSlamRunning = true;

        spanimator.SetTrigger("ShockemRockem");

        yield return new WaitUntil(() => !shockSlamRunning);

        canMove = true;
        immune = false;
        StartRandomBehavior();
    }

    internal void ShockwaveFire()
    {
        for (int i = 0; i < 8; i++)
        {
            facingAngle = -45f * i;
            shockrocks[i].gameObject.SetActive(true);
            shockrocks[i].Fire(i, timeModifier);
        }
    }

    private IEnumerator Regurgitate()
    {
        StopRandomBehavior();
        canMove = false;
        immune = true;
        gurgeyRunning = true;

        gurgectile.RemoteCondition = false;

        gurgeySpot.gameObject.SetActive(true);
        gurgeySpot.InitiateConditional(Player.plr.Rb.position);
        ResetAnimation(gurgeyAnimator);
        
        spanimator.SetTrigger("Gurgey");

        yield return new WaitUntil(() => !gurgeyRunning);

        canMove = true;
        immune = false;
        StartRandomBehavior();
    }

    internal void SpawnYaraling()
    {
        int want = rnd.Next(0, 3);
        int tried = 0;

        switch (want)
        {
            case 0:
                if (tried == 3)
                {
                    goto default;
                }

                if (!lings[0].gameObject.activeInHierarchy)
                {
                    Vector3 spawnPoint = new Vector3(gurgeySpot.transform.position.x, lings[0].transform.position.y, gurgeySpot.transform.position.z);

                    lings[0].gameObject.SetActive(true);
                    lings[0].YaralingSpawn(spawnPoint);
                }
                else
                {
                    tried++;
                    goto case 1;
                }
                break;

            case 1:
                if (tried == 3)
                {
                    goto default;
                }

                if (!lings[1].gameObject.activeInHierarchy)
                {
                    Vector3 spawnPoint = new Vector3(gurgeySpot.transform.position.x, lings[1].transform.position.y, gurgeySpot.transform.position.z);

                    lings[1].gameObject.SetActive(true);
                    lings[1].YaralingSpawn(spawnPoint);
                }
                else
                {
                    tried++;
                    goto case 2;
                }
                break;

            case 2:
                if (tried == 3)
                {
                    goto default;
                }

                if (!lings[2].gameObject.activeInHierarchy)
                {
                    Vector3 spawnPoint = new Vector3(gurgeySpot.transform.position.x, lings[2].transform.position.y, gurgeySpot.transform.position.z);

                    lings[2].gameObject.SetActive(true);
                    lings[2].YaralingSpawn(spawnPoint);
                }
                else
                {
                    tried++;
                    goto case 0;
                }
                break;

            default: break;
        }
    }

    private IEnumerator PoundRockslide(GameObject center, float minTime, float maxTime)
    {
        Attack[] rockies = center.GetComponentsInChildren<Attack>(true);

        foreach (Attack i in rockies)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime) * timeModifier);

            i.gameObject.SetActive(true);

            Animator iAnim = i.transform.GetChild(0).GetComponent<Animator>();
            ResetAnimation(iAnim);
        }
    }

    private void ResetAnimation(Animator inp)
    {
        inp.Rebind();
        inp.Update(0f);
    }




    private void StartShockwaveSlam()
    {
        shockwaveSlamCoroutine = ShockwaveSlam();
        StartCoroutine(shockwaveSlamCoroutine);
    }

    private void StopShockwaveSlam()
    {
        StopCoroutine(shockwaveSlamCoroutine);
        shockwaveSlamCoroutine = null;
    }

    private void StartGurgey()
    {
        gurgeyCoroutine = Regurgitate();
        StartCoroutine(gurgeyCoroutine);
    }

    private void StopGurgey()
    {
        StopCoroutine(gurgeyCoroutine);
        gurgeyCoroutine = null;
    }

    private void StartPoundRockslide(GameObject center, float minTime, float maxTime)
    {
        poundRockslideCoroutine = PoundRockslide(center, minTime, maxTime);
        StartCoroutine(poundRockslideCoroutine);
    }

    private void StopPoundRockslide()
    {
        StopCoroutine(poundRockslideCoroutine);
        poundRockslideCoroutine = null;
    }
}
