using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeleteButton : MonoBehaviour
{
    float slide;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SlideOutLeft(){
        slide = -1;
        SlideOut(slide);
    }
    public void SlideOutRight(){
        slide = 1;
        SlideOut(slide);
    }
    void SlideOut(float i){
        transform.DOLocalMove(transform.localPosition + new Vector3(i*300f,0f,0f), 1f).SetEase(Ease.InBack);
    }

    public void SlideInLeft(){
        slide = -1;
        SlideIn(slide);
    }
    public void SlideInRight(){
        slide = 1;
        SlideIn(slide);
    }
    void SlideIn(float i){
        transform.DOLocalMove(transform.localPosition + new Vector3(i*300f,0f,0f), 1f).SetEase(Ease.OutBack).SetDelay(1f);
    }

    public void SlideOutUp(){
        transform.DOLocalMove(transform.localPosition + new Vector3(0f,500f,0f), 1f).SetEase(Ease.InBack);
    }
    public void SlideInDown(){
        transform.DOLocalMove(transform.localPosition + new Vector3(0f,-500f,0f), 1f).SetEase(Ease.OutBack);
    }
}
