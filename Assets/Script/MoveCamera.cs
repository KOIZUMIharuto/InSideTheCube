using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MoveCamera : MonoBehaviour
{
    public GameObject Cube;
    public GameObject Box;
    public GameObject RetireButtonPrefab;
    public GameObject Canvas;
    public bool pivotCameraBool;

    public GameObject pivotCameraObject;
    bool FinishPivotCameraBool;

    float a = 60;//初期視野角

    public GameObject[] collideUnit = new GameObject[5];
    
    public GameObject[] dragPosUnit = new GameObject[2];


    void Start()
    {
        pivotCameraBool = false;//視点変更許可
        
    }

    void Update()
    {
        //初期視点からの移動と視野角変更
        if(!pivotCameraBool){
            
            transform.LookAt(Cube.transform.position);
            this.GetComponent<Camera>().fieldOfView = a;
        }
        //--------------------------

        //視点回転中はfalse
        FinishPivotCameraBool = pivotCameraObject.GetComponent<pivotCamera>().finishPivotCameraBool;
        //視点移動後の処理(主にレイキャストとドラッグ)
        if(pivotCameraBool && FinishPivotCameraBool){
            if(Input.GetMouseButtonDown(0)){
                getRelation();
                getdragPosUnit(0);
            }
            if(Input.GetMouseButtonUp(0)){
                getdragPosUnit(1);
                Cube.GetComponent<RotateCubeUnit>().checkUnitsBool = true;
            }
        }

        
    }
    public void stratGame(){
        float moveTime = 3.0f;
        transform.parent = pivotCameraObject.transform;
        Cube.GetComponent<Rigidbody>().isKinematic = true;
        Cube.transform.DOMove(Cube.transform.position + new Vector3(0.0f, 20.0f, 0.0f), 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            transform.DOLocalMove(new Vector3(-31.0f, 0.0f, 0.0f), moveTime).SetEase(Ease.InOutQuart).OnComplete(() =>
            {
                pivotCameraBool = true;
                
                Cube.GetComponent<RotateCubeUnit>().playerControl = true;

                GameObject RetireButton  = Instantiate (RetireButtonPrefab, Vector3.zero, Quaternion.identity);
                RetireButton.transform.parent = Canvas.transform;
                RetireButton.transform.localPosition = new Vector3(-560, -340, 0);
            });
            DOTween.To(() => a, (x) => a = x, 80, moveTime).SetEase(Ease.InOutQuart);
        });
        int UnitCount = Cube.transform.childCount;
        for(int i = 0; i < UnitCount; i++){
            GameObject CubeUnit = Cube.transform.GetChild(i).gameObject;
            if(CubeUnit.GetComponents<BoxCollider>() != null){
                for(int j = 0;j < CubeUnit.GetComponents<BoxCollider>().Length; j++){
                    CubeUnit.GetComponents<BoxCollider>()[j].enabled = true;
                }
            }
        }
        Cube.GetComponent<BoxCollider>().enabled = false;
    
        
            
    }

    void getRelation(){
        //正面・上下左右のユニットを取得
        RaycastHit rayHit = new RaycastHit();
        float maxDistance = 500f;

        float[] screenX = {Screen.width/2.0f, Screen.width/2.0f, Screen.width/2.0f, Screen.width*0.65f, Screen.width*0.35f};
        float[] screenY = {Screen.height/2, Screen.height*0.8f, Screen.height*0.2f, Screen.height/2, Screen.height/2};
        float screenZ = 0.0f;
        
        for(int i = 0; i < 5; i++){
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX[i], screenY[i], screenZ));
            bool isHit = Physics.Raycast(ray,out rayHit, maxDistance);
            if(isHit){
                collideUnit[i] = rayHit.collider.gameObject;//unitNameに正面・上・下・右・左の順に格納
                //Debug.Log(collideUnit[i]);
            }
        }
        //--------------------------
    }

    void getdragPosUnit(int i){
        RaycastHit rayHit = new RaycastHit();
        float maxDistance = 500f;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool isHit = Physics.Raycast(ray,out rayHit, maxDistance);
        if(isHit){
            dragPosUnit[i] = rayHit.collider.gameObject;
        }
    }

    public void BackCameraMove(){
        float moveTime = 2.0f;
        transform.parent = Box.transform;
        pivotCameraBool = false;
        Cube.GetComponent<RotateCubeUnit>().playerControl = false;
        transform.DOLocalMove(new Vector3(90.0f, 120.0f, 180.0f), moveTime).SetEase(Ease.InOutQuart).OnComplete(() =>
        {
            Cube.GetComponent<BoxCollider>().enabled = true;
            Cube.GetComponent<Rigidbody>().isKinematic = false;
        });
        int UnitCount = Cube.transform.childCount;
        for(int i = 0; i < UnitCount; i++){
            GameObject CubeUnit = Cube.transform.GetChild(i).gameObject;
            if(CubeUnit.GetComponents<BoxCollider>() != null){
                for(int j = 0;j < CubeUnit.GetComponents<BoxCollider>().Length; j++){
                    CubeUnit.GetComponents<BoxCollider>()[j].enabled = false;
                }
            }
        }
        DOTween.To(() => a, (x) => a = x, 60, moveTime).SetEase(Ease.InOutQuart);
    }

    public void RetireCameraMove(){
        float moveTime = 2.0f;
        transform.parent = Box.transform;
        pivotCameraObject.transform.DORotate(Vector3.zero, 0.0f, RotateMode.LocalAxisAdd);
        pivotCameraBool = false;
        Cube.GetComponent<RotateCubeUnit>().playerControl = false;
        Cube.GetComponent<Rigidbody>().isKinematic = true;
        transform.DOLocalMove(new Vector3(90.0f, 120.0f, 180.0f), moveTime).SetEase(Ease.InOutQuart);
        // Cube.transform.DOLocalMove(Cube.transform.localPosition + new Vector3(0.0f, -20.0f, 0.0f), 1.5f).SetEase(Ease.OutSine);
        int UnitCount = Cube.transform.childCount;
        for(int i = 0; i < UnitCount; i++){
            GameObject CubeUnit = Cube.transform.GetChild(i).gameObject;
            if(CubeUnit.GetComponents<BoxCollider>() != null){
                for(int j = 0;j < CubeUnit.GetComponents<BoxCollider>().Length; j++){
                    CubeUnit.GetComponents<BoxCollider>()[j].enabled = true;
                }
                
            }
            if(CubeUnit.GetComponents<Rigidbody>().Length >0){
                CubeUnit.GetComponent<Rigidbody>().isKinematic = false;
            }
        }
        DOTween.To(() => a, (x) => a = x, 60, moveTime).SetEase(Ease.InOutQuart);
    }
}
