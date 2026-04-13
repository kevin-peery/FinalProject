using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Bar : MonoBehaviour
{
    public enum BarType
    {
        hp,
        energy
    }

    [Header("Inscribed")]
    [SerializeField]
    private BarType barType = BarType.hp;
    [SerializeField]
    private float widthMult = 0.25f;

    [Header("Dynamic")]
    private Character player;
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().playAs;
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 anchor = rectTransform.anchorMax;
        switch (barType)
        {
            case BarType.hp:
                anchor.x = (player.HP / player.maxHP) * widthMult;
                break;
            case BarType.energy:
                anchor.x = (player.Energy / player.maxEnergy) * widthMult;
                break;
            default:
                break;
        }

        rectTransform.anchorMax = anchor;
    }
}
