using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    private const float MAX_DISTANCE = 50f;

    [Header("Inscribed")]
    [SerializeField]
    private float speed = 50f;
    [SerializeField]
    private GameObject _object;

    [Header("Dynamic")]
    public Character cameFrom;
    public Hurtbox hurtbox;
    public Color color;

    private void Awake()
    {
        hurtbox = GetComponentInChildren<Hurtbox>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GetComponentInChildren<Renderer>().material.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = _object.transform.localPosition;

        if (pos.z >= MAX_DISTANCE)
            Destroy(gameObject);

        pos.z += speed * Time.deltaTime;

        _object.transform.localPosition = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject go = other.gameObject;
        if (go.GetComponent<Character>() != null)
        {
            Destroy(gameObject);
        }
    }
}
