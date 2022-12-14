using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the parent of all three player types, and holds the majority of functions shared by every type of character, including getting and interpreting control inputs
/// 
/// Version 0.1 (1/10/2023), Will Doran
/// </summary>
public class Player : GameActor
{
    private GameObject controllerObj; //gameObject with the playercontroller script on it
    private PlayerController controller; //said playercontroller script

    private bool interactDown;
    private bool basicAttackDown;
    private bool advancedAttackDown;
    private bool load1Down;
    private bool load2Down;

    private float facingAngle;
    public float FacingAngle { get { return facingAngle; } }

    /* Exposed Variables */
    private Transform renderBox;
    /*~~~~~~~~~~~~~~~~~~~*/

    /// <summary>
    /// Standard Start function. Initializes a couple values, creates references to useful objects. You know the drill.
    /// </summary>
    private void Start()
    {
        canMove = true;
        canAttack = true;

        controllerObj = GameObject.Find("PlayerController");
        controller = controllerObj.GetComponent<PlayerController>();

        Debug.Assert(controller != null, "No controller set on: " + gameObject.name);
        Debug.Assert(rb != null, "No rigidbody set on: " + gameObject.name);
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

    private void Update()
    {
        interactDown = controller.InteractDown;
        basicAttackDown = controller.BasicFireDown;
        advancedAttackDown = controller.AdvancedFireDown;
        load1Down = controller.Load1Down;
        load2Down = controller.Load2Down;

        //This is a potentially deprecated script for handling the direction
        /*if (controller.IntendedDirection != Vector3.zero && canMove)
        {
            facingAngle = Vector2.SignedAngle(Vector2.right, controller.IntendedDirection);
            facingAngle = facingAngle < 0 ? facingAngle + 360 : facingAngle;
            spriteSwitch = (int)(facingAngle / 45f);
            render.transform.rotation = Quaternion.AngleAxis(45f * spriteSwitch, transform.forward);

            if (facingAngle >= 90 && facingAngle <= 270)
            {
                render.flipY = true;
            }
            else
            {
                render.flipY = false;
            }
        }*/
    }
}
