using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;

    public float maxOrthographicSize = 100;
    public float minOrthographicSize = 4;

    Vector2 dragPoint;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float zoom = 1.0f+Input.mouseScrollDelta.y*0.1f;
        cam.orthographicSize = Mathf.Min(maxOrthographicSize, Mathf.Max(minOrthographicSize, cam.orthographicSize * zoom));

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = cam.nearClipPlane;
        Vector2 mouseWorldPos = cam.ScreenToWorldPoint(mousePos);

        if (Input.GetMouseButtonDown(1))
        {
            dragPoint = mouseWorldPos;
        }
        if (Input.GetMouseButton(1))
        {
            Vector3 diff = mouseWorldPos - dragPoint;
            diff.z = 0;
            cam.transform.position -= diff;
            dragPoint = mouseWorldPos-new Vector2(diff.x,diff.y);
        }
    }
}
