using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    private IEnumerator immuneCoroutine;

    private static CombatManager instance;
    public static CombatManager Instance { get { return instance; } }

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

    public void Attack(GameActor attacker, Attack used, Collider target, int damageAmount, float knockbackAmount)
    {
        GameActor targetActor = target.gameObject.GetComponent<GameActor>();

        if (target.CompareTag("Wall"))
        {
            Debug.Log("Wall Hit " + target.gameObject.name);

            used.Pushback(target, 1.0f);
        }
        else if (target.CompareTag("Monster"))
        {
            HarmMonster(attacker, targetActor.gameObject.GetComponent<Monster>(), damageAmount);
            ApplyKnockback(attacker, targetActor, knockbackAmount);
        }
        else if (target.CompareTag("Player"))
        {
            ApplyKnockback(attacker, targetActor, knockbackAmount);
        }
    }
    
    private void HarmMonster(GameActor attacker, Monster target, int amount)
    {
        if (!target.Immune)
        {
            target.CurHitPoints -= amount;
            Debug.Log("Send Damage to " + target.gameObject.name + " from " + attacker.gameObject.name);
        }
    }

    private void ApplyKnockback(GameActor attacker, GameActor target, float amount)
    {
        Debug.Log("Send knockback to " + target.gameObject.name + " from " + attacker.gameObject.name);

        Rigidbody attackerRigidBody = attacker.Rb;
        Rigidbody targetRigidBody = target.Rb;

        Vector3 direction = targetRigidBody.position - attackerRigidBody.position;
        direction.Normalize();

        direction *= amount;

        targetRigidBody.AddForce(direction * amount, ForceMode.Impulse);
    }

    private IEnumerator ImmuneCountdown(GameActor target, float immuneDuration)
    {
        target.Immune = true;

        yield return new WaitForSeconds(immuneDuration);

        target.Immune = false;
    }

    private void StartImmuneCountdown(GameActor target, float immuneDuration)
    {
        immuneCoroutine = ImmuneCountdown(target, immuneDuration);
        StartCoroutine(immuneCoroutine);
    }

    private void StopImmuneCountdown()
    {
        StopCoroutine(immuneCoroutine);
        immuneCoroutine = null;
    }
}
