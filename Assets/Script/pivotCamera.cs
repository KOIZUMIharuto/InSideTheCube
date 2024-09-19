using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class pivotCamera : MonoBehaviour
{
    public GameObject Camera;
    public bool finishPivotCameraBool;
    bool PivotCameraBool;//視点移動用

    void Start()
    {
        finishPivotCameraBool = true;//視点移動用
        

    }

    void Update()
    {
        //視点移動用
        PivotCameraBool = Camera.GetComponent<MoveCamera>().pivotCameraBool;
        if(PivotCameraBool && finishPivotCameraBool){
            pivotCameraFunction();
        }
        //----------


    }

    //視点移動用
    void pivotCameraFunction(){
        float pivotTime = 0.4f;

        if (Input.GetKeyDown(KeyCode.D)) {
            transform.DORotate(Vector3.up * 90.0f, pivotTime, RotateMode.LocalAxisAdd).OnStart(offPivotFunction).OnComplete(onPivotFunction);
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            transform.DORotate(Vector3.up * -90.0f, pivotTime, RotateMode.LocalAxisAdd).OnStart(offPivotFunction).OnComplete(onPivotFunction);
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            transform.DORotate(Vector3.forward * 90.0f, pivotTime, RotateMode.LocalAxisAdd).OnStart(offPivotFunction).OnComplete(onPivotFunction);
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            transform.DORotate(Vector3.forward * -90.0f, pivotTime, RotateMode.LocalAxisAdd).OnStart(offPivotFunction).OnComplete(onPivotFunction);
        }
    }
    void offPivotFunction(){
        finishPivotCameraBool = false;
    }
    void onPivotFunction(){
        finishPivotCameraBool = true;
    }
    //-----------
}
