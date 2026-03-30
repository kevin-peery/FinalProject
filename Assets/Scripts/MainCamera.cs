using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private const int MIN_ZOOM = 1,
                      MAX_ZOOM = 3;

    [Header("Inscribed")]
    [SerializeField]
    [Range(MIN_ZOOM, MAX_ZOOM)]
    private int _zoomLevel = 2;
    [SerializeField]
    private float easingSpeed = 1;

    [Header("Dynamic")]
    public GameObject POI;
    Vector3 prevLocation;
    private int prevZoom;
    private float easing = 0;

    public int ZoomLevel
    {
        get { return _zoomLevel; }
        set
        {
            if (value < MIN_ZOOM)
                _zoomLevel = MIN_ZOOM;
            else if (value > MAX_ZOOM)
                _zoomLevel = MAX_ZOOM;
            else
                _zoomLevel = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        POI = GameObject.FindGameObjectWithTag("Player");
        prevZoom = ZoomLevel;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = POI.transform.position.x;
        pos.z = POI.transform.position.z;
        pos.y = ZoomLevel * 10 + POI.transform.position.y;

        if (ZoomLevel != prevZoom)
        {
            if (easing > 1)
                easing = 1;

            pos.y = Mathf.Lerp(prevLocation.y, pos.y, easing);

            if (easing == 1)
            {
                easing = 0;
                prevZoom = ZoomLevel;
            }
            else
                easing += Time.deltaTime / easingSpeed;
        }

        transform.position = pos;
        prevLocation = pos;

        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0)
        {
            if (scroll > 0)
                ZoomLevel--;
            else
                ZoomLevel++;
            
        }


    }


}
