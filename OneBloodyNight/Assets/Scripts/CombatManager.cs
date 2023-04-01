using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class drives and facilitates all combat interactions between GameActors
/// Primarily damage and knockback; pushback is handled individually
/// 
/// Version 1.0 (2/2/2023), Will Doran
/// Version 1.1 (2/8/2023), Will Doran
/// </summary>
public class CombatManager : MonoBehaviour
{


    private static CombatManager instance; //static reference
    public static CombatManager Instance { get { return instance; } }

    /// <summary>
    /// Creates a singleton static reference to this
    /// </summary>
    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    /// <summary>
    /// A public method that can be called from any Attack GameActor upon hitting a target.
    /// Spins off functions to apply knockback, and deals damage from attacker to target if applicable.
    /// </summary>
    /// <param name="attacker">reference to the GameActor who owns the Attack</param>
    /// <param name="used">reference to the Attack that called this function</param>
    /// <param name="target">reference to the GameActor being hit by the Attack</param>
    /// <param name="damageAmount">amount of damage to deal to target if target is a valid recipient for damage</param>
    /// <param name="knockbackAmount">amount of knockback to deal to target if target is a valid recipient for knockback</param>
    /// <returns>true if the attack dealt damage and/or knockback</returns>
    public bool Attack(GameActor attacker, Attack used, Collider target, int damageAmount, float knockbackAmount)
    {
        //creates a reference to the GameActor script of the target. Must be done here rather than as a parameter since the target won't always be a GameActor (if it's a wall)
        GameActor targetActor = target.gameObject.GetComponent<GameActor>();

        if (target.CompareTag("Wall")) //Pushes back off of walls
        {
            Debug.Log("Wall Hit " + target.gameObject.name);

            used.OnHitWall();

            used.Pushback(target, 1.0f);

            return false;
        }
        else if (target.CompareTag("Monster")) //Spins off damage, knockback, immunity, and hitstun functions for monsters (and bosses, even though they aren't *technically* monsters)
        {
            if (used.CheckForWalls)
            {
                if (WallCheck(attacker, targetActor))
                {
                    return false;
                }
            }

            //Spins off various reactive functions
            if (!targetActor.Immune)
            {
                HarmMonster(attacker, targetActor.gameObject.GetComponent<GameActor>(), damageAmount);
                if (targetActor.CurHitPoints > 0)
                {
                    ApplyKnockback(attacker, targetActor, knockbackAmount);
                }
                targetActor.StartImmuneCountdown(targetActor, targetActor.ImmuneDuration);
                targetActor.StartHitstun(targetActor, used.HitstunDuration);
                used.OnHitMonster();
                targetActor.OnReceiveHit();
            }
                

            return true;
        }
        else if (target.CompareTag("Player")) //Reduces blood meter and spins off knockback, immunity, and hitstun for players
        {
            if (used.CheckForWalls)
            {
                if (WallCheck(attacker, targetActor))
                {
                    return false;
                }
            }

            //Spins off various reactive functions
            if (!targetActor.Immune)
            {
                Bloodmeter.instance.changeBlood(-damageAmount);
                ApplyKnockback(attacker, targetActor, knockbackAmount);
                targetActor.StartImmuneCountdown(targetActor, targetActor.ImmuneDuration);
                targetActor.StartHitstun(targetActor, used.HitstunDuration);
                used.OnHitPlayer();
                targetActor.OnReceiveHit();
            }

            return true;
        }

        return false; //Should never be reached. Only here to avoid a compile-time error
    }
    
    /// <summary>
    /// Applies a reduction to the target monster's curHitPoints based on a parameter
    /// </summary>
    /// <param name="attacker">the GameActor who owns the Attack</param>
    /// <param name="target">the Monster being targeted</param>
    /// <param name="amount">the amount to reduce curHitPoints</param>
    private void HarmMonster(GameActor attacker, GameActor target, int amount)
    {
        
        target.CurHitPoints -= amount;
        Debug.Log("Send Damage to " + target.gameObject.name + " from " + attacker.gameObject.name);
        
    }

    /// <summary>
    /// Applies knockback to a GameActor's attached rigidbody
    /// </summary>
    /// <param name="attacker">the GameActor who owns the Attack</param>
    /// <param name="target">the GameActor who's rigidbody should be forced</param>
    /// <param name="amount">the amount of force to apply</param>
    private void ApplyKnockback(GameActor attacker, GameActor target, float amount)
    {
        Debug.Log("Send knockback to " + target.gameObject.name + " from " + attacker.gameObject.name);

        //Gets reference to rigidbodies
        Rigidbody attackerRigidBody = attacker.Rb;
        Rigidbody targetRigidBody = target.Rb;
        Vector3 direction;

        //Calculates the direction the knockback applies in based on the vector pointing from the attacker's centre to the target's centre
        if (attackerRigidBody != null)
        {
            direction = targetRigidBody.position - attackerRigidBody.position;
        }
        else
        {
            direction = targetRigidBody.position - attacker.transform.position;
        }
        direction.Normalize();

        direction *= amount; //scales the knockback

        targetRigidBody.AddForce(direction, ForceMode.Impulse); //applies force
    }



    /// <summary>
    /// Checks whether there is a wall inbetween the target and the attacker by raycasting
    /// </summary>
    /// <returns>True for interrupted by wall, false otherwise</returns>
    private bool WallCheck(GameActor attacker, GameActor target)
    {
        Vector3 attackerPosition = attacker.Rb.position;
        Vector3 targetPosition = target.Rb.position;
        Vector3 direction = targetPosition - attackerPosition;

        int layerMask = 1 << 6;

        return Physics.Raycast(attacker.Rb.position, direction, direction.magnitude, layerMask);
    }


    //~~The Coroutine Section~~
    


}
