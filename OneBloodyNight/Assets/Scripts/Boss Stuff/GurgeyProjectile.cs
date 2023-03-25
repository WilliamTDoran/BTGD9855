using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GurgeyProjectile : GameActor
{
    private Vector3 direction;
    private Vector3 targetPosition;
    private float targetDistance;

    protected override void Start()
    {
        base.Start();

        canMove = false;
    }

    protected override void Update()
    {
        base.Update();

        if (canMove)
        {
            transform.Translate(direction.normalized * Time.deltaTime * speed);
        }

        if ((targetPosition - rb.position).magnitude >= direction.magnitude)
        {
            Arrive();
        }
    }

    internal void Shoot()
    {
        targetPosition = Player.plr.Rb.position;
        rb.position = Boss.instance.Rb.position;

        direction = targetPosition - rb.position;
        targetDistance = direction.magnitude;

        canMove = true;
    }

    private void Arrive()
    {
        canMove = false;
        RemoteCondition = true;
        gameObject.SetActive(false);
    }
}
