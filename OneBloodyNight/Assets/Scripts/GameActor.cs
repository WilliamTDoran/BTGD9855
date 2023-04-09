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
    private bool remoteCondition = false;
    public bool RemoteCondition { get { return remoteCondition; } set { remoteCondition = value; } }

    private IEnumerator immuneCoroutine; //drives i-frames
    private IEnumerator hitstunCoroutine; //drives hitstun

    protected Animator animator;
    protected SpriteRenderer render;

    protected float actualVelocity; //the real, measured velocity of the rigidbody
    protected float clampedVelocity; //the velocity clamped to a max of 1, used for setting animation
    protected bool facingOverride = false; //just don't worry about it.

    protected float facingAngle; //the direction the actor is 'facing' in degrees (has no inherent bearing on the object's actual transform rotation)
    public float FacingAngle { get { return facingAngle; } }

    protected Rigidbody rb; //the rigidbody attached the actor's gameObject
    public Rigidbody Rb { get { return rb; } }
    protected Collider col; //the collider attached to the actor's gameObject
    public Collider Col { get { return col; } }

    protected bool canMove; //whether the actor can (willingly) move
    public bool CanMove { get { return canMove; } set { canMove = value; } }

    protected bool canAttack; //whether the actor can initiate attacks
    public bool CanAttack { get { return canAttack; } set { canAttack = value; } }

    protected bool stunned; //whether the actor is affected by hitstun
    public bool Stunned { get { return stunned; } set { stunned = value; } }

    protected bool immune = false; //whether the actor can take damage
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
    protected bool walkAnim = false;

    [Tooltip("Reference to the behind-sprite part of the buffing aura")]
    [SerializeField]
    internal SpriteRenderer backBuffAura;

    [Tooltip("Reference to the afore-sprite part of the buffing aura")]
    [SerializeField]
    internal SpriteRenderer frontBuffAura;
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
    protected virtual void LateUpdate()
    {
        if (walkAnim)
        {
            actualVelocity = rb.velocity.magnitude;
            clampedVelocity = actualVelocity > 1f ? 1f : actualVelocity;
            animator.SetFloat("Speed", clampedVelocity);

            //This is clumsy and causes awkward stuttering when moving vertically or near-vertically. Should ideally be replaced
            if (rb.velocity.magnitude > 0.1 && !facingOverride)
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

    /// <summary>
    /// Keeps the target immune for a certain duration after being attacked.
    /// </summary>
    /// <param name="target">The target to be made immune</param>
    /// <param name="immuneDuration">How long the immunity lasts in seconds</param>
    /// <returns>Functional return IEnumerator</returns>
    private IEnumerator ImmuneCountdown(GameActor target, float immuneDuration)
    {
        target.Immune = true;

        yield return new WaitForSeconds(immuneDuration);

        target.Immune = false;
    }

    /// <summary>
    /// Prevents the target from taking action after being hit for a certain time.
    /// </summary>
    /// <returns>Functional return IEnumerator</returns>
    private IEnumerator Hitstun(GameActor target, float hitstunDuration)
    {
        target.Stunned = true;

        yield return new WaitForSeconds(hitstunDuration);

        target.Stunned = false;
    }

    internal void StartImmuneCountdown(GameActor target, float immuneDuration)
    {
        immuneCoroutine = ImmuneCountdown(target, immuneDuration);
        StartCoroutine(immuneCoroutine);
    }

    private void StopImmuneCountdown()
    {
        StopCoroutine(immuneCoroutine);
        immuneCoroutine = null;
    }

    internal void StartHitstun(GameActor target, float hitstunDuration)
    {
        hitstunCoroutine = Hitstun(target, hitstunDuration);
        StartCoroutine(hitstunCoroutine);
    }

    private void StopHitstun()
    {
        StopCoroutine(hitstunCoroutine);
        hitstunCoroutine = null;
    }

    //These are basically just here serving the same function as an interface cause I don't want to double up on both virtual inherited classes and also interfaces
    internal virtual void OnAttackEnd(string code) {}

    internal virtual void OnSuccessfulAttack(string code) {}

    internal virtual void OnHitWall() {}

    internal virtual void OnReceiveHit() {}

    internal virtual void OnHitPlayer() { }
}
