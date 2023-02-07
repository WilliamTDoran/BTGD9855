using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This class is the parent of all three player types, and holds the majority of functions shared by every type of character, including getting and interpreting control inputs
/// 
/// Version 1.0 (1/13/2023), Will Doran
/// Version 1.1 (2/7/2023),  Will Doran
/// </summary>
public class Player : GameActor
{
    private Transform renderBox;
    internal static Player plr;

    private int loadedBullet = 0;

    private bool visible = true; //used for strigoi invisibility. placed here rather that in playerstrigoi to avoid an ugly complicated if-chain in monsters
    public bool Visible { get { return visible; } set { visible = value; } }

    /* Exposed Variables */
    [Tooltip("Sprite Renderer reference")]
    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    [Tooltip("The cost of Ability 1 (Q/LB)")]
    [SerializeField]
    protected float abilityOneCost;

    [Tooltip("The cost of Ability 2 (E/RB)")]
    [SerializeField]
    protected float abilityTwoCost;

    [Tooltip("Facing Angle Debug Text")]
    [SerializeField]
    protected TextMeshProUGUI facingDebugText;
    public TextMeshProUGUI FacingDebugText { get { return facingDebugText; } }

    [Tooltip("Can Attack Debug Text")]
    [SerializeField]
    protected TextMeshProUGUI canAttackDebugText;
    public TextMeshProUGUI CanAttackDebugText { get { return canAttackDebugText; } }
    /*~~~~~~~~~~~~~~~~~~~*/

    /// <summary>
    /// Used for ease of reference
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        if (plr != null & plr != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            plr = this;
        }
    }

    /// <summary>
    /// Standard Start function. Initializes a couple values, creates references to useful objects. You know the drill.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        canMove = true;
        canAttack = true;
    }

    /// <summary>
    /// Standard FixedUpdate function. Drives movement. Note that despite the code appearing here, in execution order FixedUpdate takes place after Update
    /// </summary>
    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = controller.IntendedDirection * speed;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (controller.IntendedDirection != Vector3.zero && canMove)
        {
            facingAngle = Vector3.SignedAngle(Vector3.right, controller.IntendedDirection, Vector3.forward);
            facingAngle = facingAngle < 0 ? facingAngle + 360 : facingAngle;
        }

        if (facingDebugText != null)
        {
            facingDebugText.text = facingAngle + "";
        }

        if (load1Down)
        {
            if (loadedBullet == 1)
            {
                loadedBullet = 0;
            }
            else
            {
                loadedBullet = 1;
            }
        }

        if (load2Down)
        {
            if (loadedBullet == 2)
            {
                loadedBullet = 0;
            }
            else
            {
                loadedBullet = 2;
            }
        }

        if (advancedAttackDown)
        {
            AdvancedFire(ref loadedBullet);
        }
    }

    protected virtual void AdvancedFire(ref int bullet)
    {

    }

    internal override void OnSuccessfulAttack(string code)
    {
        base.OnSuccessfulAttack(code);
    }
}
