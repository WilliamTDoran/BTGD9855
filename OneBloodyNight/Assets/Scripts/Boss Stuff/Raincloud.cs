using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Raincloud : GameActor
{
    private IEnumerator rainDamageCoroutine;

    /* Exposed Variables */
    [Tooltip("Be cautious when playing with me")]
    [SerializeField]
    private float speedClamp;

    [SerializeField]
    private Animator spritenimator;

    [Tooltip("How long in seconds between each damage tick")]
    [SerializeField]
    private float rainTickInterval = 0.15f;

    [SerializeField]
    private float damage = 20;
    /*~~~~~~~~~~~~~~~~~~~*/

    protected void OnEnable()
    {
        rb.velocity = Vector3.zero;
        spritenimator.SetTrigger("RainOnEm");
    }

    private void FixedUpdate()
    {
        Vector3 direction = Player.plr.Rb.position - rb.position;
        rb.AddForce(direction.normalized * speed, ForceMode.Impulse);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, 10);
    }

    private IEnumerator RainDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(rainTickInterval);

            if (col.bounds.Contains(Player.plr.Rb.position))
            {
                if (!Player.plr.Immune)
                {
                    Bloodmeter.instance.changeBlood(-damage);
                    Player.plr.StartImmuneCountdown(Player.plr, Player.plr.ImmuneDuration);
                    Player.plr.OnReceiveHit();
                }
            }
        }
    }

    internal void StartRainDamage()
    {
        rainDamageCoroutine = RainDamage();
        StartCoroutine(rainDamageCoroutine);
    }

    internal void StopRainDamage()
    {
        StopCoroutine(rainDamageCoroutine);
        rainDamageCoroutine = null;
    }
}
