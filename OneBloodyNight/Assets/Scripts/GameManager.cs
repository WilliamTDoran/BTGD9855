using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject debugCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            debugCanvas.SetActive(!debugCanvas.activeInHierarchy);
        }
    }
}
