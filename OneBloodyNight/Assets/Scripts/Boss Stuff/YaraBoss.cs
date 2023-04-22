using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// This script drives the Yara-ma-yha-who boss fight.
/// 
/// Version 1.0 (3/24/23), Will Doran
/// </summary>
public class YaraBoss : Boss
{
    /* Sound */
    public AudioSource audioSource;

    public AudioClip Hurt;
    public AudioClip slam;
    public AudioClip Gurg;
    public AudioClip poundCity;
    public AudioClip Death;
    /*~~~~~~~*/

    private IEnumerator shockwaveSlamCoroutine;
    private IEnumerator gurgeyCoroutine;
    private IEnumerator groundPoundCoroutine;
    private IEnumerator poundRockslideCoroutine;
    private IEnumerator handSwipeCoroutine;

    private bool shockSlamRunning = false;
    public bool ShockSlamRunning { set { shockSlamRunning = value; } }
    private bool gurgeyRunning = false;
    public bool GurgeyRunning { set { gurgeyRunning = value; } }
    private bool groundPoundRunning = false;
    public bool GroundPoundRunning { set { groundPoundRunning = value; } }
    private bool handSwipeRunning = false;
    public bool HandSwipeRunning { set { handSwipeRunning = value; } }

    private bool poundBegin = false;
    public bool PoundBegin { set { poundBegin = value; } }
    private bool poundDive = false;
    public bool PoundDive { set { poundDive = value; } }

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

    [Tooltip("Speed of Groudn Pound Dive")]
    [SerializeField]
    private float diveSpeed;

    [Tooltip("Reference to the attack made when hitting the ground after GP")]
    [SerializeField]
    private Attack groundPoundImpact;

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
    private float timeBeforeHandSwipe;

    private bool isDead;
    /*~~~~~~~~~~~~~~~~~~~*/

    protected override void Start()
    {
        base.Start();
        
        StartCoroutine("startanim");
        isDead = false;
    }
    protected override void Update()
    {
        base.Update();

        if ((CurHitPoints <= 0) && (isDead == false))
        {
            isDead = true;
            PlayerPrefs.SetInt("Yara", 0);
            StopAllCoroutines();
            StartCoroutine("Ded");

        }

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
    private IEnumerator starter()
    {
        yield return new WaitForSeconds(5f);
    }

    private IEnumerator startanim()
    {
        Player.plr.Stunned = true;
        yield return new WaitForSeconds(7f);
        StartRandomBehavior();
        Player.plr.Stunned = false;
    }

    internal override void OnReceiveHit()
    {
        base.OnReceiveHit();
        spanimator.SetTrigger("Owie");
        audioSource.PlayOneShot(Hurt);
    }

    protected override IEnumerator RandomAttacking()
    {
        spanimator.ResetTrigger("Owie");
        int upperLimit = rndCap + currentPhase;
        int check = rnd.Next(0, upperLimit);

        if (GameManager.instance.TimeInSauce >= tooLong)
        {
            check = 1;
        }

        switch (check)
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
                    //Ground Pound
                    yield return new WaitForSeconds(timeBeforePound * timeModifier);

                    if (!Player.plr.Visible)
                    {
                        StopRandomBehavior();
                        StartRandomBehavior();
                        break;
                    }

                    Vector3 playerDirection = (Player.plr.transform.position - transform.position).normalized;
                    playerDirection = new Vector3(playerDirection.x, 0, playerDirection.z).normalized;
                    if (System.Math.Abs(playerDirection.x) < System.Math.Abs(playerDirection.z))
                    {
                        goto case 2;
                    }

                    StartGroundPound();
                    break;
                }

            case 2:
                {
                    //Regurgitate
                    yield return new WaitForSeconds(timeBeforeRegurgitate * timeModifier);

                    if (!Player.plr.Visible)
                    {
                        StopRandomBehavior();
                        StartRandomBehavior();
                        break;
                    }

                    StartGurgey();
                    break;
                }

            case 3:
                {
                    //Shockwave Slam
                    yield return new WaitForSeconds(timeBeforeShockwave * timeModifier);

                    if (!Player.plr.Visible)
                    {
                        StopRandomBehavior();
                        StartRandomBehavior();
                        break;
                    }

                    StartShockwaveSlam();
                    break;
                }

            case 4:
                {
                    //Hand Swipe
                    yield return new WaitForSeconds(timeBeforeHandSwipe * timeModifier);

                    if (!Player.plr.Visible)
                    {
                        StopRandomBehavior();
                        StartRandomBehavior();
                        break;
                    }

                    StartHandSwipe();
                    break;
                }

            default:
                Debug.LogError("Yara defaulted on attack choice");
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
        audioSource.PlayOneShot(slam);

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
        gurgeyRunning = true;

        gurgectile.RemoteCondition = false;

        gurgeySpot.gameObject.SetActive(true);
        gurgeySpot.InitiateConditional(Player.plr.Rb.position);
        ResetAnimation(gurgeyAnimator);
        
        spanimator.SetTrigger("Gurgey");
        audioSource.PlayOneShot(Gurg);

        yield return new WaitUntil(() => !gurgeyRunning);

        canMove = true;
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

    private IEnumerator GroundPound()
    {
        StopRandomBehavior();
        canMove = false;
        immune = true;
        groundPoundRunning = true;

        col.enabled = false;

        FacePlayer();
        spanimator.SetTrigger("PoundTown");

        yield return new WaitUntil(() => poundBegin);
        poundBegin = false;

        while (!poundDive)
        {
            transform.Translate(new Vector3(0, 0, 35 * Time.deltaTime));
            yield return new WaitForEndOfFrame();
        }
        poundDive = false;

        Vector3 targetPosition = Player.plr.transform.position;
        Vector3 startPosition = transform.position;
        float targetDistance = Vector3.Distance(startPosition, targetPosition);
        Vector3 targetVector = (targetPosition - startPosition).normalized;
        targetVector = new Vector3(targetVector.x, 0, targetVector.z).normalized;

        FacePlayer();

        while (Vector3.Distance(startPosition, transform.position) < targetDistance)
        {
            transform.Translate(targetVector * diveSpeed * Time.deltaTime / timeModifier);
            yield return new WaitForEndOfFrame();
        }

        immune = false;
        transform.position = targetPosition;

        spanimator.SetTrigger("PoundCity");
        audioSource.PlayOneShot(poundCity);
        GroundPoundHit();

        yield return new WaitForEndOfFrame();
        col.enabled = true;

        yield return new WaitUntil(() => !groundPoundRunning);
        groundPoundRunning = false;

        groundPoundImpact.Col.enabled = false;
        groundPoundImpact.EndSwing();

        canMove = true;
        StartRandomBehavior();
    }

    private void GroundPoundHit()
    {
        groundPoundImpact.Col.enabled = true;

        int targetPattern = rnd.Next(0, rockBaskets.Length);
        rockBaskets[targetPattern].transform.position = transform.position;

        StartPoundRockslide(rockBaskets[targetPattern], poundRockMinTimes[targetPattern], poundRockMaxTimes[targetPattern]);
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

    private IEnumerator HandSwipe()
    {
        StopRandomBehavior();
        canMove = false;
        handSwipeRunning = true;

        spanimator.SetTrigger("SwiperNoSwiping");

        yield return new WaitUntil(() => !handSwipeRunning);

        canMove = true;
        StartRandomBehavior();
    }

    /// <summary>
    /// I have no idea what this actually *does*, I just know that if you apply it to an animator that's hit its exit, it'll restart it lmao
    /// </summary>
    /// <param name="inp">The animator to restart</param>
    private void ResetAnimation(Animator inp)
    {
        inp.Rebind();
        inp.Update(0f);
    }

    private void FacePlayer()
    {
        if (Player.plr.transform.position.x > transform.position.x)
        {
            sprender.flipX = false;
        }
        else
        {
            sprender.flipX = true;
        }
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

    private void StartGroundPound()
    {
        groundPoundCoroutine = GroundPound();
        StartCoroutine(groundPoundCoroutine);
    }

    private void StopGroundPound()
    {
        StopCoroutine(groundPoundCoroutine);
        groundPoundCoroutine = null;
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

    private void StartHandSwipe()
    {
        handSwipeCoroutine = HandSwipe();
        StartCoroutine(handSwipeCoroutine);
    }

    private void StopHandSwip()
    {
        StopCoroutine(handSwipeCoroutine);
        handSwipeCoroutine = null;
    }

    private IEnumerator Ded()
    {
        spanimator.SetTrigger("Die");
        
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}
