using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField]
    Player player;
    [SerializeField]
    Rigidbody rigid;
    Animator anim;

    [Space]
    [SerializeField]
    float transitionSpeed;

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
}
