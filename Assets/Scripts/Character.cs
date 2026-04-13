using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [System.Flags]
    public enum Status
    {
        None = 0,
        IsMoving = 1,
        IsAttacking = 2,
        IsShooting = 4,
        IsGuarding = 8,
        IsRecharging = 16,
        IsStunned = 32,
        IsDamaged = 64,
        IsDefeated = 128,
        IsDenyingInputs = 256
    }

    [Header("Inscribed")]
    public int maxHP = 100,
               maxEnergy = 100;
    public float rechargeRate = 10f;
    [SerializeField]
    protected float _speed = 10f;
    public Weapon weaponL,
                  weaponR;
    public GameObject model;
    public AimingGuide aimingGuide;
    public GuardZone guardZone;

    [Header("Dynamic")]
    public Animator animator;
    private int _hp;
    private float _energy,
                  _moveDirection,
                  _aimDirection,
                  prevSpeedMult;
    public Status status = 0;

    public float Speed
    {
        get { return _speed; }
    }
    public int HP
    {
        get { return _hp; }
        private set
        {
            _hp = value;
            if (_hp <= 0)
                Defeated();
        }
    }
    public float Energy
    {
        get { return _energy; }
        set
        {
            if (value <= 0f)
            {
                _energy = 0f;
                status |= Status.IsRecharging;
            }
            else if (value >= maxEnergy)
            {
                _energy = maxEnergy;
                status &= ~Status.IsRecharging;
            }
            else
                _energy = value;
        }
    }
    public float MoveDirection
    {
        get { return _moveDirection; }
        set { _moveDirection = Main.CheckRotation(value); }
    }
    public float AimDirection
    {
        get { return _aimDirection; }
        set { _aimDirection = Main.CheckRotation(value); }
    }

    // Start is called before the first frame update
    public virtual void Start()
    {
        HP = maxHP;
        Energy = maxEnergy;
        weaponL.wielder = this;
        weaponR.wielder = this;
        animator = GetComponentInChildren<Animator>();
        prevSpeedMult = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!(status.HasFlag(Status.IsAttacking) ||
              status.HasFlag(Status.IsShooting) ||
              status.HasFlag(Status.IsGuarding)))
        {
            Energy += rechargeRate * Time.deltaTime;

        }
    }

    public virtual void Move(float x, float z, float speedMult)
    {
        if (speedMult == 0f || status.HasFlag(Status.IsDenyingInputs))
        {
            status &= ~Status.IsMoving;
            prevSpeedMult = speedMult;
            return;
        }
        status |= Status.IsMoving;

        Vector3 pos = transform.position;
        pos.x += x * Speed * Time.deltaTime;
        pos.z += z * Speed * Time.deltaTime;
        transform.position = pos;

        if (speedMult >= prevSpeedMult)
            if (x == 0)
            {
                if (z < 0)
                    MoveDirection = 180f;
                else if (z > 0)
                    MoveDirection = 0f;
            }
            else
                MoveDirection = -(Mathf.Atan2(z, x)) * Mathf.Rad2Deg + 90;
        transform.rotation = Quaternion.Euler(0, MoveDirection, 0);

        prevSpeedMult = speedMult;
    }

    public virtual void MeleeAttack()
    {
        Debug.Log("melee");
    }
    public virtual void RangedAttack()
    {
        Debug.Log($"ranged-{aimingGuide.transform.localEulerAngles.y}");

    }
    public virtual void Guard()
    {
        if (status.HasFlag(Status.IsDenyingInputs))
            return;

        CancelInvoke(nameof(NotGuarding));

        status |= Status.IsGuarding;

        Invoke(nameof(NotGuarding), 0.1f);
    }
    public virtual void Block()
    {

    }
    public virtual void SpinAttack(bool melee)
    {
        if (melee)
            Debug.Log("melee spin");
        else
            Debug.Log("range spin");
    }

    public virtual void Damaged(int amount)
    {
        if (amount <= 0)
            return;

        CancelInvoke(nameof(NotDamaged));

        status |= Status.IsDamaged;
        HP -= amount;

        Invoke(nameof(NotDamaged), 0.1f);
    }

    public virtual void Defeated()
    {
        status = Status.IsDefeated;
        status |= Status.IsDenyingInputs;

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = true;
        }
        Invoke(nameof(Destroy), 5f);
    }

    protected void OnTriggerEnter(Collider other)
    {
        Hurtbox hurtbox = other.gameObject.GetComponent<Hurtbox>();
        if (hurtbox != null)
            Damaged(hurtbox.damage);
    }

    protected void NotAttacking() => status &= ~Status.IsAttacking;
    protected void NotShooting() => status &= ~Status.IsShooting;
    protected void NotGuarding() => status &= ~Status.IsGuarding;
    protected void NotDamaged() => status &= ~Status.IsDamaged;
    protected void NotDenyingInputs() => status &= ~Status.IsDenyingInputs;
}
