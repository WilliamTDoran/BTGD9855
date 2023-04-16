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
    private bool nearThis;

    public GameObject firstTreeButton;

    void Update()
    {
        if (Vector3.Distance(Player.plr.transform.position, transform.position) < range) 
        {
            Player.plr.enableInteractToolTip();
            nearThis = true;
            if (Input.GetButtonDown("Interact"))
            {
                upgradeHUD.SetActive(true);
                Time.timeScale = 0f;
                Player.plr.Stunned = true;

                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(firstTreeButton);
            }
        } else if (nearThis)
        {
            nearThis = false;
            Player.plr.disableInteractToolTip();
        }
    }
}
