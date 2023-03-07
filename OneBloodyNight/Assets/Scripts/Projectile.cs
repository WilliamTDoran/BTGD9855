using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView; //archer base speed 3500
using UnityEngine;

public class Projectile : Attack
{
    private Vector3 direction;

    /* Exposed Variables */
    [Tooltip("What would normally be called attacker")]
    [SerializeField]
    private GameActor shooter;
    /*~~~~~~~~~~~~~~~~~~~*/

    protected override void Update()
    {
        if (canMove)
        {
            transform.Translate(direction * Time.deltaTime);
        }
    }

    internal override void Fire()
    {
        PositionAttack();

        direction = new Vector3(0, -1, 0) * speed;

        canMove = true;
    }

    protected override void PositionAttack()
    {
        //Determines the angle the attack is facing, with 0 being screen-right, 90 being screen-down
        Quaternion facingAngleRotation = Quaternion.Euler(0, shooter.FacingAngle + 90f, 0); //Adding 90f is silly, but it works to align the numbers. Partially antiquated.

        //Extracts the attacker's current facing direction
        Vector3 v3Facing = facingAngleRotation * Vector3.forward;
        Vector3 attackerFacingDirection = new Vector3(v3Facing.x, v3Facing.z, 0);

        attackerFacingDirection.Normalize();

        //Positions the attack hitbox based on the gathered parameters
        transform.position = shooter.transform.position + (attackerFacingDirection * floatDistance);
        transform.rotation = Quaternion.Euler(90f, 0, shooter.FacingAngle + 90f);
    }

    internal override void OnHitWall()
    {
        base.OnHitWall();

        FinishFlight();
    }

    internal override void OnHitPlayer()
    {
        FinishFlight();
    }

    internal void FinishFlight()
    {
        EndSwing();
        canMove = false;
        gameObject.SetActive(false);
    }
}
