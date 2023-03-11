using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This class is the parent of all three player types, and holds the majority of functions shared by every type of character, including getting and interpreting control inputs
/// 
/// Version 1.0 (1/13/2023), Will Doran
/// Version 1.1 (2/7/2023),  Will Doran
/// Version 1.2 (2/10/2023), Will Doran
/// </summary>
public class Player : GameActor
{
    private GameObject controllerObj; //gameObject with the playercontroller script on it
    protected PlayerController controller; //said playercontroller script

    //Button input variables
    protected bool interactDown;
    protected bool basicAttackDown;
    protected bool advancedAttackDown;
    protected bool load1Down;
    protected bool load2Down;

    internal static Player plr; //static reference

    private int loadedBullet = 0; //the currently prepared advanced attack. I use a 'loading special bullets in a gun, then pulling the trigger' analogy for how the advanced attacks work, hence the variable name

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
    /// Standard Start function. 
    /// Initializes a couple values, creates references to useful objects. You know the drill.
    /// Also now holds code relevant to getting control input which GameActor previously handled.
    /// </summary>
    protected override void Start()
    {
        base.Start();

        controllerObj = GameObject.Find("PlayerController");
        controller = controllerObj.GetComponent<PlayerController>(); //finds the playercontroller

        Debug.Assert(controller != null, "No controller set on: " + gameObject.name);

        canMove = true;
        canAttack = true;
        speed *= 10; //With the new scale for movement, this variable needs to be really high so I just arbitrarily multiply it by 10 so that the value exposed to the editor doesn't have to be too crazy.
    }

    /// <summary>
    /// Standard FixedUpdate function. Drives movement. Note that despite the code appearing here, in execution order FixedUpdate takes place after Update
    /// </summary>
    private void FixedUpdate()
    {
        if (canMove && !stunned)
        {
            //rb.velocity = controller.IntendedDirection * speed; //This was the old way of handling movement. I'm leaving it here in case I ever want to swap back, since it has a bit of a different feel
            rb.AddForce(controller.IntendedDirection * speed, ForceMode.Force);
        }
    }

    /// <summary>
    /// Standard Update function. Primarily drives getting control inputs.
    /// </summary>
    protected override void Update()
    {
        base.Update();

        //Gets all control button checks
        interactDown = controller.InteractDown;
        basicAttackDown = controller.BasicFireDown;
        advancedAttackDown = controller.AdvancedFireDown;
        load1Down = controller.Load1Down;
        load2Down = controller.Load2Down;

        //sets the facing direction based on control direction (just a variable that measures rotation in degrees; doesn't do anything to the transform yet)
        if (controller.IntendedDirection != Vector3.zero && canMove)
        {
            facingAngle = Vector3.SignedAngle(Vector3.right, controller.IntendedDirection, Vector3.up);
            facingAngle = facingAngle < 0 ? facingAngle + 360 : facingAngle;
        }

        if (facingDebugText != null)
        {
            facingDebugText.text = facingAngle + "";
        }

        //drives loading advanced abilities
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

        //launches advanced abilities
        if (advancedAttackDown)
        {
            AdvancedFire(ref loadedBullet);
        }
    }

    internal override void OnReceiveHit()
    {
        base.OnReceiveHit();

        animator.SetTrigger("Owie");
    }


    //Like in GameActor, these are mostly just being used an impromptu interface since I don't want to double up on both virtual methods and interfaces
    protected virtual void AdvancedFire(ref int bullet)
    {

    }

    internal override void OnSuccessfulAttack(string code)
    {
        base.OnSuccessfulAttack(code);
    }
}
