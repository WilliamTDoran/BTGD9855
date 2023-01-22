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

    public void HarmTarget(GameActor attacker, GameActor target, int amount, float knockBackAmount)
    {
        if (!target.Immune)
        {
            //target.HitPoints -= amount;
            Debug.Log("Send Damage to " + target.gameObject.name + " from " + attacker.gameObject.name);
            ApplyKnockback(attacker, target, knockBackAmount);
        }
    }

    private void ApplyKnockback(GameActor attacker, GameActor target, float amount)
    {
        Rigidbody attackerRigidBody = attacker.Rb;
        Rigidbody targetRigidBody = target.Rb;

        Vector3 direction = targetRigidBody.position - attackerRigidBody.position;
        direction.Normalize();

        direction *= amount;

        targetRigidBody.velocity += direction;
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
