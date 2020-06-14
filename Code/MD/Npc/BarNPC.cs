using UnityEngine;
using System.Collections;

public class BarNPC : MonoBehaviour {
	
	public static BarNPC _instance;
    public TweenPosition InventoryTween;
	
	void Awake() {
        _instance = this;
    }

    void OnMouseOver() {//当鼠标位于这个collider之上的时候，会在每一帧调用这个方法
        if (Input.GetMouseButtonDown(0)) {//当点击了老爷爷
            //GetComponent<AudioSource>().Play();
            ShowInventory();
        }
    }

    void ShowInventory() {
        InventoryTween.gameObject.SetActive(true);
        InventoryTween.PlayForward();
    }
	
    void HideInventory() {
        InventoryTween.PlayReverse();
    }
	
    //点击事件处理
    public void OnCloseButtonClick() {
        HideInventory();
    }
}
