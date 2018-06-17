using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MonoBehaviour
{
    [SerializeField]
    PlayerAnimator anim;

    float cooldown = 10.0f;
    float timer;

    Player.MoveSettings moveSettingsBoosted = new Player.MoveSettings();

    void Start()
    {
        timer = Time.time - cooldown;
    }

    void Update()
    {
        if (timer + cooldown < Time.time)
        {
            Debug.Log("Timer ready");
            if (Input.GetAxisRaw("VerticalSteuerkreuz") < 0)
            {
                // Steuerkreuz nach unten
            }
            else if (Input.GetAxisRaw("VerticalSteuerkreuz") > 0)
            {
                // Steuerkreuz nach oben --> Boost speed for 5 secs
                Debug.Log("Move BoosT");
                StartCoroutine(BoostSpeed());

            }
            else if (Input.GetAxisRaw("HorizontalSteuerkreuz") > 0)
            {
                // Steuerkreuz nach links
            }
            else if (Input.GetAxisRaw("HorizontalSteuerkreuz") < 0)
            {
                // Steuerkreuz nach rechts
            }
            else
            {
                return;
            }
            timer = Time.time;
        }
        if (Input.GetButtonDown("InteractRight"))
        {
            Attack();
        }
    }

    void Attack()
    {
        if (!anim.attacking)
            anim.Punch();
    }

    IEnumerator BoostSpeed()
    {
        GetComponent<Player>().speedBoost = 2;
        yield return new WaitForSeconds(5);
        GetComponent<Player>().speedBoost = 1;
    }
}
