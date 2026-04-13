using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardZone : MonoBehaviour
{
    [Header("Inscribed")]
    public Character blocker;

    [Header("Dynamic")]
    public float triggered = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (triggered != 0f)
        {
            triggered -= Time.deltaTime;
            if (triggered < 0f)
                triggered = 0f;

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!blocker.status.HasFlag(Character.Status.IsGuarding))
            return;

        Vector3 point = other.ClosestPointOnBounds(transform.position),
                rotation = transform.eulerAngles;
        float xDistance = point.x - transform.position.x,
              yDistance = point.y - transform.position.y;

        if (xDistance == 0)
        {
            if (yDistance < 0)
                rotation.y = 270f;
            else if (yDistance > 0)
                rotation.y = 90f;
        }
        else
            rotation.y = -(Mathf.Atan2(yDistance, xDistance) * Mathf.Rad2Deg);

        transform.eulerAngles = rotation;

        GameObject go = other.gameObject;
        if (go.GetComponent<Bullet>() != null)
        {
            Destroy(other.gameObject);
            triggered++;

        }
        if (go.GetComponent<Hurtbox>() != null)
        {

        }
    }
}
