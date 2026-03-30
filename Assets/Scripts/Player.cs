using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Flags]
    public enum Inputs
    {
        None = 0,
        MeleeAttack = 1,
        RangedAttack = 2,
        Skill1 = 4,
        Skill2 = 8,
        Skill3 = 16,
        Skill4 = 32
    }

    private const int MAX_TRACK_COUNT = 60;

    private static Player S;

    [Header("Inscribed")]
    public Character playAs;

    [Header("Dynamic")]
    private List<Inputs> _inputTracker;
    private List<float[]> _axesTracker;
    private float _moveDirection,
                  _aimDirection;

    public List<Inputs> InputTracker
    {
        get { return _inputTracker; }
        private set { _inputTracker = value; }
    }
    public List<float[]> AxesTracker
    {
        get { return _axesTracker; }
        private set { _axesTracker = value; }
    }
    public float MoveDirection
    {
        get { return _moveDirection; }
        set {  _moveDirection = CheckRotation(value); }
    }

    private float CheckRotation(float rotation)
    {
        while (rotation < 0)
            rotation += 360;
        while (rotation >= 360)
            rotation -= 360;
        return rotation;
    }

    void Awake()
    {
        S = this;
        InputTracker = new List<Inputs>();
        AxesTracker = new List<float[]>();
    }

    void Update()
    {
        // Read current inputs
        int trackBack;
        Inputs input = 0;

        // Read axes and mouse
        float xAxis = Input.GetAxis("Horizontal"),
              zAxis = Input.GetAxis("Vertical"),
              hAxis = Mathf.Sqrt((xAxis * xAxis) + (zAxis * zAxis));

        if (hAxis > 1)
            hAxis = 1;

        if (InputTracker.Count >= MAX_TRACK_COUNT)
            InputTracker.RemoveAt(MAX_TRACK_COUNT - 1);
        if (AxesTracker.Count >= MAX_TRACK_COUNT)
            AxesTracker.RemoveAt(MAX_TRACK_COUNT - 1);

        InputTracker.Insert(0, input);
        AxesTracker.Insert(0, new float[] { xAxis, zAxis, hAxis });

        // Read previous inputs
        Inputs prevInputs;
        float prevHAxis = 0;

        trackBack = 0;
        foreach (var axes in AxesTracker)
        {
            trackBack++;
            if (trackBack > 1)
            {
                prevHAxis = axes[2];
                break;
            }
        }

        Vector3 pos = transform.position;
        pos.x += xAxis * playAs.Speed * Time.deltaTime;
        pos.z += zAxis * playAs.Speed * Time.deltaTime;
        transform.position = pos;

        if (hAxis >= prevHAxis)
            if (xAxis == 0)
            {
                if (zAxis < 0)
                    transform.rotation = Quaternion.Euler(0, 180, 0);
                else if (zAxis > 0)
                    transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                MoveDirection = -(Mathf.Atan2(zAxis, xAxis)) * Mathf.Rad2Deg + 90;
                transform.rotation = Quaternion.Euler(0, MoveDirection, 0);
            }


    }

    void FixedUpdate()
    {
        
    }
}
