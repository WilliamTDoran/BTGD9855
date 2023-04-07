using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterArcher : Monster
{
    private IEnumerator shootWaitCoroutine;

    public void ArrowUse()
    {
        facingAngle = Vector3.SignedAngle(Vector3.right, (Player.plr.Rb.position - rb.position), Vector3.up);
        facingAngle = facingAngle < 0 ? facingAngle + 360 : facingAngle;
        facingAngle += Random.Range(-10, 10);
        facingAngle *= -1;
        basicAttack.gameObject.SetActive(true);
        basicAttack.Fire();
    }

    public void ShootWaitDriver()
    {
        if(shootWaitCoroutine != null) StopShootWait();
        StartShootWait();
    }

    private IEnumerator ShootWait()
    {
        canMove = false;
        float ttw = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(ttw);
        canMove = true;
    }

    private void StartShootWait()
    {
        shootWaitCoroutine = ShootWait();
        StartCoroutine(shootWaitCoroutine);
    }

    private void StopShootWait()
    {
        StopCoroutine(shootWaitCoroutine);
        shootWaitCoroutine = null;
    }
}
