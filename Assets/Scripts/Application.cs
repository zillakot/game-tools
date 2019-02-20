using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Unity.Linq;
using System;
using System.Linq;
using UniRx.Diagnostics;

public class Application : MonoBehaviour
{
    // Start is called before the first frame update
    static readonly UniRx.Diagnostics.Logger logger = new UniRx.Diagnostics.Logger("test");
    public IObservable<long> clickStream;

    void Start()
    {
        InitLogger();
        InitMouseInput();
        
    }

    private void InitLogger()
    {
        ObservableLogger.Listener.LogToUnityDebug();
        ObservableLogger.Listener
        .Where(x => x.LogType == LogType.Exception)
        .Subscribe(x =>
        {
            Debug.Log(x.LoggerName + x.Message);
        });
    }

    private void InitMouseInput()
    {
        var mouseDownStream = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0))
            .Select(_ => Input.mousePosition);

        var mouseUpStream = Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonUp(0))
            .Select(_ => Input.mousePosition);

        var mouseMoveStream = Observable.EveryUpdate()
            .Select(_ => Input.mousePosition)
            .DistinctUntilChanged();

        var mouseDragStream = mouseMoveStream
            .SkipUntil(mouseDownStream)
            .TakeUntil(mouseUpStream)
            .Repeat();

        mouseDownStream.Subscribe(x => Debug.Log("MouseDown: " + x));
        mouseUpStream.Subscribe(x => Debug.Log("MouseUp: " + x));
        mouseMoveStream.Subscribe(x => Debug.Log("MouseMove: " + x));
        mouseDragStream.Subscribe(x => Debug.Log("Drag: " + x));

        GameObject cube;
        mouseDownStream.Subscribe(x => {
            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = toRaycastPoint(x);
        });
    }

    private Vector3 toRaycastPoint(Vector3 screenVector3)
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(screenVector3), out hit);
        return hit.point;
    }

    private RaycastHit toRaycastHit(Vector3 screenVector3)
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(screenVector3), out hit);
        return hit;
    }

    private GameObject toRaycastGameObject(Vector3 screenVector3)
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(screenVector3), out hit);
        return hit.collider.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
