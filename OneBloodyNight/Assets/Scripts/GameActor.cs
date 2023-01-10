using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActor : MonoBehaviour
{
    protected Rigidbody rb;
    protected Collider col;

    protected bool canMove;
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    protected bool canAttack;
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }
    protected bool immune;
    public bool Immune { get { return immune; } set { immune = value; } }

    /* Exposed Variables */
    [Tooltip("Maximum movement speed")]
    [SerializeField]
    protected float speed;

    [Tooltip("Duration of immmunity after taking damage")]
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
