using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteAttack : Attack
{
    private Vector3 location;
    private int actualDamage;

    /* Exposed Variables */
    [Header("Remote Stuff")]
    [SerializeField]
    private float delay;

    [SerializeField]
    private bool centreWeighting;

    [SerializeField]
    private MeshRenderer debugTargetPreview;
    /*~~~~~~~~~~~~~~~~~~~*/

    internal void Target(Vector3 targetPoint)
    {
        this.location = targetPoint;
        debugTargetPreview.enabled = true;
    }







    protected override void PositionAttack() //gets it out of the way so it doesn't fuck things up
    {
        
    }
}
