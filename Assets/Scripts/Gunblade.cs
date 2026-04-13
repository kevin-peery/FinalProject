using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gunblade : Weapon
{
    [Header("Inscribed")]
    public GameObject blade;

    [Header("Dynamic")]
    private Hurtbox hurtbox;

    protected override void Start()
    {
        base.Start();
        hurtbox = blade.GetComponent<Hurtbox>();
        hurtbox.damage = meleeDamage;
    }

    protected override void Update()
    {
        base.Update();

        blade.SetActive(wielder.status.HasFlag(Character.Status.IsAttacking) ||
                        wielder.status.HasFlag(Character.Status.IsGuarding));
        hurtbox.active = wielder.status.HasFlag(Character.Status.IsAttacking);
    }
}
