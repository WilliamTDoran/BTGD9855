using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 cursorHotspot;

    private Vector2 intendedDirection;
    public Vector2 IntendedDirection { get { return intendedDirection; } }

    private bool primaryFireDown;
    public bool PrimaryFireDown { get { return primaryFireDown; } }

    private bool secondaryFireDown;
    public bool SecondaryFireDown { get { return secondaryFireDown; } }

    private void Update()
    {
        intendedDirection = CheckDirection();
        primaryFireDown = CheckPrimaryFire();
        secondaryFireDown = CheckSecondaryFire();
    }

    private Vector3 CheckDirection()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float yDirection = Input.GetAxis("Vertical");

        Vector2 newDirection = new Vector3(xDirection, yDirection);

        if (newDirection.magnitude > 1) { newDirection.Normalize(); } //Normalizes only if needed

        return newDirection;
    }

    private bool CheckPrimaryFire()
    {
        bool fire = Input.GetButtonDown("Fire1");

        return fire;
    }

    private bool CheckSecondaryFire()
    {
        bool fire = Input.GetButtonDown("Fire2");

        return fire;
    }
}
