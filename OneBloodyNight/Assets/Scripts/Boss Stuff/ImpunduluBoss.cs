using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ImpunduluBoss : Boss
{
    private IEnumerator spinAttackCoroutine;
    private IEnumerator randomBehaviorCoroutine;

    [SerializeField]
    private Projectile[] feathers;

    [SerializeField]
    private float timeBetweenFeathers = 0.6f;

    protected override void Start()
    {
        base.Start();

        StartSpinAttack();
    }

    private IEnumerator SpinAttack()
    {
        for (int i = 0; i < 8; i++)
        {
            yield return new WaitForSeconds(timeBetweenFeathers);

            facingAngle = -45f * i;
            feathers[i].gameObject.SetActive(true);
            feathers[i].Fire();
        }
    }

    private IEnumerator RandomBehavior()
    {
        throw new NotImplementedException();
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
}
