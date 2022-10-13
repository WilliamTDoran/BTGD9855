using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : GameActor
{
    private IEnumerator hitStunCoroutine;

    private Color baseColor;

    private MonsterSight sight;

    /* for blood meter */
    [SerializeField]
    private int health;
    [SerializeField]
    private Slider bloodmeter;
    /*~~~~~~~~~~~~~~~~~~~~~~~*/

    /* Exposed Variables */
    [SerializeField]
    private float hitStunDuration = 0.5f;
    [SerializeField]
    private float hitStunDrag = 10f;
    [SerializeField]
    private SpriteRenderer render;
    [SerializeField]
    private Color colorWhenHit;
    [SerializeField]
    private GameObject visionArea;
    [SerializeField]
    private GameObject player;
    /* -~-~-~-~-~-~-~-~- */

    private void Start()
    {
        baseColor = render.material.color;
        sight = visionArea.GetComponent<MonsterSight>();
        canMove = true;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            GameActor target = collision.gameObject.GetComponent<GameActor>();
            Debug.Log(gameObject.name + " has touched " + target.name);

            if (!target.Immune) { HitTarget(target); }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = player.GetComponent<Rigidbody2D>().position - rb.position;
        direction.Normalize();

        if (canMove && sight.Chasing)
        {
            rb.AddForce(direction * speed, ForceMode2D.Force);
        }
        /* for blood meter */
        if (health < 1)
        {
            //enemy dies player gets blood
            bloodmeter.value = bloodmeter.value + 10;
        }
    /*~~~~~~~~~~~~~~~~~~~~~~~*/
}

private void LateUpdate()
    {
        if (immune)
        {
            render.material.color = colorWhenHit;
        }
        else
        {
            render.material.color = baseColor;
        }
    }

    private void HitTarget(GameActor target)
    {
        StartHitStun(target);

        CombatManager.Instance.HarmTarget(this, target);
    }

    private IEnumerator HitStun(GameActor target)
    {
        canMove = false;

        PlayerBody playerScript = player.GetComponent<PlayerBody>();
        playerScript.CanMove = false;
        playerScript.CanSwing = false;

        Rigidbody2D targetrb = target.gameObject.GetComponent<Rigidbody2D>();
        targetrb.velocity = Vector2.zero;
        targetrb.drag = hitStunDrag;

        yield return new WaitForSeconds(hitStunDuration);

        playerScript.CanMove = true;
        playerScript.CanSwing = true;

        canMove = true;
    }

    private void StartHitStun(GameActor target)
    {
        hitStunCoroutine = HitStun(target);
        StartCoroutine(hitStunCoroutine);
    }

    private void StopHitStun()
    {
        StopCoroutine(hitStunCoroutine);
        hitStunCoroutine = null;
    }
}
