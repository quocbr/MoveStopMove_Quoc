
using System.Collections;
using UnityEngine;

public class Char : MonoBehaviour
{
    public GameObject weapon;
    public GameObject bulletPrefab;
    public float speed;
    public float lifeTime;
    public Animator animator;
    string curanim;
    public bool canAttack = true;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            ChangeAnim("Attack");
            canAttack = false;
        }
    }

    IEnumerator IE_Bullet(Transform bulletTF, Vector3 dir)
    {
        for (int i = 0; i < 60 * lifeTime; i++)
        {
            bulletTF.Translate(dir * Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }
        Destroy(bulletTF.gameObject);
    }

    public void Throw()
    {
        weapon.SetActive(false);
        StartCoroutine(
            IE_Bullet(
                Instantiate(bulletPrefab, weapon.transform.position, weapon.transform.rotation).transform,
                transform.forward
            )
        );
        print("Throw");
    }

    public void ResetAttack()
    {
        weapon.SetActive(true);
        print("ResetAttack");
        ChangeAnim("Idle");
        canAttack = true;
    }

    public void ChangeAnim(string animName)
    {
        animator.ResetTrigger(curanim);
        curanim = animName;
        animator.SetTrigger(curanim);
    }
}
