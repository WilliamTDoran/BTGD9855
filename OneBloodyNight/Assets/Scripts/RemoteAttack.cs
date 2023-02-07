using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteAttack : Attack
{
    private IEnumerator fireCoroutine;

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
    /*~~~~~~~~~~~~~~~~~~~*/

    internal void Initiate(Vector3 targetPoint)
    {
        gameObject.SetActive(true);
        Target(targetPoint);
        StartFire();
    }

    private void Target(Vector3 targetPoint)
    {
        this.gameObject.transform.position = targetPoint;
        debugTargetPreview.enabled = true;
    }

    private IEnumerator Fire()
    {
        yield return new WaitForSeconds(delay);

        debugTargetPreview.enabled = false;
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
        gameObject.SetActive(false);
    }

    private void StartFire()
    {
        fireCoroutine = Fire();
        StartCoroutine(fireCoroutine);
    }

    private void StopFire()
    {
        StopCoroutine(fireCoroutine);
        fireCoroutine = null;
    }







    protected override void PositionAttack() //gets it out of the way so it doesn't fuck things up
    {
        
    }
}
