using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the parent of all three player types, and holds the majority of functions shared by every type of character, including getting and interpreting control inputs
/// 
/// Version 1.0 (1/13/2023), Will Doran
/// </summary>
public class Player : GameActor
{
    /* Exposed Variables */
    private Transform renderBox;
    /*~~~~~~~~~~~~~~~~~~~*/

    /// <summary>
    /// Standard Start function. Initializes a couple values, creates references to useful objects. You know the drill.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        canMove = true;
        canAttack = true;
    }

    /// <summary>
    /// Standard FixedUpdate function. Drives movement. Note that despite the code appearing here, in execution order FixedUpdate takes place after Update
    /// </summary>
    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = controller.IntendedDirection * speed;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (controller.IntendedDirection != Vector3.zero && canMove)
        {
            facingAngle = Vector3.SignedAngle(Vector3.right, controller.IntendedDirection, Vector3.forward);
            facingAngle = facingAngle < 0 ? facingAngle + 360 : facingAngle;
        }

        if (facingDebugText != null)
        {
            facingDebugText.text = facingAngle + "";
        }
    }
}
