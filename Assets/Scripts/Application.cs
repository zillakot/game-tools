using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Unity.Linq;
using System;
using System.Linq;
using UniRx.Diagnostics;
using UniRx.Triggers;

public enum Move
{
    Left,
    Right,
    Up,
    Down,
}

public class Application : MonoBehaviour
{
    // Start is called before the first frame update
    static readonly UniRx.Diagnostics.Logger logger = new UniRx.Diagnostics.Logger("test");
    public IObservable<long> clickStream;
    public static IObservable<string> KeyboardStream;
    public static IObservable<Vector3> MoveStream;
    public static IObservable<Vector3> MouseMoveStream;
    private IEnumerable<KeyCode> moveKeyCodes = new KeyCode[]{KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D};
    private IEnumerable<Vector3> moveVector = new Vector3[]{Vector3.forward, -Vector3.right, -Vector3.forward, Vector3.right};
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        InitLogger();
        logger.Log("logger test");
        KeyboardStream = Observable.EveryUpdate()
            .Where(_ => Input.anyKey)
            .Select(_ => Input.inputString);

        MoveStream = Observable.EveryUpdate()
            .Where(_ => Input.anyKey)
            .Select(_ => moveKeyCodes.Select(x => Input.GetKey(x)).ToList())
            .Where(x => x.Any(y => y))
            //.Do(x => Debug.Log(x.Select(y => y.ToString()).Aggregate((k,l) => k+l)))
            .Select(x => moveVector.Where((y,i) => x[i]).Aggregate((k, l) => (k + l)).normalized);
        
        MouseMoveStream = Observable.EveryUpdate()
            .Select(_ => Input.mousePosition)
            .DistinctUntilChanged();

        InitMouseInput();
    }
    void Start()
    {
        
        
    }

    private void InitLogger()
    {
        ObservableLogger.Listener.LogToUnityDebug();
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
        
        

        this.OnMouseDownAsObservable().Subscribe(x => logger.Log("FUU"));
        //KeyboardStream.Subscribe(x => logger.Log(x));
        /* 
        mouseDownStream.Subscribe(x => Debug.Log("MouseDown: " + x));
        mouseUpStream.Subscribe(x => Debug.Log("MouseUp: " + x));
        mouseMoveStream.Subscribe(x => Debug.Log("MouseMove: " + x));
        mouseDragStream.Subscribe(x => Debug.Log("Drag: " + x));

        GameObject cube;
        mouseDownStream.Subscribe(x => {
            cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = toRaycastPoint(x);
        }); */
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
