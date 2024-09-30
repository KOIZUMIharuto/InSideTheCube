using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetireGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Retire(){
        GameObject Cube = GameObject.FindWithTag("Cube");
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
}
