using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActor : MonoBehaviour
{
    /* This class is mostly used for being an easy place to define variables common to a large number of game entities.
     * Its main use is just keeping code on other classes more organized, and reducing the number of times I need to define a 'speed' variable.
     * 
     * Oh also that and facilitating damage and combat. */

    protected Rigidbody2D rb;
    protected Collider2D col;
    //protected Fader fader;

    protected bool canMove;
    public bool CanMove { get { return canMove; } set { canMove = value; } }
    protected bool immune;
    public bool Immune { get { return immune; } set { immune = value; } }

    /* Exposed Variables */
    [SerializeField]
    protected float speed;
    [Tooltip ("Not used for player")]
    [SerializeField]
    protected int hitPoints;
    public int HitPoints { get { return hitPoints; } set { hitPoints = value; } }
    [SerializeField]
    protected float lifetime;
    [SerializeField]
    protected int bloodPerKill;
    public int BloodPerKill { get { return bloodPerKill; } }
    [SerializeField]
    protected float fadeTime;
    [SerializeField]
    protected List<Renderer> fadeableRenderers;
    [SerializeField]
    protected float knockbackForce;
    public float KnockbackForce { get { return knockbackForce; } }
    [SerializeField]
    protected float immuneDuration;
    public float ImmuneDuration { get { return immuneDuration; } }
    /* -~-~-~-~-~-~-~-~- */

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        //fader = GetComponent<Fader>();

        Debug.Assert(rb != null, "No rigidbody set on: " + gameObject.name);
        Debug.Assert(col != null, "No collider set on: " + gameObject.name);
        //Debug.Assert(fader != null, "No fader set on: " + gameObject.name);
    }
}
