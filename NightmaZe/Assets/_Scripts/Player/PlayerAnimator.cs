using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    Player player;
    [SerializeField]
    Rigidbody rigid;
    [SerializeField]
    GameObject hitBox;
    [SerializeField]
    Rigidbody[] dreamerRagdolls;

    [Space]
    [SerializeField]
    float transitionSpeed;

    public bool attacking { get; private set; }

    Animator anim;

    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float speed = rigid.velocity.magnitude;
        float toPose;
        Player.Posture posture = player.GetCurrentPosture();
        switch (posture)
        {
            case Player.Posture.standing:
                speed /= player.moveSettings.walkVelocity * player.moveSettings.runMultiplier;
                toPose = 1;
                break;
            case Player.Posture.crouching:
                speed /= player.moveSettings.crouchVelocity * player.moveSettings.runMultiplier;
                toPose = .5f;
                break;
            default:
                speed /= player.moveSettings.proneVelocity * player.moveSettings.runMultiplier;
                toPose = 0;
                break;
        }
        anim.SetFloat("Speed", speed);
        anim.SetFloat("Pose", Mathf.Lerp(anim.GetFloat("Pose"), toPose, Time.fixedDeltaTime * transitionSpeed));
    }

    public void Jump()
    {
        anim.SetTrigger("Jump");
    }

    public void Punch()
    {
        if (hitBox == null)
            return;

        StartCoroutine(Punching());
    }

    IEnumerator Punching()
    {
        anim.SetTrigger("Punch");
        attacking = true;
        yield return new WaitForSeconds(1);

        hitBox.SetActive(true);
        while(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Punch")
            yield return null;

        attacking = false;
    }

    public void Kill()
    {
        foreach (Rigidbody r in dreamerRagdolls)
        {
            r.isKinematic = false;
            r.useGravity = true;
            r.GetComponent<Collider>().enabled = true;
        }
    }
}
