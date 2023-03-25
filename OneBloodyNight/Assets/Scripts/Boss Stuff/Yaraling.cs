using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yaraling : GameActor
{
    /* Exposed Variables */
    [Header("Yaraling Stuff")]
    [Tooltip("The amount of blood the player recovers when killing the enemy")]
    [SerializeField]
    private float bloodOnKill = 50.0f;
    public float BloodOnKill { get { return bloodOnKill; } }

    [SerializeField]
    private Animator spanimator;

    [SerializeField]
    private SpriteRenderer sprender;
    /*~~~~~~~~~~~~~~~~~~~*/

    protected override void Start()
    {
        base.Start();

        CurHitPoints = MaxHitPoints;
    }

    protected override void Update()
    {
        base.Update();

        if (CurHitPoints <= 0 && !dead)
        {
            Spawner.enemKilled();
            Bloodmeter.instance.bloodmeter.value += bloodOnKill * Player.plr.bloodRegainMult;

            dead = true;
            spanimator.SetTrigger("Die");
            StopAllCoroutines();
            canMove = false;
            canAttack = false;
            Stunned = true;
            col.enabled = false;
            StartCoroutine(PopAway());
        }
    }

    protected override void LateUpdate()
    {
        if (walkAnim)
        {
            actualVelocity = rb.velocity.magnitude;
            clampedVelocity = actualVelocity > 1f ? 1f : actualVelocity;
            spanimator.SetFloat("Speed", clampedVelocity);

            //This is clumsy and causes awkward stuttering when moving vertically or near-vertically. Should ideally be replaced
            if (rb.velocity.magnitude > 0.1)
            {
                if (rb.velocity.x < 0)
                {
                    sprender.flipX = true;
                }
                else
                {
                    sprender.flipX = false;
                }
            }
        }
    }

    internal override void OnReceiveHit()
    {
        base.OnReceiveHit();
        spanimator.SetTrigger("Owie");
    }

    private IEnumerator PopAway()
    {
        yield return new WaitForSeconds(3);
        gameObject.SetActive(false);
    }
}
