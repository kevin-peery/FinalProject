using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hurtbox : MonoBehaviour
{
    [Header("Inscribed")]
    public bool melee;

    [Header("Dynamic")]
    public int damage = 1;
    public bool active,
                blockable;
    public Character cameFrom;

    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        Hurtbox h = other.gameObject.GetComponent<Hurtbox>();
        if (h == null)
            return;

        //if (melee && !h.melee)
        //    Destroy(h);

    }
}
