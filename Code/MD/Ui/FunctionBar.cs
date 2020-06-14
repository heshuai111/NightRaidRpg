using UnityEngine;
using System.Collections;

public class FunctionBar : MonoBehaviour {

    public void OnStatusButtonClick() {
      //  Debug.Log("this is stause");
        Status._instance.TransformState();
    }
    public void OnBagButtonClick() {
        Inventory._instance.TransformState();
    }
    public void OnEquipButtonClick() {
        EquipmentUI._instance.TransformState();
    }
    public void OnSkillButtonClick() {
        SkillUI._instance.TransformState();
    }
    public void OnSettingButtonClick() {
    }


}
