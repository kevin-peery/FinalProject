using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protagonist : Character
{
    [Header("Inscribed")]
    public GameObject lPosition,
                      rPosition;

    [Header("Armature Joints")]
    [SerializeField]
    private GameObject root;
    [SerializeField]
    private GameObject hips, spine, chest, upperChest, neck, head,
                      lUpperLeg, lLowerLeg, lFoot, lToes,
                      rUpperLeg, rLowerLeg, rFoot, rToes,
                      lShoulder, lUpperArm, lLowerArm, lHand,
                      lThumb1, lThumb2, lThumb3,
                      lIndex1, lIndex2, lIndex3,
                      lMiddle1, lMiddle2, lMiddle3,
                      lRing1, lRing2, lRing3,
                      lPinkie1, lPinkie2, lPinkie3,
                      rShoulder, rUpperArm, rLowerArm, rHand,
                      rThumb1, rThumb2, rThumb3,
                      rIndex1, rIndex2, rIndex3,
                      rMiddle1, rMiddle2, rMiddle3,
                      rRing1, rRing2, rRing3,
                      rPinkie1, rPinkie2, rPinkie3;

    [Header("Dynamic")]
    private Weapon lw,
                   rw;
    private int comboCount;

    public override void Start()
    {
        base.Start();
        lw = Instantiate(weaponL, lPosition.transform);
        rw = Instantiate(weaponR, rPosition.transform);
        lPosition.transform.localRotation = Quaternion.Euler(-90f, 0f, 180f);
        rPosition.transform.localRotation = Quaternion.Euler(-90f, 0f, 180f);
        comboCount = 0;

        animator.speed = 0f;
        animator.Play("Idle", 0, 0f);
    }

    protected override void Update()
    {
        base.Update();

        if (!status.HasFlag(Status.IsShooting) && status.HasFlag(Status.IsMoving))
        {
            model.transform.localEulerAngles = Vector3.zero;
            animator.speed = 0f;
            animator.Play("Idle", 0, 0f);
        }
        if (!status.HasFlag(Status.IsGuarding))
        {
            animator.speed = 0f;
            animator.Play("Idle", 0, 0f);
        }
    }

    public override void Move(float x, float z, float speedMult)
    {
        base.Move(x, z, speedMult);
    }
    public override void MeleeAttack()
    {
        CancelInvoke(nameof(NotAttacking));
        status |= Status.IsAttacking;
        if (comboCount < 3)
            comboCount++;
        else
            comboCount = 1;

        switch (comboCount)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            default:
                break;
        }

        Invoke(nameof(NotAttacking), 0.1f);
    }
    public override void RangedAttack()
    {
        CancelInvoke(nameof(NotShooting));
        status |= Status.IsShooting;

        model.transform.localEulerAngles = aimingGuide.transform.localEulerAngles;

        animator.speed = 0f;
        animator.Play("Shoot", 0, 0f);

        if (Energy > 0)
        {
            lw.Shoot();
            rw.Shoot();
        }

        Invoke(nameof(NotShooting), 0.1f);
    }
    public override void Guard()
    {
        base.Guard();

        // Guard pose
        animator.speed = 0f;
        animator.Play("Guard", 0, 1f);
    }
    public override void SpinAttack(bool melee)
    {

    }
}
