using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// All active gameObjects in scene inherit from this parent class, which holds a number of crucial variables that almost every attribute needs
/// This facilitates easy access and communication between discrete objects, and avoids repetition of shared attributes various entity types.
/// 
/// Version 1.0 (1/10/2023), Will Doran
/// </summary>
public class GameActor : MonoBehaviour
{
    protected Rigidbody rb;
    protected Collider col;

    protected bool canMove; //whether the actor can (willingly) move
    public bool CanMove { get { return canMove; } set { canMove = value; } }

    protected bool canAttack; //whether the actor can initiate attacks
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    protected bool immune; //whether the actor can take damage
    public bool Immune { get { return immune; } set { immune = value; } }

    /* Exposed Variables */
    [Tooltip("Maximum movement speed - IRRELEVANT FOR NON-MOVING ENTITIES")]
    [SerializeField]
    protected float speed;

    [Tooltip("Duration of immmunity after taking damage - IRRELEVANT FOR NON-ATTACKABLE ENTITIES")]
    [SerializeField]
    protected float immuneDuration;
    public float ImmuneDuration { get { return immuneDuration; } }
    /* -~-~-~-~-~-~-~-~- */

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        Debug.Assert(rb != null, "No rigidbody set on: " + gameObject.name);
        Debug.Assert(col != null, "No collider set on: " + gameObject.name);
    }
}
