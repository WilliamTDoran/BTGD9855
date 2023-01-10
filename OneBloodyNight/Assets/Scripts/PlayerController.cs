using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

/// <summary>
/// This is a universal control interface that can be drawn from as needed, allowing for multiple characters to use the same controls.
/// This returns nothing; instead, the various public variables can be drawn from a script with reference to this
/// 
/// Version 1.0 (1/10/2023), Will Doran
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Vector3 intendedDirection; //A vector3 with 0'd z derived from left joystick or wasd
    public Vector3 IntendedDirection { get { return intendedDirection; } }

    private bool interactDown; //(A)/(cross) or f
    public bool InteractDown { get { return interactDown; } }

    private bool basicFireDown; //(X)/(square) or left click
    public bool BasicFireDown { get { return basicFireDown; } }

    private bool advancedFireDown; //(Y)/(triangle), (LT)/(L2), (RT)/(R2) or right click
    public bool AdvancedFireDown { get { return advancedFireDown; } }

    private bool load1Down; //(LB)/(L1) or q
    public bool Load1Down { get { return load1Down; } }

    private bool load2Down; //(RB)/(R1) or e
    public bool Load2Down { get { return load2Down; } }

    /// <summary>
    /// Update cycle that checks all axes every frame
    /// </summary>
    private void Update()
    {
        intendedDirection = CheckDirection();
        interactDown = CheckInteract();
        basicFireDown = CheckBasicFire();
        advancedFireDown = CheckAdvancedFire();
        load1Down = CheckLoad1();
        load2Down = CheckLoad2();
    }

    /// <summary>
    /// Gets an intended direction from the horizontal/vertical axes (controller left joystick, or wasd), and returns it as a Vector3 with 0'd z
    /// </summary>
    /// <returns>A Vector3 with intended direction stored in X,Y. Z is 0</returns>
    private Vector3 CheckDirection()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float yDirection = Input.GetAxis("Vertical");

        Vector3 newDirection = new Vector3(xDirection, yDirection, 0);

        if (newDirection.magnitude > 1) { newDirection.Normalize(); } //Normalizes only if needed

        return newDirection;
    }

    /// <summary>
    /// Checks for the interact button (controller A/cross, or f) being pressed, returning true if so
    /// </summary>
    /// <returns>True if interact is pressed, false otherwise</returns>
    private bool CheckInteract()
    {
        bool down = Input.GetButtonDown("Interact");

        return down;
    }

    /// <summary>
    /// Checks for the basic fire button (controller X/square, or left mouse click) being pressed, returning true if so
    /// </summary>
    /// <returns>True if basic fire is pressed, false otherwise</returns>
    private bool CheckBasicFire()
    {
        bool down = Input.GetButtonDown("BasicFire");

        return down;
    }

    /// <summary>
    /// Checks for the advanced fire button (controller Y/triangle or LT/L2 or RT/R2, or right mouse click) being pressed, returning true if so
    /// </summary>
    /// <returns>True if advanced fire is pressed, false otherwise</returns>
    private bool CheckAdvancedFire()
    {
        bool down = Input.GetButtonDown("AdvancedFire");

        return down;
    }

    /// <summary>
    /// Checks for the load 1 button (controller LB/L1, or q) being pressed, returning true if so
    /// </summary>
    /// <returns>True if load is pressed, false otherwise</returns>
    private bool CheckLoad1()
    {
        bool down = Input.GetButtonDown("Load1");

        return down;
    }

    /// <summary>
    /// Checks for the load 2 button (controller RB/R1, or e) being pressed, returning true if so
    /// </summary>
    /// <returns>True if load 2 is pressed, false otherwise</returns>
    private bool CheckLoad2()
    {
        bool down = Input.GetButtonDown("Load2");

        return down;
    }
}
