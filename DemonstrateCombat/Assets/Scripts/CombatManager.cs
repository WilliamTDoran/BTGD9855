using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{
    private IEnumerator immuneCoroutine;

    private static CombatManager instance;

    public static CombatManager Instance { get { return instance; } }

    [SerializeField]
    private Slider bloodSlider;

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

    public void HarmTarget(GameActor attacker, GameActor target, int damage)
    {
        if (!target.Immune)
        {
            Debug.Log("Send Damage to " + target.gameObject.name + " from " + attacker.gameObject.name);
            ApplyKnockback(attacker, target);
            ApplyDamage(attacker, target, damage);
            StartImmuneCountdown(target, target.ImmuneDuration);
        }
    }

    //Just repurposed from Melee. Probably should have tried to make this into a static method or something; I used basically this exact code like 3 times in 3 separate places.
    private void ApplyKnockback(GameActor attacker, GameActor target)
    {
        Rigidbody2D attackerRigidBody = attacker.gameObject.GetComponent<Rigidbody2D>();
        Rigidbody2D targetRigidBody = target.gameObject.GetComponent<Rigidbody2D>();

        Vector2 direction = targetRigidBody.position - attackerRigidBody.position;
        direction.Normalize();

        direction *= attacker.KnockbackForce;

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

    private void ApplyDamage(GameActor attacker, GameActor target, int damage)
    {
        if (target.gameObject.CompareTag("Player"))
        {
            bloodSlider.value -= damage;
        }
        else
        {
            target.HitPoints -= damage;

            if (attacker.gameObject.CompareTag("Player") && target.HitPoints <= 0)
            {
                bloodSlider.value += target.BloodPerKill;
            }
        }
    }
}
