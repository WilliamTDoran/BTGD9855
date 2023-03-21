using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class Raincloud : GameActor
{
    private IEnumerator rainDamageCoroutine;

    /* Exposed Variables */
    [Tooltip("How long in seconds between each damage tick")]
    [SerializeField]
    private float rainTickInterval = 0.15f;

    [SerializeField]
    private float damage = 20;
    /*~~~~~~~~~~~~~~~~~~~*/

    protected override void Start()
    {
        base.Start();

        StartRainDamage();
    }

    private IEnumerator RainDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);

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
