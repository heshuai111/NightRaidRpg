﻿using UnityEngine;
using System.Collections;

public class ShopWeaponNPC : MonoBehaviour {
    public void OnMouseOver() {//当鼠标在这个游戏物体之上的时候，会一直调用这个方法
        if (Input.GetMouseButtonDown(0)) {//弹出来武器商店
            //audio.Play();
            ShopWeaponUI._instance.TransformState();
        }
    }
}
