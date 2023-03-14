using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// All active GameObjects in scene inherit from this parent class, which holds a number of crucial variables that almost every actor needs
/// This facilitates easy access and communication between discrete objects, and avoids repetition of shared attributes among various entity types.
/// 
/// Version 1.0 (1/10/2023), Will Doran
/// Version 1.1 (2/8/2023),  Will Doran
/// Version 1.2 (2/10/2023), Will Doran
/// </summary>
public class GameActor : MonoBehaviour
{
    protected Animator animator;
    protected SpriteRenderer render;

    private float actualVelocity; //the real, measured velocity of the rigidbody
    private float clampedVelocity; //the velocity clamped to a max of 1, used for setting animation

    protected float facingAngle; //the direction the actor is 'facing' in degrees (has no inherent bearing on the object's actual transform rotation)
    public float FacingAngle { get { return facingAngle; } }

    protected Rigidbody rb; //the rigidbody attached the actor's gameObject
    public Rigidbody Rb { get { return rb; } }
    protected Collider col; //the collider attached to the actor's gameObject

    protected bool canMove; //whether the actor can (willingly) move
    public bool CanMove { get { return canMove; } set { canMove = value; } }

    protected bool canAttack; //whether the actor can initiate attacks
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    protected bool stunned; //whether the actor is affected by hitstun
    public bool Stunned { get { return stunned; } set { stunned = value; } }

    protected bool immune; //whether the actor can take damage
    public bool Immune { get { return immune; } set { immune = value; } }

    private int curHitPoints; //current hit points of the actor, if applicable. Only relevant for monsters and bosses
    public int CurHitPoints { get { return curHitPoints; } set { curHitPoints = value; } }

    protected bool dead = false;

    /* Exposed Variables */
    [Tooltip("Maximum movement speed - IRRELEVANT FOR NON-MOVING ENTITIES")]
    [SerializeField]
    protected float speed;

    [Tooltip("Duration of immmunity after taking damage - IRRELEVANT FOR NON-ATTACKABLE ENTITIES")]
    [SerializeField]
    protected float immuneDuration;
    public float ImmuneDuration { get { return immuneDuration; } }

    [Tooltip("The Monster's maximum possible hit points (also the hit points it spawns with)")]
    [SerializeField]
    private int maxHitPoints;
    public int MaxHitPoints { get { return maxHitPoints; } set { maxHitPoints = value; } }

    [Tooltip("Whether this GameActor has a walking animation")]
    [SerializeField]
    private bool walkAnim = false;
    /* -~-~-~-~-~-~-~-~- */

    /// <summary>
    /// Standard awake, getting references to key components
    /// </summary>
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        Debug.Assert(rb != null, "No rigidbody set on: " + gameObject.name);
        Debug.Assert(col != null, "No collider set on: " + gameObject.name);
    }

    /// <summary>
    /// Standard start. 
    /// Used to hold code related to getting control input, but after certain aspects of the monster code changed it no longer made sense to put it here
    /// </summary>
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        render = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Standard update. 
    /// Used to hold code related to getting control input, but after certain aspects of the monster code changed it no longer made sense to put it here. Now just a virtual.
    /// Arguably could be removed but I reason that at some point I might need it again and I don't want to go through and un-override all sub-class's Updates only to redo them later
    /// </summary>
    protected virtual void Update() {}

    /// <summary>
    /// Sets an animation parameter called speed which modifies a blend tree, driving the transition between idle and walk animations. Also flips left and right based on heading.
    /// </summary>
    private void LateUpdate()
    {
        if (walkAnim)
        {
            actualVelocity = rb.velocity.magnitude;
            clampedVelocity = actualVelocity > 1f ? 1f : actualVelocity;
            animator.SetFloat("Speed", clampedVelocity);

            //This is clumsy and causes awkward stuttering when moving vertically or near-vertically. Should ideally be replaced
            if (rb.velocity.magnitude > 0.1)
            {
                if (rb.velocity.x < 0)
                {
                    render.flipX = true;
                }
                else
                {
                    render.flipX = false;
                }
            }
        }
    }

    //These are basically just here serving the same function as an interface cause I don't want to double up on both virtual inherited classes and also interfaces
    internal virtual void OnAttackEnd(string code) {}

    internal virtual void OnSuccessfulAttack(string code) {}

    internal virtual void OnHitWall() {}

    internal virtual void OnReceiveHit() {}
}
