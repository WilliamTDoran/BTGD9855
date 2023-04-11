using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpgradeTotem : MonoBehaviour
{

    [SerializeField]
    GameObject upgradeHUD;
    [SerializeField]
    float range;

    public GameObject firstTreeButton;

    void Update()
    {
        if (Input.GetButtonDown("Interact") && Vector3.Distance(Player.plr.transform.position, transform.position) < range) 
        {
            upgradeHUD.SetActive(true);
            Time.timeScale = 0f;
            Player.plr.Stunned = true;

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstTreeButton);
        }
    }
}
