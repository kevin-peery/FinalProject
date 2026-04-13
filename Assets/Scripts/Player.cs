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

    private const int MAX_FRAME_COUNT = 60;

    public static Player S {  get; private set; }

    [Header("Inscribed")]
    public Character playAs;

    [Header("Dynamic")]
    private List<Inputs> _inputTracker;
    Inputs currInputs;
    private List<float[]> _axesTracker;
    private float frameCount;

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

    void Awake()
    {
        S = this;
        InputTracker = new List<Inputs>();
        AxesTracker = new List<float[]>();
    }

    private void Start()
    {
        playAs.aimingGuide.playerControlled = true;
        
    }

    void Update()
    {
        frameCount += Time.deltaTime;
        frameCount %= 1;

        // Read current inputs
        int trackBack;
        Inputs input = 0;

        // Read axes
        float xAxis = Input.GetAxis("Horizontal"),
              zAxis = Input.GetAxis("Vertical"),
              hypotenuse = Mathf.Sqrt((xAxis * xAxis) + (zAxis * zAxis));

        if (hypotenuse > 1)
            hypotenuse = 1f;

        float[] axesInput = new float[] { xAxis, zAxis, hypotenuse };

        // Read mouse
        Vector3 mousePos = Input.mousePosition;
        mousePos = new Vector3(mousePos.x - (Screen.width / 2f), mousePos.y - (Screen.height / 2f));



        // Read button inputs
        if (Input.GetMouseButton(0))
        {
            input |= Inputs.MeleeAttack;
        }
        if (Input.GetMouseButton(1))
        {
            input |= Inputs.RangedAttack;
        }

        // Add to InputTracker
        if (InputTracker.Count >= MAX_FRAME_COUNT)
            InputTracker.RemoveAt(MAX_FRAME_COUNT - 1);
        if (AxesTracker.Count >= MAX_FRAME_COUNT)
            AxesTracker.RemoveAt(MAX_FRAME_COUNT - 1);

        InputTracker.Insert(0, input);
        AxesTracker.Insert(0, axesInput);

        // Read previous inputs
        float prevHypotenuse = 0;

        trackBack = 0;
        foreach (var axes in AxesTracker)
        {
            if (trackBack > 0)
            {
                prevHypotenuse = axes[2];
                break;
            }
            trackBack++;
        }

        // Move player
        playAs.Move(xAxis, zAxis, hypotenuse);

        // Rotate aim
        if (mousePos.x == 0)
        {
            if (mousePos.y < 0)
                playAs.AimDirection = 180f;
            else if (mousePos.y > 0)
                playAs.AimDirection = 0f;
        }
        else
            playAs.AimDirection = -(Mathf.Atan2(mousePos.y, mousePos.x)) * Mathf.Rad2Deg + 90;

        // Determine player action

        // Spin
        trackBack = 0;
        foreach (float[] axes in AxesTracker)
        {
            if (axes[2] < 0.5f)
                break;


        }

        // Guard
        trackBack = 0;
        bool guardCheck = false;
        foreach (Inputs i in InputTracker)
            if (i.HasFlag(Inputs.MeleeAttack))
            {
                trackBack++;
                if (trackBack >= MAX_FRAME_COUNT / 2f)
                {
                    playAs.Guard();
                    guardCheck = true;
                    break;
                }
            }
            else
                break;

        // Melee attack
        if (input.HasFlag(Inputs.MeleeAttack) && (!playAs.status.HasFlag(Character.Status.IsGuarding)))
            playAs.MeleeAttack();

        // Ranged attack
        if (input.HasFlag(Inputs.RangedAttack) && (!playAs.status.HasFlag(Character.Status.IsAttacking)))
            playAs.RangedAttack();
    }
}
