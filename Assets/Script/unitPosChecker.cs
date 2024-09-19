using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unitPosChecker : MonoBehaviour
{
    public GameObject Cube;

    public string[] unitPos = new string[3];

    

    public bool correctPos;

    void Start()
    {
        checkUnitPos();
        // 子オブジェクトのコンポーネントを取得する
        // Component[] components = this.GetComponentsInChildren<BoxCollider>();
        // Debug.Log(components);

        // foreach (Component component in components)
        // {
        //     if(component != null){
        //         component.enabled = false;
        //     }
        // }
        // BoxCollider[] components = GetComponents<BoxCollider>();  
        if(GetComponents<BoxCollider>() != null){
            for(int i = 0;i < GetComponents<BoxCollider>().Length; i++){
                GetComponents<BoxCollider>()[i].enabled = false;
            }
        }
        if(GetComponent<Rigidbody>() != null){
            GetComponent<Rigidbody>().isKinematic = true;
        }
        GetChildren(this.gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkUnitPos(){
        Vector3 pos = transform.localPosition;
        float[] posArr = {pos.x, pos.y, pos.z};
        float[] basePos = {21.0f, 0.0f, -21.0f};
        string[,] posName = {
            {"F", "S", "B"},
            {"U", "E", "D"},
            {"R", "M", "L"}
        };
        for(int i = 0; i < 3; i++){
            for(int j = 0; j < 3; j++){
                if(posArr[i] >= basePos[j] - 10.5f && posArr[i] < basePos[j] + 10.5f){
                    unitPos[i] = posName[i, j];
                    break;
                }
            }
        }
        if(gameObject.name == unitPos[0]+unitPos[1]+unitPos[2]+"unit"){
            correctPos = true;
        }else{
            correctPos = false;
        }
        
    }

    void GetChildren(GameObject obj) {
	Transform children = obj.GetComponentInChildren < Transform > ();
	//子要素がいなければ終了
	if (children.childCount == 0) {
		return;
	}
	foreach(Transform ob in children) {

        if(ob.GetComponent<BoxCollider>() != null){
            ob.GetComponent<BoxCollider>().enabled = false;
        }else if(ob.GetComponent<CapsuleCollider>() != null){
            ob.GetComponent<CapsuleCollider>().enabled = false;
        }else if(ob.GetComponent<SphereCollider>() != null){
            ob.GetComponent<SphereCollider>().enabled = false;
        }

		GetChildren(ob.gameObject);
	}
}
}
