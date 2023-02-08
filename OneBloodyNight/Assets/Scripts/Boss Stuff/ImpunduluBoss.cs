using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ImpunduluBoss : Boss
{
    private IEnumerator spinAttackCoroutine;
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

    protected override void Start()
    {
        base.Start();

        StartRandomBehavior();
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
            feathers[i].Fire();
        }

        canMove = true;
        StartRandomBehavior();
    }

    private IEnumerator RandomBehavior()
    {
        StartRandomAttacking();

        while (true)
        {
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
        Vector3 direction = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), 0);
        direction.Normalize();
        return direction;
    }

    private IEnumerator RandomAttacking()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(1.5f, 4.0f));

        if (UnityEngine.Random.Range(-1.0f,1.0f) < 0)
        {
            StartSpinAttack();
        }
        else
        {
            lightnings[lightningCycle].Initiate(Player.plr.Rb.position);

            lightningCycle = lightningCycle > 2 ? 0 : lightningCycle + 1;

            StopRandomBehavior();
            StartRandomBehavior();
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
