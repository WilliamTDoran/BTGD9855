using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteAttack : Attack
{
    private IEnumerator fireCoroutine;
    private IEnumerator fireConditionalCoroutine;

    private Vector3 location;
    private int actualDamage;

    /* Exposed Variables */
    [Header("Remote Stuff")]
    [SerializeField]
    private float delay;

    [SerializeField]
    private float activeTime = 0.3f;

    [SerializeField]
    private MeshRenderer debugTargetPreview;

    [Tooltip("Any monobehaviours that contain variables needed for conditional remote")]
    [SerializeField]
    private GameActor[] conditionals;

    [Header("Yara Boss gurgey only—anything else can and must ignore this")]
    [SerializeField]
    private bool yaraling = false;

    [SerializeField]
    private YaraBoss yara;
    /*~~~~~~~~~~~~~~~~~~~*/

    internal void Initiate(Vector3 targetPoint)
    {
        gameObject.SetActive(true);
        Target(targetPoint);
        StartFire();
    }

    internal void InitiateConditional(Vector3 targetPoint)
    {
        gameObject.SetActive(true);
        Target(targetPoint);
        StartFireConditional();
    }

    private void Target(Vector3 targetPoint)
    {
        this.gameObject.transform.position = targetPoint + new Vector3(0,0,0.0001f);
        if (debugTargetPreview != null)
        {
            debugTargetPreview.enabled = true;
        }
    }

    private IEnumerator FireMe()
    {
        yield return new WaitForSeconds(delay);

        if (debugTargetPreview != null)
        {
            debugTargetPreview.enabled = false;
        }
        col.enabled = true;
        if (showHitbox)
        {
            hitboxMesh.enabled = true;
        }

        yield return new WaitForSeconds(activeTime);

        col.enabled = false;
        if (showHitbox)
        {
            hitboxMesh.enabled = false;
        }
        EndSwing();
        gameObject.SetActive(false);
    }

    private IEnumerator FireMeConditional()
    {
        yield return new WaitUntil(() => conditionals[0].RemoteCondition);

        if (debugTargetPreview != null)
        {
            debugTargetPreview.enabled = false;
        }
        conditionals[0].RemoteCondition = false;
        col.enabled = true;
        if (showHitbox)
        {
            hitboxMesh.enabled = true;
        }

        yield return new WaitUntil(() => conditionals[1].RemoteCondition);

        conditionals[1].RemoteCondition = false;
        col.enabled = false;
        if (showHitbox)
        {
            hitboxMesh.enabled = false;
        }
        EndSwing();
        gameObject.SetActive(false);
    }

    internal override void OnHitPlayer()
    {
        if (yaraling)
        {
            yara.SpawnYaraling();
        }
    }




    private void StartFire()
    {
        fireCoroutine = FireMe();
        StartCoroutine(fireCoroutine);
    }

    private void StopFire()
    {
        StopCoroutine(fireCoroutine);
        fireCoroutine = null;
    }

    private void StartFireConditional()
    {
        fireConditionalCoroutine = FireMeConditional();
        StartCoroutine(fireConditionalCoroutine);
    }

    private void StopFireConditional()
    {
        StopCoroutine(fireConditionalCoroutine);
        fireConditionalCoroutine = null;
    }




    protected override void PositionAttack() //gets it out of the way so it doesn't fuck things up
    {
        
    }
}
