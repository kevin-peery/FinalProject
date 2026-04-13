using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingGuide : MonoBehaviour
{
    [Header("Inscribed")]
    [SerializeField]
    private Character character;

    [Header("Dynamic")]
    public bool playerControlled = false;

    void Update()
    {
        //if (character == null)
        //    Debug.Log("char");
        //else
        transform.rotation = Quaternion.Euler(0, character.AimDirection, 0);
    }
}
