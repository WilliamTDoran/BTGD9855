using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        Player.plr.Stunned = true;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
