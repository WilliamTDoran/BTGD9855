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

    private IEnumerator spinAttackCoroutine;
    private IEnumerator diveAttackCoroutine;
    private IEnumerator randomBehaviorCoroutine;
    private IEnumerator randomAttackingCoroutine;

    private Vector3 faceDirection;

    private int lightningCycle = 0;

    [SerializeField]
    private Projectile[] feathers;

    [SerializeField]
    private RemoteAttack[] lightnings;

    [SerializeField]
    private float timeBetweenFeathers = 0.6f;

    [SerializeField]
    private Camera cam;

    [Header("Dive")]
    [SerializeField]
    private Attack swoopBox;

    [SerializeField]
    private float initialPositionTime = 1.0f;

    [SerializeField]
    private float threatenTime = 0.75f;

    [SerializeField]
    private float diveSpeed = 3000;

    [SerializeField]
    private float stopTime = 0.4f;

    [SerializeField]
    private float recoverTime = 0.3f;

    

    protected override void Start()
    {
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

        float genTimer = 0.0f;

        animator.SetTrigger("swoop");
        while (genTimer <= initialPositionTime)
        {
            Vector3 cameraRightsidePoint = cam.ViewportToWorldPoint(new Vector3(0.95f, 0.52f, rb.position.z));

            Vector3 targetPos = new Vector3(cameraRightsidePoint.x, rb.position.y, Player.plr.Rb.position.z);

            rb.position = Vector3.Lerp(returnPos, targetPos, genTimer / initialPositionTime);
            rb.position = new Vector3(rb.position.x, targetPos.y, rb.position.z);

            genTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        genTimer = 0.0f;

        while (genTimer <= threatenTime)
        {
            rb.position = cam.ViewportToWorldPoint(new Vector3(0.95f, 0.52f, rb.position.z));

            genTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        genTimer = 0.0f;
        Vector3 cameraOffPoint = cam.ViewportToWorldPoint(new Vector3(-0.1f, 0.52f, rb.position.z));
        swoopBox.gameObject.GetComponent<BoxCollider>().enabled = true;

        while (rb.position.x > cameraOffPoint.x)
        {
            rb.position -= new Vector3(diveSpeed * Time.deltaTime, 0, 0);
            cameraOffPoint = cam.ViewportToWorldPoint(new Vector3(-0.1f, 0.52f, rb.position.z));

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(stopTime);
        Vector3 endPos = rb.position;
        swoopBox.gameObject.GetComponent<BoxCollider>().enabled = false;

        while (genTimer <= recoverTime)
        {
            rb.position = Vector3.Lerp(endPos, returnPos, genTimer / recoverTime);

            genTimer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        animator.ResetTrigger("spin");
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
        yield return new WaitForSeconds(UnityEngine.Random.Range(1.5f, 4.0f));

        int check = (int)UnityEngine.Random.Range(0, 2.99999999f);

        switch (check)
        {
            case 0:
                StartSpinAttack();
                animator.SetTrigger("spin");
                
                break;
            case 1:
                lightnings[lightningCycle].Initiate(Player.plr.Rb.position);

                lightningCycle = lightningCycle > 1 ? 0 : lightningCycle + 1;
                audioSource.PlayOneShot(Lightning);
                animator.ResetTrigger("spin");
                StopRandomBehavior();
                StartRandomBehavior();
                break;
            case 2:
                animator.ResetTrigger("spin");
                StartDiveAttack();
                break;
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
}
