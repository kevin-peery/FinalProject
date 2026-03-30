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
        IsGuarding = 4,
        IsAirborne = 8,
        IsStunned = 16,
        IsDefeated = 32
    }



    private int _hp;
    private float _speed = 10f;
    public int maxHP;
    public Weapon weapon;
    [Header("Dynamic")]
    public Status status = 0;

    public float Speed
    {
        get { return _speed; }
        //get { return _speed * weapon.weight; }
    }
    public int HP
    {
        get { return _hp; }
        private set
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void AnimateRun(float speedMult)
    {

    }

    public void AnimateMeleeAttack()
    {
        Debug.Log("melee");
    }

    public void Defeated()
    {
        status = Status.IsDefeated;
    }
}
