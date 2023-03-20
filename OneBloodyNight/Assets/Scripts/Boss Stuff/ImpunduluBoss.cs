using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ImpunduluBoss : Boss
{
    /* Sound */
    public AudioSource audioSource;

    public AudioClip Dive;
    public AudioClip Lightning;
    public AudioClip feather;
    public AudioClip Flap;
    public AudioClip Intro;
    /*~~~~~~~*/

    private System.Random rnd;
    private int rndCap = 3;

    private IEnumerator spinAttackCoroutine;
    private IEnumerator diveAttackCoroutine;
    private IEnumerator randomBehaviorCoroutine;
    private IEnumerator randomAttackingCoroutine;
    private IEnumerator phaseCheckCoroutine;

    private Vector3 faceDirection;

    private int lightningCycle = 0;

    [Tooltip("Reference to all feather objects")]
    [SerializeField]
    private Projectile[] feathers;

    [Tooltip("Reference to all lightning objects")]
    [SerializeField]
    private RemoteAttack[] lightnings;

    [Tooltip("Time between individual feathers")]
    [SerializeField]
    private float timeBetweenFeathers = 0.6f;

    [Tooltip("Reference to the main camera, used for swoop positioning")]
    [SerializeField]
    private Camera cam;

    [Header("Attack Timings")]
    [SerializeField]
    private float timeBeforeFeathers;

    [SerializeField]
    private float timeBeforeLightning;

    [SerializeField]
    private float timeBeforeSwoop;

    [SerializeField]
    private float timeBeforeHoming;

    [SerializeField]
    private float timeBeforeBeam;

    [Header("Dive")]
    [Tooltip("The hitbox/attack for the swoop")]
    [SerializeField]
    private Attack swoopBox;

    [Tooltip("How long it takes the 'dulu to reach the side of the screen pre-swoop, in seconds")]
    [SerializeField]
    private float initialPositionTime = 1.0f;

    [Tooltip("How long the 'dulu holds to the side of the screen pre-swoop, in seconds")]
    [SerializeField]
    private float threatenTime = 0.75f;

    [Tooltip("How fast the 'dulu swoops")]
    [SerializeField]
    private float diveSpeed = 3000;

    [Tooltip("How long the 'dulu stands still after finishing the swoop, in seconds")]
    [SerializeField]
    private float stopTime = 0.4f;

    [Tooltip("How long the 'dulu takes to return to its starting position, in seconds")]
    [SerializeField]
    private float recoverTime = 0.3f;

    

    protected override void Start()
    {
        rnd = new System.Random();
        StartCoroutine(PhaseCheck());

        //audioSource.PlayOneShot(Intro,2f);
        base.Start();
        animator.SetTrigger("start");
        StartCoroutine("startanim");
        //StartRandomBehavior();
        //StartSpinAttack();
    }


    private IEnumerator startanim()
    {
        yield return new WaitForSeconds(5f);
        StartRandomBehavior();
        animator.ResetTrigger("start");

    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.AddForce(faceDirection, ForceMode.Force);
        }
    }

    private IEnumerator SpinAttack()
    {
        StopRandomBehavior();
        canMove = false;

        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(timeBetweenFeathers);

            facingAngle = -45f * i;
            feathers[i].gameObject.SetActive(true);
            audioSource.PlayOneShot(feather);
            feathers[i].Fire();
        }

        canMove = true;
        animator.ResetTrigger("spin");
        StartRandomBehavior();
    }

    private IEnumerator DiveAttack()
    {
        Vector3 returnPos = rb.position;
        canMove = false;
        StopRandomBehavior();

        bool leftStart = UnityEngine.Random.Range(0.0f, 1.0f) > 0.5f;

        float genTimer = 0.0f;

        if (!leftStart) { render.flipX = true; }

        animator.SetTrigger("swoop");
        while (genTimer <= initialPositionTime)
        {
            Vector3 cameraSidePoint = leftStart ? cam.ViewportToWorldPoint(new Vector3(0.05f, 0.52f, rb.position.z)) : cam.ViewportToWorldPoint(new Vector3(0.95f, 0.52f, rb.position.z));
            
            Vector3 targetPos = new Vector3(cameraSidePoint.x, rb.position.y, Player.plr.Rb.position.z);

            rb.position = Vector3.Lerp(returnPos, targetPos, genTimer / initialPositionTime);
            rb.position = new Vector3(rb.position.x, targetPos.y, rb.position.z);

            genTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        genTimer = 0.0f;

        while (genTimer <= threatenTime)
        {
            rb.position = leftStart ? cam.ViewportToWorldPoint(new Vector3(0.05f, 0.52f, rb.position.z)) : cam.ViewportToWorldPoint(new Vector3(0.95f, 0.52f, rb.position.z));

            genTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        genTimer = 0.0f;
        Vector3 cameraOffPoint = leftStart ? cam.ViewportToWorldPoint(new Vector3(1.1f, 0.52f, rb.position.z)) : cam.ViewportToWorldPoint(new Vector3(-0.1f, 0.52f, rb.position.z));
        swoopBox.gameObject.GetComponent<BoxCollider>().enabled = true;
        
        if (leftStart)
        {
            while (rb.position.x < cameraOffPoint.x)
            {
                rb.position += new Vector3(diveSpeed * Time.deltaTime, 0, 0);
                cameraOffPoint = cam.ViewportToWorldPoint(new Vector3(1.1f, 0.52f, rb.position.z));

                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (rb.position.x > cameraOffPoint.x)
            {
                rb.position -= new Vector3(diveSpeed * Time.deltaTime, 0, 0);
                cameraOffPoint = cam.ViewportToWorldPoint(new Vector3(-0.1f, 0.52f, rb.position.z));

                yield return new WaitForEndOfFrame();
            }
        }

        if (!leftStart) { render.flipX = false; }
        
        if (leftStart) { render.flipX = true; }

        yield return new WaitForSeconds(stopTime);
        Vector3 endPos = rb.position;
        swoopBox.gameObject.GetComponent<BoxCollider>().enabled = false;
        swoopBox.EndSwing();

        while (genTimer <= recoverTime)
        {
            rb.position = Vector3.Lerp(endPos, returnPos, genTimer / recoverTime);

            genTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        animator.ResetTrigger("spin");

        if (leftStart) { render.flipX = false; }

        StartRandomBehavior();
    }

    private IEnumerator RandomBehavior()
    {
        StartRandomAttacking();

        while (true)
        {
            animator.SetTrigger("walk");
            canMove = true;
            faceDirection = PickDirection() * speed;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.8f, 1.5f));
            canMove = false;
            rb.velocity = Vector3.zero;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.2f, 0.8f));
        }
    }

    private Vector3 PickDirection()
    {
        Vector3 direction = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
        direction.Normalize();
        return direction;
    }

    private IEnumerator RandomAttacking()
    {
        int check = rnd.Next(0, rndCap + currentPhase);

        switch (check)
        {
            case 0:
                yield return new WaitForSeconds(timeBeforeFeathers);

                StartSpinAttack();
                animator.SetTrigger("spin");
                
                break;

            case 1:
                yield return new WaitForSeconds(timeBeforeLightning);

                lightnings[lightningCycle].Initiate(Player.plr.Rb.position);

                lightningCycle = lightningCycle > 1 ? 0 : lightningCycle + 1;
                audioSource.PlayOneShot(Lightning);
                animator.ResetTrigger("spin");
                StopRandomBehavior();
                StartRandomBehavior();
                break;

            case 2:
                yield return new WaitForSeconds(timeBeforeSwoop);

                animator.ResetTrigger("spin");
                StartDiveAttack();
                break;

            case 3:
                break;

            case 4:
                break;

            default:
                yield return new WaitForSeconds(timeBeforeFeathers);

                StartSpinAttack();
                animator.SetTrigger("spin");

                break;
        }
    }

    private IEnumerator PhaseCheck()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            float healthPercent = (float)CurHitPoints / (float)MaxHitPoints;
            if (healthPercent <= phases[currentPhase + 1])
            {
                currentPhase++;
            }
        }
    }


    private void StartSpinAttack()
    {
        spinAttackCoroutine = SpinAttack();
        StartCoroutine(spinAttackCoroutine);
        
    }
    
    private void StopSpinAttack()
    {
        StopCoroutine(spinAttackCoroutine);
        spinAttackCoroutine = null;
        animator.SetTrigger("walk");
    }

    private void StartDiveAttack()
    {
        diveAttackCoroutine = DiveAttack();
        audioSource.PlayOneShot(Dive);
        StartCoroutine(diveAttackCoroutine);
    }

    private void StopDiveAttack()
    {
        StopCoroutine(diveAttackCoroutine);
        diveAttackCoroutine = null;
        animator.SetTrigger("walk");
    }

    private void StartRandomBehavior()
    {
        randomBehaviorCoroutine = RandomBehavior();
        StartCoroutine(randomBehaviorCoroutine);
    }

    private void StopRandomBehavior()
    {
        StopCoroutine(randomBehaviorCoroutine);
        randomBehaviorCoroutine = null;
    }

    private void StartRandomAttacking()
    {
        randomAttackingCoroutine = RandomAttacking();
        StartCoroutine(randomAttackingCoroutine);
    }

    private void StopRandomAttacking()
    {
        StopCoroutine(randomAttackingCoroutine);
        randomAttackingCoroutine = null;
    }

    private void StartPhaseCheck()
    {
        phaseCheckCoroutine = PhaseCheck();
        StartCoroutine(phaseCheckCoroutine);
    }

    private void StopPhaseCheck()
    {
        StopCoroutine(phaseCheckCoroutine);
        phaseCheckCoroutine = null;
    }
}
