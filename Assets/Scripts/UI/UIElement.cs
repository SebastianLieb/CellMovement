using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum ModifyerType
{
    Simulation,
    Cell,
    OriginPoint,
    GripPoint
}

public class UIElement : MonoBehaviour
{
    public CellSimulation simulation;
    public ModifyerType modifyerType = ModifyerType.Simulation;
    public string componentName;
    public string fieldName;

    public object value;

    // Start is called before the first frame update
    public virtual void Start()
    {
        PullValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PullValue()
    {
        Type target = null;
        switch (modifyerType)
        {
            case ModifyerType.Simulation:
                target = typeof(CellSimulation);
                break;
            case ModifyerType.Cell:
                target = typeof(Cell);
                break;
            case ModifyerType.OriginPoint:
                target = typeof(OriginPoint);
                break;
            case ModifyerType.GripPoint:
                target = typeof(GripPoint);
                break;
        }
        FieldInfo field = target.GetField(fieldName);
        value = field.GetValue(null);
    }

    public void PushValue()
    {
        Type target = null;
        switch (modifyerType)
        {
            case ModifyerType.Simulation:
                target = typeof(CellSimulation);
                break;
            case ModifyerType.Cell:
                target = typeof(Cell);
                break;
            case ModifyerType.OriginPoint:
                target = typeof(OriginPoint);
                break;
            case ModifyerType.GripPoint:
                target = typeof(GripPoint);
                break;
        }

        FieldInfo field = target.GetField(fieldName);
        field.SetValue(null, value);
    }
}
