using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GurgeyProjectile : GameActor
{
    private Vector3 direction;
    private Vector3 targetPosition;
    private float prevDistance = 1000;
    private float currentDistance = 1000;

    /* Exposed Variables */
    [SerializeField]
    private Animator spotAnimator;

    [SerializeField]
    private RemoteAttack spot;
    /*~~~~~~~~~~~~~~~~~~~*/

    protected override void Update()
    {
        base.Update();

        if (canMove)
        {
            transform.Translate(direction.normalized * Time.deltaTime * speed);
        }

        prevDistance = currentDistance;

        Vector3 yFlattenedTarget = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        currentDistance = (yFlattenedTarget - transform.position).magnitude;

        if (currentDistance > prevDistance || currentDistance <= 2.0f)
        {
            Arrive();
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (RemoteCondition)
        {
            RemoteCondition = false;
            gameObject.SetActive(false);
        }
    }

    internal void Shoot()
    {
        RemoteCondition = false;
        targetPosition = spot.transform.position;
        transform.position = Boss.instance.transform.position;

        direction = (targetPosition - transform.position).normalized;
        direction = new Vector3(direction.x, 0, direction.z).normalized;
        transform.position += direction * 2;

        prevDistance = 1000;
        currentDistance = 1000;

        canMove = true;
    }

    private void Arrive()
    {
        canMove = false;
        spotAnimator.SetTrigger("Arrive");
        RemoteCondition = true;
    }
}
