using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;

public class mousetest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.OnMouseDownAsObservable().Subscribe(x => Debug.Log("FUU"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
