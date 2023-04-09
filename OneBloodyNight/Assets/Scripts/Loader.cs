using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{

    void awake()
    {
        GameManager.instance.loadGame();
    }
}
