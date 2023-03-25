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

    protected override void Update()
    {
        base.Update();

        if (canMove)
        {
            transform.Translate(direction.normalized * Time.deltaTime * speed);
        }

        prevDistance = currentDistance;
        currentDistance = (targetPosition - transform.position).magnitude;

        if (currentDistance > prevDistance || currentDistance <= 1.0f)
        {
            Arrive();
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (RemoteCondition)
        {
            gameObject.SetActive(false);
        }
    }

    internal void Shoot()
    {
        targetPosition = Player.plr.transform.position;
        transform.position = Boss.instance.transform.position;

        direction = (targetPosition - transform.position).normalized;
        direction = new Vector3(direction.x, 0, direction.z).normalized;
        transform.position += direction * 2;

        canMove = true;
    }

    private void Arrive()
    {
        canMove = false;
        RemoteCondition = true;
    }
}
