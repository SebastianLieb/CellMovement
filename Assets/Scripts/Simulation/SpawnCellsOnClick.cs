using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnCellsOnClick : MonoBehaviour
{
    public EventSystem uiEventSystem;
    public CellSimulation cellSimulation;
    public Camera camera;

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(uiEventSystem);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsPointerOverUIObject() && Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = camera.nearClipPlane;
            Vector3 p = camera.ScreenToWorldPoint(mousePos);
            p.z = 0;
            cellSimulation.AddCell(p);
        }
    }
}
