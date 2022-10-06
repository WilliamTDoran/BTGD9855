using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBody : GameActor
{
    private GameObject controllerObj;
    private PlayerController controller;

    private bool primaryFireDown;
    public bool PrimaryFireDown { get { return primaryFireDown; } }
    private bool canSwing;
    public bool CanSwing { get { return canSwing; } set { canSwing = value; } }

    private Color baseColor;
    private int spriteSwitch = 0;
    private float facingAngle;
    public float FacingAngle { get { return facingAngle; } }

    /* Exposed Variables */
    [SerializeField]
    private List<Sprite> positions = new List<Sprite>();
    [SerializeField]
    private SpriteRenderer render;
    [SerializeField]
    private Color colorWhenHit;
    /* -~-~-~-~-~-~-~-~- */

    private void Start()
    {
        render.sprite = positions[0];
        baseColor = render.material.color;

        canMove = true;
        canSwing = true;

        controllerObj = GameObject.Find("ControllerHub");
        controller = controllerObj.GetComponent<PlayerController>();

        Debug.Assert(controller != null, "No controller set on: " + gameObject.name);
        Debug.Assert(rb != null, "No rigidbody set on: " + gameObject.name);
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            rb.velocity = controller.IntendedDirection * speed;
        }

        //rb.angularVelocity = new Vector3(0, rotationSpeed, 0) * controller.IntendedFacing; //Deprecated. Used to control facing in early test versions pre mouse control
    }

    private void Update()
    {
        primaryFireDown = controller.PrimaryFireDown;

        if (controller.IntendedDirection != Vector2.zero && canMove)
        {
            facingAngle = Vector2.SignedAngle(Vector2.right, controller.IntendedDirection);
            facingAngle = facingAngle < 0 ? facingAngle + 360 : facingAngle;
            spriteSwitch = (int)(facingAngle / 45f);
            render.sprite = positions[spriteSwitch];
        }
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
}
