using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    private Rigidbody2D playerrb;
    private Vector2 playerFacingDirection;

    private IEnumerator swingCoroutine;
    private List<Collider2D> hitThisSwing = new List<Collider2D>();

    private float activeTime;
    private float buffer;
    private float bufferPunish;
    private bool inBuffer;

    /* Exposed Variables */
    [SerializeField]
    private float totalSwingTime;
    [SerializeField]
    private float warmupTime;
    [SerializeField]
    private float coolDownTime;
    [SerializeField]
    private float pushBackAmount = 50f;
    [SerializeField]
    private float pushBackEnemyModifier = 0.5f;
    [SerializeField]
    private float drag = 25f;
    [SerializeField]
    private float floatDistance = 1.0f;
    [SerializeField]
    private float bufferSize = 5f;
    [SerializeField]
    private float bufferPunishTime = 2f;

    [Header("References")]
    [SerializeField]
    private PlayerBody player;
    [SerializeField]
    private Collider2D col;
    [SerializeField]
    private SpriteRenderer attackSprite;

    [Header("Debug")]
    [SerializeField]
    private bool visualizeHitBox;
    [SerializeField]
    private Renderer colVisualizer;
    /* -~-~-~-~-~-~-~-~- */

    private void Awake()
    {
        activeTime = totalSwingTime - warmupTime - coolDownTime;
    }

    private void Start()
    {
        playerrb = player.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (player.PrimaryFireDown)
        {
            if (player.CanSwing)
            {
                StartSwing();
            }
            else if (!inBuffer && bufferPunish <= 0)
            {
                inBuffer = true;
                buffer = bufferSize;
            }
        }
        else if (inBuffer)
        {
            if (player.CanSwing)
            {
                StartSwing();
                inBuffer = false;
                buffer = 0f;
                bufferPunish = 0f;
            }

            if (buffer > 0f)
            {
                buffer -= Time.deltaTime;
            }
            else
            {
                inBuffer = false;
                bufferPunish = bufferPunishTime;
            }
        }
        else if (bufferPunish > 0)
        {
            bufferPunish -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit Detected");

        if (other.CompareTag("Wall"))
        {
            Debug.Log("Wall Hit " + other.gameObject.name);

            Pushback(other, 1.0f);
        }
        else if (other.CompareTag("Monster"))
        {
            GameActor target = other.gameObject.GetComponent<GameActor>();
            CombatManager.Instance.HarmTarget(player, target);

            Pushback(other, pushBackEnemyModifier);
        }
    }

    private IEnumerator Swing()
    {
        hitThisSwing.Clear();

        Debug.Log("Start Swing");
        player.CanMove = false;
        player.CanSwing = false;

        playerrb.velocity = Vector2.zero; //Player must stand still to swing

        yield return new WaitForSeconds(warmupTime); //First handful of frames start the animation, but have no active collider

        Quaternion facingAngleRotation = Quaternion.Euler(0, 0, player.FacingAngle + 90f);
        Vector3 v3Facing = facingAngleRotation * Vector3.down; //Adding the 90f is a really silly way to handle this but damn it if it doesn't work...
        playerFacingDirection = new Vector2(v3Facing.x, v3Facing.y);
        playerFacingDirection.Normalize();

        transform.localPosition = playerFacingDirection * floatDistance;
        transform.rotation = facingAngleRotation;

        attackSprite.enabled = true;

        col.enabled = true;

        Debug.Log("Melee Active");
        if (visualizeHitBox) { colVisualizer.enabled = true; }

        yield return new WaitForSeconds(activeTime);

        attackSprite.enabled = false;

        col.enabled = false;

        Debug.Log("Melee Inactive");
        if (visualizeHitBox) { colVisualizer.enabled = false; }

        yield return new WaitForSeconds(coolDownTime); //Similar to warmup, last handful of frames have no hitbox but are still winding down animation

        player.CanSwing = true;
        player.CanMove = true;
        playerrb.drag = 0;
    }

    private void StartSwing()
    {
        swingCoroutine = Swing();
        StartCoroutine(swingCoroutine);
    }

    private void StopSwing()
    {
        StopCoroutine(swingCoroutine);
        swingCoroutine = null;

        col.enabled = false;

        player.CanSwing = true;
        player.CanMove = true;
        playerrb.drag = 0;
    }

    private void Pushback(Collider2D other, float multiplier)
    {
        //Causes pushback on the player when you strike something. Vector points halfway between player facing and the line between the player and the struck target
        Vector2 direction = playerrb.position - other.ClosestPoint(playerrb.position);
        direction.Normalize();

        direction -= playerFacingDirection;
        direction.Normalize();
        direction *= (pushBackAmount * multiplier);

        playerrb.AddForce(direction, ForceMode2D.Impulse);
        playerrb.drag = drag;
    }
}
