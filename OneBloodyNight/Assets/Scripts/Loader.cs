using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{

    void Start()
    {
        GameManager.instance.loadGame();
        GameManager.instance.LoadPerm();
    }
}
