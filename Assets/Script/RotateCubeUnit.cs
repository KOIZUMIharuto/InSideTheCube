using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RotateCubeUnit : MonoBehaviour
{
    const int unitsNum = 27;

    public GameObject Camera;

    public GameObject[] parentUnits0 = new GameObject[3];
    public GameObject[] parentUnits1 = new GameObject[3];
    public GameObject[] parentUnits2 = new GameObject[3];
    GameObject[,] parentUnits = new GameObject[3,3];

    public GameObject[] CubeUnits = new GameObject[unitsNum];

    public bool playerControl, autoShuffleBool, checkUnitsBool, checkUnitsPosBool;
    bool rotateUnitsBool;
    
    int shuffleTimes = 0;

    unitPosChecker script;

    //見てるとこのやつ
    GameObject[] CollideUnit = new GameObject[5];//unitNameに正面・上・下・右・左の順に格納
    GameObject[] DragPosUnit = new GameObject[2];
    //-------------

    GameObject[,,] eachUnitChilde = new GameObject[3,3,9];//[xyz方向,第n層,units]
    // {"F", "S", "B"}
    // {"U", "E", "D"}
    // {"R", "M", "L"}

    void Start()
    {
        playerControl = false;
        checkUnitsBool = false;
        rotateUnitsBool = false;

        // transform.DOLocalMove(new Vector3(transform.position.x, 40.0f, transform.position.z), 1.8f).SetEase(Ease.InCubic);
        for(int i = 0; i < 3; i++){
            parentUnits[0,i] = parentUnits0[i];
            parentUnits[1,i] = parentUnits1[i];
            parentUnits[2,i] = parentUnits2[i];
        }
        
    }

    void Update()
    {
        
        if(playerControl){
            if(checkUnitsBool){
                checkUnitsPosBool = false;
                getUnitsRelation();
                checkUnitsBool = false;
                rotateUnitsBool = true;
            }
            if(rotateUnitsBool){
                checkUnitsPos();
                setUnitsParent();

                rotateUnitsBool = false;
                
            }
        }else{
            if(autoShuffleBool){
                if(shuffleTimes < 30 && checkUnitsPosBool){
                    
                    autoShuffle();
                    shuffleTimes++;
                }else if(shuffleTimes >= 30){
                    autoShuffleBool = false;
                    GetComponent<Rigidbody>().isKinematic = false;
                }
            }
            
        }
         
        
    }

    void getUnitsRelation(){
        CollideUnit = Camera.GetComponent<MoveCamera>().collideUnit;
        DragPosUnit = Camera.GetComponent<MoveCamera>().dragPosUnit;
    }

    void checkUnitsPos(){//必要
        string[,] posName = {
            {"F", "S", "B"},
            {"U", "E", "D"},
            {"R", "M", "L"}
        };
        string[,] unitPos = new string[unitsNum,3];

        for(int i = 0; i < unitsNum; i++){
            for(int j = 0; j < 3; j++){
                unitPos[i,j] = CubeUnits[i].GetComponent<unitPosChecker>().unitPos[j];
            }
        }
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 3; j++){
                int l = 0;
                for(int k = 0; k < unitsNum; k++){
                    if(unitPos[k,i] == posName[i,j] && l < 9){
                        eachUnitChilde[i,j,l] = CubeUnits[k];
                        l++;
                    }
                }
            }
        }
    }

    void setUnitsParent(){
        getUnitsRelation();
        string[] frontUnitPos = CollideUnit[0].GetComponent<unitPosChecker>().unitPos;
        string[] upUnitPos = CollideUnit[1].GetComponent<unitPosChecker>().unitPos;
        string[] downUnitPos = CollideUnit[2].GetComponent<unitPosChecker>().unitPos;
        string[] rightUnitPos = CollideUnit[3].GetComponent<unitPosChecker>().unitPos;
        string[] leftUnitPos = CollideUnit[4].GetComponent<unitPosChecker>().unitPos;

        string[] dragStartUnitPos = DragPosUnit[0].GetComponent<unitPosChecker>().unitPos;
        string[] dragFinishUnitPos = DragPosUnit[1].GetComponent<unitPosChecker>().unitPos;

        string frontUnit = null;

        //正面の取得
        for(int i = 0; i < 3; i++){
            if(frontUnitPos[i] == upUnitPos[i] && frontUnitPos[i] == rightUnitPos[i]){
                frontUnit = frontUnitPos[i];
                // Debug.Log(frontUnit);
            }
        }
        //動かそうとしている面の取得
        string rotationUnit = null;
        for(int i = 0; i < 3; i++){
            if(dragStartUnitPos[i] != frontUnit && dragFinishUnitPos[i] != frontUnit){
                if(dragStartUnitPos[i] == dragFinishUnitPos[i]){
                    rotationUnit = dragFinishUnitPos[i];
                }
            }
        }

        //メモ
        string[,] posName = {
            {"F", "S", "B"},
            {"U", "E", "D"},
            {"R", "M", "L"}
        };

        //動かす面の取得　rotationUnitNumだけ必要
        int[] rotationUnitNum = new int[2];
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 3; j++){
                if(posName[i,j] == rotationUnit){
                    rotationUnitNum[0] = i;
                    rotationUnitNum[1] = j;
                }
            }
        }

        //動かす面のグループ化　必要
        GameObject parentUnit = parentUnits[rotationUnitNum[0],rotationUnitNum[1]];
        for(int i = 0; i < 9; i++){
            eachUnitChilde[rotationUnitNum[0],rotationUnitNum[1],i].transform.parent = parentUnit.transform;
        }

        //回転軸指定　rotationAxisだけ必要
        Vector3 rotationAxis = Vector3.zero;
        switch (rotationUnitNum[0])
        {
            case 0:
                rotationAxis = Vector3.right;
                break;
            case 1:
                rotationAxis = Vector3.up;
                break;
            case 2:
                rotationAxis = Vector3.back;
                break;
        }
        int[] frontUnitNum = new int[2];
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 3; j++){
                if(posName[i,j] == frontUnit){
                    frontUnitNum[0] = i;
                    frontUnitNum[1] = j;
                }
            }
        }
        if(frontUnitNum[1] == 2){
            rotationAxis = -rotationAxis;
        }
        if(frontUnitNum[0] == 1){
            rotationAxis = -rotationAxis;
        }else if(frontUnitNum[0] == 2){
            if(rotationUnitNum[0] == 1){
                rotationAxis = -rotationAxis;
            }
        }
        
        //回転方向　rotationAngleだけ必要
        float rotationAngle = 0.0f;
        for(int i = 0; i < 3; i++){
            if(dragStartUnitPos[i] != frontUnit && dragStartUnitPos[i] != rotationUnit){
                int dragStartUnitPosNum = 0;
                int dragFinishUnitPosNum = 0;
                for(int j = 0; j < 3; j++){
                    for(int k = 0; k < 3; k++){
                        if(posName[j,k] == dragStartUnitPos[i]){
                            dragStartUnitPosNum = k;
                        }else if(posName[j,k] == dragFinishUnitPos[i]){
                            dragFinishUnitPosNum = k;
                        }
                    }
                }
                if(dragStartUnitPosNum < dragFinishUnitPosNum){
                    rotationAngle = 90.0f;
                }else{
                    rotationAngle = -90.0f;
                }
            }
        }

        //回転　必要
        parentUnit.transform.DORotate(rotationAxis * rotationAngle, 0.3f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutQuart).OnComplete(() =>
        {
            for(int i = 0; i < 9; i++){
                eachUnitChilde[rotationUnitNum[0],rotationUnitNum[1],i].transform.parent = this.transform;
            }
            for(int i = 0; i < unitsNum; i++){
                script = CubeUnits[i].GetComponent<unitPosChecker>();
                script.checkUnitPos();
            }
            parentUnit.transform.DORotate(-rotationAxis * rotationAngle, 0f, RotateMode.LocalAxisAdd);
            checkUnitsPosBool = true;
        });
    }

    void shuffle(){
        for(int i = 0; i < unitsNum; i++){
                script = CubeUnits[i].GetComponent<unitPosChecker>();
                script.checkUnitPos();
            }
        checkUnitsPos();
        int[] rotationUnitNum = {Random.Range(0,3),Random.Range(0,3)};

        GameObject parentUnit = parentUnits[rotationUnitNum[0],rotationUnitNum[1]];
        for(int i = 0; i < 9; i++){
            eachUnitChilde[rotationUnitNum[0],rotationUnitNum[1],i].transform.parent = parentUnit.transform;
        }

        Vector3 rotationAxis = Vector3.zero;
        switch (rotationUnitNum[0])
        {
            case 0:
                rotationAxis = Vector3.right;
                break;
            case 1:
                rotationAxis = Vector3.up;
                break;
            case 2:
                rotationAxis = Vector3.back;
                break;
        }
        rotationAxis = rotationAxis * (Random.Range(0,2) * 2 - 1);
        
        float rotationAngle = 90.0f * (Random.Range(0,2) * 2 - 1);
        
        parentUnit.transform.DORotate(rotationAxis * rotationAngle, 0.1f, RotateMode.LocalAxisAdd).SetEase(Ease.InOutQuart).OnComplete(() =>
        {
            for(int i = 0; i < 9; i++){
                eachUnitChilde[rotationUnitNum[0],rotationUnitNum[1],i].transform.parent = this.transform;
            }

            for(int i = 0; i < unitsNum; i++){
                script = CubeUnits[i].GetComponent<unitPosChecker>();
                script.checkUnitPos();
            }
            parentUnit.transform.DORotate(-rotationAxis * rotationAngle, 0f, RotateMode.LocalAxisAdd);
            checkUnitsPosBool = true;
        });
    }

    public void autoShuffle(){
        checkUnitsPosBool = false;
        shuffle();
    }

    
    public void autoShuffles(){
        shuffleTimes = 0;
        GetComponent<Rigidbody>().isKinematic = true;
        transform.DOMove(transform.position + new Vector3(0.0f, 20.0f, 0.0f), 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            autoShuffleBool = true;
            checkUnitsPosBool = true;
        });
        
    }

    public void RetireGame(){
        for(int i = 0; i < unitsNum; i++){
            if(CubeUnits[i].GetComponents<BoxCollider>() != null){
                for(int j = 0;j < CubeUnits[i].GetComponents<BoxCollider>().Length; j++){
                    CubeUnits[i].GetComponents<BoxCollider>()[j].enabled = true;
                }
            }
            if(CubeUnits[i].GetComponents<Rigidbody>().Length >0){
                CubeUnits[i].GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-1.0f,1.0f), Random.Range(-1.0f,1.0f), Random.Range(-1.0f,1.0f))*5f, ForceMode.VelocityChange);
            }
        }
        this.GetComponent<BoxCollider>().enabled = false;
    }

    public void resetUnits(){
        for(int i = 0; i < unitsNum; i++){
            string UnitName = CubeUnits[i].name;
            float[] UnitOriginalPos = new float[3];
            string[,] posName = {
                {"F", "S", "B"},
                {"U", "E", "D"},
                {"R", "M", "L"}
            };

            if(CubeUnits[i].GetComponents<BoxCollider>() != null){
                for(int j = 0;j < CubeUnits[i].GetComponents<BoxCollider>().Length; j++){
                    CubeUnits[i].GetComponents<BoxCollider>()[j].enabled = false;
                }
            }
            if(CubeUnits[i].GetComponents<Rigidbody>().Length >0){
                CubeUnits[i].GetComponent<Rigidbody>().isKinematic = true;
            }

            for(int j = 0; j < 3; j++){
                string UnitPosName = UnitName.Substring(j, 1);
                for(int k = 0; k < 3; k++){
                    if(UnitPosName == posName[j,k]){
                        UnitOriginalPos[j] = -21*(k-1);
                    }
                }
            }
            float tweenTime = 1.5f;
            CubeUnits[i].transform.DOLocalMove(
                new Vector3(
                    UnitOriginalPos[0],
                    UnitOriginalPos[1],
                    UnitOriginalPos[2]
                ),
                tweenTime
            ).SetEase(Ease.OutSine).OnComplete(() =>
            {
                CubeUnits[i].GetComponent<unitPosChecker>().checkUnitPos();
            });
            CubeUnits[i].transform.DOLocalRotate(Vector3.zero, tweenTime);

            this.GetComponent<BoxCollider>().enabled = true;
            
        }
    }
}

