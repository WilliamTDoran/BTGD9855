using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a parent class that handles all activated attacks and hit-boxed abilities.
/// 
/// Version 1.0 (1/24/2023), Will Doran
/// Version 1.1 (2/8/2023),  Will Doran
/// Version 1.2 (3/7/2023),  Will Doran
/// </summary>
public class Attack : GameActor
{
    private string attackerGrantedCode; //A unique ID that gets passed between attacker and attack. Used for activating functions that key off of moments in particular attacks
    private string swingTrigger = "Swing"; //Literally just a string for referencing the animator trigger
    private Vector3 attackerFacingDirection; //The direction the attacker is facing, as determined by the most recent direction they've moved

    private List<Collider> hitThisSwing = new List<Collider>(); //A list of everything already hit during this attack. Used to avoid double-hitting the same collider

    private Quaternion baseAnimRotation;

    private bool buffed = false;
    public bool Buffed { get { return buffed; } set { buffed = value; } }

    /* Exposed Variables */
    [Header("Attack Statistics")]

    [Tooltip("The amount of damage dealt by the attack")]
    [SerializeField]
    private int damage; //the amount of damage dealt by the attack
    public int Damage { get { return damage; } set { damage = value; } }

    [Tooltip("Optional: the damage dealt when buffed. If left blank, it will default to the same as normal damage")]
    [SerializeField]
    private int buffedDamage;

    [Tooltip("The amount of knockback dealt by the attack")]
    [SerializeField]
    private float knockbackAmount; //the amount of knockback dealt by the attack
    public float KnockbackAmount { get { return knockbackAmount; } set { knockbackAmount = value; } }

    [Tooltip("The duration of hitstun. A duration of 0 means no hitstun")]
    [SerializeField]
    private float hitstunDuration; //the amount of hitstun dealt by the attack
    public float HitstunDuration { get { return hitstunDuration; } }

    [Tooltip("The amount of pushback on the attacker")]
    [SerializeField]
    private float pushbackAmount; //the amount of pushback on the attacker
    public float PushbackAmount { get { return pushbackAmount; } set { pushbackAmount = value; } }

    [Tooltip("Return drag. Don't fuck with this unless you know what you're doing")]
    [SerializeField]
    private float returnDrag; //the drag value that's returned to at the end of the swing after pushback

    [Tooltip("Drag on rb when pushing back. Don't fuck with this unless you know what you're doing")]
    [SerializeField]
    private float drag = 20f; //the drag value that's set during pushback to avoid weird drift

    [Tooltip("Multiplier for pushback when hitting a wall instead of a gameactor. 1 is identical.")]
    [SerializeField]
    private float wallPushbackScalar; //multiplier for pushback when hitting a wall instead of a gameactor. 1 is identical.
    public float WallPushbackScalar { get { return wallPushbackScalar; } set { wallPushbackScalar = value; } }

    [Tooltip("Whether the attacker is forced to stand still while using this attack")]
    [SerializeField]
    private bool forceStill; //whether the attacker is forced to stand still while using this attack
    public bool ForceStill { get { return forceStill; } set { forceStill = value; } }

    [Tooltip("Scalar that defines how far from the player the attack hitbox is")]
    [SerializeField]
    protected float floatDistance; //1 is approximately directly adjacent

    [Tooltip("List of ignored attack tags. Essentially, anything that this attack *shouldn't* be able to hit.")]
    [SerializeField]
    private string[] untargetableTags; //used to dictate what things this attack is capable of hitting; monsters shouldn't hit other monsters, for example

    [Tooltip("Whether this attack should check for walls interrupting its line of effect")]
    [SerializeField]
    private bool checkForWalls = true;
    public bool CheckForWalls { get { return checkForWalls; } }
    /*~~~~~~~~~~~~~~~~~~~~*/
    [Header("References")]

    [Tooltip("The GameActor who is making the attack")]
    [SerializeField]
    protected GameActor attacker;

    [Tooltip("The game object which holds the visual animation")]
    [SerializeField]
    protected Animator spriteAnimator;
    /*~~~~~~~~~~~~~~~~~~~~*/
    [Header("Debug")]

    [Tooltip("Hide or show the hitbox of the attack when attacking")]
    [SerializeField]
    protected bool showHitbox = false;

    [Tooltip("Reference to the hitbox renderer. Must be not null to show hitbox")]
    [SerializeField]
    protected MeshRenderer hitboxMesh;
    /*~~~~~~~~~~~~~~~~~~~~*/

    protected override void Start()
    {
        base.Start();

        if (spriteAnimator != null)
        {
            baseAnimRotation = spriteAnimator.transform.localRotation;
        }

        if (buffedDamage == 0)
        {
            buffedDamage = damage;
        }
    }

    /// <summary>
    /// A standard Update function.
    /// Clumsy and might soon be deprecated, but it ensures that the attack swish is oriented correctly regardless of where the attacker is facing
    /// </summary>
    protected override void Update()
    {
        base.Update();

        if (spriteAnimator != null)
        {
            if (attacker.FacingAngle + 90f > 180 && attacker.FacingAngle + 90f <= 360) //I'm so sure there's a way to do this without && but I can't think of it
            {
                spriteAnimator.GetComponent<SpriteRenderer>().flipX = true;
                spriteAnimator.transform.localRotation = Quaternion.Euler(90f, facingAngle + 110f, 0);
            }
            else
            {
                spriteAnimator.GetComponent<SpriteRenderer>().flipX = false;
                spriteAnimator.transform.localRotation = baseAnimRotation;
            }
        }

    }

    /// <summary>
    /// A publically accessible method used to initiate the attack. Called by the attack, and spins off the animator and some helper functions
    /// </summary>
    public void StartSwing(string code)
    {
        attackerGrantedCode = code;

        if (attackerGrantedCode != "bat") { PositionAttack(); }//The attack hitbox always exists, and needs to have its relative position to the attacker updated each time. The bats thing is a hacky workaround that will likely get improved later

        if (animator != null) //projectiles don't need an animator
        {
            animator.SetTrigger(swingTrigger); //Each attack has its own animation controller that is used to dictate its windup and active time
        }

        if (spriteAnimator != null)
        {
            spriteAnimator.SetTrigger(swingTrigger); //The visual 'swish' effect operates on its own child object because it wasn't working otherwise
        }
    }

    /// <summary>
    /// Positions the attack somewhere in a 360 degree circle around the attacker. 
    /// Determined by finding the correct normalized position, then applying a distance scalar
    /// </summary>
    protected virtual void PositionAttack()
    {
        //Determines the angle the attack is facing, with 0 being screen-right, 90 being screen-down
        Quaternion facingAngleRotation = Quaternion.Euler(0, attacker.FacingAngle + 90f, 0); //Adding 90f is silly, but it works to align the numbers. Partially antiquated.

        //Extracts the attacker's current facing direction
        Vector3 v3Facing = facingAngleRotation * Vector3.forward;
        Vector3 attackerFacingDirection = new Vector3(v3Facing.x, v3Facing.z, 0);

        attackerFacingDirection.Normalize();

        //Positions the attack hitbox based on the gathered parameters
        transform.localPosition = attackerFacingDirection * floatDistance;
        transform.rotation = facingAngleRotation;
    }

    /// <summary>
    /// A debug function that shows the hitbox area while it's active
    /// Called by the animator when the hitbox becomes active
    /// </summary>
    public void SwingBoxActive()
    {
        Debug.Log("Swing Box Active");

        if (showHitbox)
        {
            hitboxMesh.enabled = true;
        }
    }

    /// <summary>
    /// Ends the swing, and undoes any ongoing conditions (forced still, etc.) related to the attack
    /// Called by the animator at the end of the swing animation
    /// </summary>
    internal void EndSwing()
    {
        Debug.Log("End Swing");

        //Clears forced still if applicable
        if (forceStill)
        {
            attacker.CanMove = true;
        }

        //Clears debug shown hitbox if applicable
        if (showHitbox)
        {
            hitboxMesh.enabled = false;
        }

        if (attacker.Rb != null)
        {
            attacker.Rb.drag = returnDrag; //Returns the drag to the starting position (no change if no pushback)
        }
        hitThisSwing.Clear(); //Clears the list of Colliders hit (largely just a precaution)
        attacker.OnAttackEnd(attackerGrantedCode);
    }

    /// <summary>
    /// A stock OnTriggerEnter. Sanitizes collision data to find valid hittable objects, and adds hit objects to a checked list to avoid double-hitting.
    /// </summary>
    /// <param name="other">A collider being overlapped. Each overlapped collider calls this function once</param>
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit Detected: " + other.gameObject.name);

        bool proceed = true; //In many cases, a detected trigger shouldn't actually move forward to a full attack. This handles that

        //Checks that the detected collider is of a type that this attack should be capable of hitting
        if (untargetableTags.Length > 0)
        {
            for (int i = 0; i < untargetableTags.Length; i++)
            {
                if (other.CompareTag(untargetableTags[i]))
                {
                    proceed = false;
                }
            }
        }

        //Checks that this attack hasn't already hit the detected collider
        if (hitThisSwing.Contains(other))
        {
            proceed = false;
        }

        if (proceed)
        {
            hitThisSwing.Add(other); //Records the detected collider as already hit by this attack

            //Calls to CombatManager to run the attack. Returns true if it hits a useful target, spinning off any requisite follower functions on attacker
            if (CombatManager.Instance.Attack(attacker, this, other, buffed ? buffedDamage : damage, knockbackAmount)) 
            {
                attacker.OnSuccessfulAttack(attackerGrantedCode);
            }
        }
    }

    /// <summary>
    /// On a hit, the attacker may be pushed slightly back from the target (typically less than the knockback dealt)
    /// </summary>
    /// <param name="other">The collider being pushed back from</param>
    /// <param name="multiplier">A scalar difference in how much more or less the attack should pushback if the target is a wall or not</param>
    public void Pushback(Collider other, float multiplier)
    {
        //Causes pushback on the attacker when you strike something. Vector points halfway between attacker facing and the line between the attacker and the struck target
        Vector3 direction = attacker.Rb.position - other.ClosestPoint(attacker.Rb.position);
        direction.y = 0;
        direction.Normalize();
        direction -= attackerFacingDirection;
        direction.Normalize();
        direction *= (pushbackAmount * multiplier);

        attacker.Rb.AddForce(direction, ForceMode.Impulse); //Applies the calculated force
        attacker.Rb.drag = drag; //sets the drag to a higher parameterized value until the end of the attack. Prevents attackers with low base drag (aka player) from sliding backward icily.
    }

    //Used like an interface
    internal virtual void Fire() { }

    internal virtual void OnHitMonster()
    {
        if (attackerGrantedCode == "bat")
        {
            Player.plr.GetComponent<PlayerStrigoi>().swarmHit();
        }
    }
}
