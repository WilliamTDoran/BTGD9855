using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    /* Exposed Variables */
    [Tooltip("The number of stages, in order from first to last, with the hp percent threshold for when the stage ENDS")]
    [SerializeField]
    private Stage[] stages;
    /*~~~~~~~~~~~~~~~~~~~*/
}
