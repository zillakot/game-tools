using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Unity.Linq;
using System;
using System.Linq;

public class Application : MonoBehaviour
{
    // Start is called before the first frame update

    public IObservable<long> clickStream;
    void Start()
    {

        InitMouseInput();
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

        var clickObjectStream = mouseDownStream.Select(x => toRaycastHit(x).collider?.gameObject?.name);

        mouseDownStream.Subscribe(x => Debug.Log("MouseDown: " + x));
        mouseUpStream.Subscribe(x => Debug.Log("MouseUp: " + x));
        mouseMoveStream.Subscribe(x => Debug.Log("MouseMove: " + x));
        mouseDragStream.Subscribe(x => Debug.Log("Drag: " + x));
        clickObjectStream.Subscribe(x => Debug.Log("Click raycast: " + x));
            //.Buffer(2)
            //.Where(xs => xs.First() != xs.Last())
            //.A

        

        //clickStream.Buffer(clickStream.Throttle(TimeSpan.FromMilliseconds(250)))
        //    .Where(xs => xs.Count >= 2)
        //    .Subscribe(xs => Debug.Log("DoubleClick Detected! Count:" + xs.Count));
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
