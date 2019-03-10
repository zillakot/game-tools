using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;


public class MovementController : MonoBehaviour
{
    // Start is called before the first frame update
    float speed = 0.1f;
    CompositeDisposable streams = new CompositeDisposable();
    Camera mycam;

    void Start()
    {
        mycam = this.GetComponentInChildren<Camera>();
        
        streams.Add(
            Application.MoveStream.Subscribe(x => this.transform.position += transform.TransformVector(x) * speed)
        );
        //streams.Add(

        //.Subscribe(x => transform.position = Camera.main.ScreenToViewportPoint(Input.mousePosition))
        //);
        Observable.EveryFixedUpdate()
            .Select(_ => Tuple.Create(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")))
            .DistinctUntilChanged()
            .Subscribe(x => {
                MouseLookRotate3(x.Item1, x.Item2);
            });
        //this.transform.Rotate( new Vector3(x.Item2,x.Item1,0))
    }

     IDisposable RotateCameraStream()
    {
        //Application.MouseMoveStream.Scan((x,y)=> (y-x)*0.1f).Skip(1).Subscribe(x => this.transform.rotation = Quaternion.Euler(-x.y,x.x,0));
        return null;//Observable.EveryFixedUpdate().Select(_ => Cros)
    }

    void MouseLookRotate1(float mouseX, float mouseY)
    {

        this.transform.RotateAround(transform.position, new Vector3(0,1,0), mouseX);
        this.transform.RotateAround(transform.position, this.transform.right, mouseY);
    }

    void MouseLookRotate2(float mouseX, float mouseY)
    {
        var newforward = Quaternion.Euler(mouseY,mouseX,0) * transform.forward;
        Debug.DrawRay(transform.position, newforward, Color.green, 1.0f);
        transform.rotation = Quaternion.LookRotation(newforward, Vector3.up);
    }

    void MouseLookRotate3(float mouseX, float mouseY)
    {
        transform.localRotation *= Quaternion.Euler(0,mouseX,0);
        mycam.transform.localRotation = ClampRotationAroundXAxis (Quaternion.Euler(-mouseY,0,0) * mycam.transform.localRotation);
        //transform.rotation *= Quaternion.Euler(mouseY,mouseX,0);
        Debug.DrawRay(transform.position, mycam.transform.forward * 5, Color.green, 1.0f);
        //transform.rotation = Quaternion.LookRotation(newforward, Vector3.up);
    }

    // Update is called once per frame
    void Update()
    {
    }   

    void OnDestroy()
    {
        streams.Dispose();
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);
        angleX = Mathf.Clamp (angleX, -90f, 90f);
        q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);
        return q;
    }
}
