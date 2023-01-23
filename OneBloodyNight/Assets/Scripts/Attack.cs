using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a parent class that handles all activated attacks and hit-boxed abilities.
/// </summary>
public class Attack : GameActor
{
    private string swingTrigger = "Swing";
    private Vector3 attackerFacingDirection;

    private List<GameObject> hitThisSwing;

    /* Exposed Variables */
    [Header("Attack Statistics")]

    [Tooltip("The amount of damage dealt by the attack")]
    [SerializeField]
    private int damage; //the amount of damage dealt by the attack
    public int Damage { get { return damage; } set { damage = value; } }

    [Tooltip("The amount of knockback dealt by the attack")]
    [SerializeField]
    private float knockbackAmount; //the amount of knockback dealt by the attack
    public float KnockbackAmount { get { return knockbackAmount; } set { knockbackAmount = value; } }

    [Tooltip("The amount of pushback on the attacker")]
    [SerializeField]
    private float pushbackAmount; //the amount of pushback on the attacker
    public float PushbackAmount { get { return pushbackAmount; } set { pushbackAmount = value; } }

    [Tooltip("Return drag. Don't fuck with this unless you know what you're doing")]
    [SerializeField]
    private float returnDrag;

    [Tooltip("Drag on rb when pushing back. Don't fuck with this unless you know what you're doing")]
    [SerializeField]
    private float drag = 25f;

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
    private float floatDistance;

    [Tooltip("List of ignored attack tags. Essentially, anything that this attack *shouldn't* be able to hit.")]
    [SerializeField]
    private string[] untargetableTags;
    /*~~~~~~~~~~~~~~~~~~~~*/
    [Header("References")]

    [Tooltip("The GameActor who is making the attack")]
    [SerializeField]
    private GameActor attacker;

    [Tooltip("The attack's own animator")]
    [SerializeField]
    private Animator animator;
    /*~~~~~~~~~~~~~~~~~~~~*/
    [Header("Debug")]

    [Tooltip("Hide or show the hitbox of the attack when attacking")]
    [SerializeField]
    private bool showHitbox = false;

    [Tooltip("Reference to the hitbox renderer. Must be not null to show hitbox")]
    [SerializeField]
    private MeshRenderer hitboxMesh;
    /*~~~~~~~~~~~~~~~~~~~~*/

    public void StartSwing()
    {
        if (attacker.CanAttack)
        {
            PositionAttack();

            if (forceStill)
            {
                attacker.CanMove = false;
                attacker.Rb.velocity = Vector3.zero;
            }

            attacker.CanAttack = false;
            animator.SetTrigger(swingTrigger);
        }
    }

    private void PositionAttack()
    {
        Quaternion facingAngleRotation = Quaternion.Euler(0, 0, attacker.FacingAngle + 90f);
        Vector3 v3Facing = facingAngleRotation * Vector3.down; //Adding the 90f is a really silly way to handle this but damn it if it doesn't work...
        Vector3 attackerFacingDirection = new Vector3(v3Facing.x, v3Facing.y, 0);
        attackerFacingDirection.Normalize();

        transform.localPosition = attackerFacingDirection * floatDistance;
        transform.rotation = facingAngleRotation;
    }

    public void SwingBoxActive()
    {
        Debug.Log("Swing Box Active");

        if (showHitbox)
        {
            hitboxMesh.enabled = true;
        }
    }

    public void EndSwing()
    {
        Debug.Log("End Swing");

        if (forceStill)
        {
            attacker.CanMove = true;
        }

        if (showHitbox)
        {
            hitboxMesh.enabled = false;
        }

        attacker.Rb.drag = returnDrag;

        attacker.CanAttack = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit Detected");

        bool proceed = true;

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

        if (hitThisSwing.Contains(other.gameObject))
        {
            proceed = false;
        }

        if (proceed)
        {
            hitThisSwing.Add(other.gameObject);
            CombatManager.Instance.Attack(attacker, this, other, damage, knockbackAmount);
        }
    }

    public void Pushback(Collider other, float multiplier)
    {
        //Causes pushback on the attacker when you strike something. Vector points halfway between attacker facing and the line between the attacker and the struck target
        Vector3 direction = attacker.Rb.position - other.ClosestPoint(attacker.Rb.position);
        direction.z = 0;
        direction.Normalize();

        direction -= attackerFacingDirection;
        direction.Normalize();
        direction *= (pushbackAmount * multiplier);

        attacker.Rb.AddForce(direction, ForceMode.Impulse);
        attacker.Rb.drag = drag;
    }
}
