using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class drives the Strigoi character. 
/// It has a basic attack which is a quick slash,
/// an activated ability which is invisibility,
/// and a passive effect which is an increasing power as it fights.
/// </summary>
public class PlayerStrigoi : Player
{
    /* Exposed Variables */
    [Tooltip("A reference to the strigoi's basic attack object")]
    [SerializeField]
    private Attack basicAttack;
    /*~~~~~~~~~~~~~~~~~~~*/
}
