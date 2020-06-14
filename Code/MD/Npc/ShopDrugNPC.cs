using UnityEngine;
using System.Collections;

public class ShopDrugNPC : MonoBehaviour {


    public void OnMouseOver() {//当鼠标在这个游戏物体之上的时候，会一直调用这个方法
        if (Input.GetMouseButtonDown(0)) {//弹出来药品购买列表
            //audio.Play();
            ShopDrug._instance.TransformState();
        }
    }
	
}
