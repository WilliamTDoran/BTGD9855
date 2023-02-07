using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Attack
{
    private Vector3 direction;

    protected override void Update()
    {
        if (canMove)
        {
            transform.Translate(direction * Time.deltaTime);
        }
    }

    internal void Fire()
    {
        PositionAttack();

        direction = new Vector3(0, 0, 1) * speed;

        canMove = true;
    }

    internal override void OnHitWall()
    {
        base.OnHitWall();

        FinishFlight();
    }

    internal void FinishFlight()
    {
        canMove = false;
        gameObject.SetActive(false);
    }
}
