using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Inscribed")]
    public Color color = new Color(1, 1, 1);
    public Bullet bullet;
    public GameObject barrelPoint;
    public float fireRate = 4f;
    public int rangeDamage = 1,
               rangeEnergy = 1,
               meleeDamage = 10,
               meleeEnergy = 5;

    [Header("Dynamic")]
    protected float fireRateCount;
    public float energy;
    public Character wielder;

    private void Awake()
    {

    }

    protected virtual void Start()
    {
        fireRateCount = 0f;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        fireRateCount -= Time.deltaTime;
        if (fireRateCount < 0)
            fireRateCount = 0f;

    }

    public virtual void Shoot()
    {
        if (fireRateCount == 0)
        {
            Bullet b = Instantiate(bullet, barrelPoint.transform.position, barrelPoint.transform.rotation);
            b.cameFrom = wielder;
            b.hurtbox.damage = rangeDamage;
            b.color = color;

            fireRateCount += 1 / fireRate;
            wielder.Energy -= rangeEnergy;
        }
    }
}
