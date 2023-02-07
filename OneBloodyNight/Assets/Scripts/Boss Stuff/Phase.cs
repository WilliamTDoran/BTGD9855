using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase : MonoBehaviour
{
    enum Conditions
    {
        None,   //0
        HP,     //1
        Time,   //2
        Enemies //3
    }

    /* Exposed Variables */
    [Tooltip("The type of condition for exiting this phase")]
    [SerializeField]
    Conditions exitCondition;
    /*~~~~~~~~~~~~~~~~~~~*/
}
