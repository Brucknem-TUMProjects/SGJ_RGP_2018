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
    public bool knockedOut { get; private set; }

    Animator anim;

    float rightArmHoldWeight;
    float rightHandCloseWeight;

    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();

        if (dreamerRagdolls.Length > 0)
        {
            foreach (Rigidbody rigid in dreamerRagdolls)
                rigid.GetComponent<Collider>().enabled = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!knockedOut)
            UpdateMovement();
        else
            Crawling();

        if(anim.layerCount > 1)
        {
            anim.SetLayerWeight(1, rightHandCloseWeight);
            anim.SetLayerWeight(2, Mathf.Lerp(anim.GetLayerWeight(2), rightArmHoldWeight, Time.fixedDeltaTime * 5));
        }
    }

    void UpdateMovement()
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

    void Crawling()
    {
        anim.speed = rigid.velocity.magnitude;
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
        while (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Punch")
            yield return null;

        attacking = false;
    }

    public void KnockOut()
    {
        anim.SetTrigger("KnockOut");
    }

    public void Revive()
    {
        anim.SetTrigger("Revive");
    }

    public void Kill()
    {
        foreach (Rigidbody rigid in dreamerRagdolls)
        {
            rigid.isKinematic = false;
            rigid.GetComponent<Collider>().enabled = true;
        }
    }

    public void HaveLatern(bool have)
    {
        rightHandCloseWeight = have ? 1 : 0;
    }

    public void UseLantern(bool use)
    {
        rightArmHoldWeight = use ? 1 : 0;
    }
}
