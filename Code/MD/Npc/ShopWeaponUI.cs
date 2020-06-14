using UnityEngine;
using System.Collections;

public class ShopWeaponUI : MonoBehaviour {

    public static ShopWeaponUI _instance;
    public int[] weaponidArray;
    public UIGrid grid;
    public GameObject weaponItem;
    private TweenPosition tween;
    private bool isShow = false;
    private GameObject numberDialog;
    private UIInput numberInput;
    private int buy_id = 0;

    void Awake() {
        _instance = this;
        tween = this.GetComponent<TweenPosition>();
        numberDialog = transform.Find("Panel/NumberDialog").gameObject;
        numberInput = transform.Find("Panel/NumberDialog/NumberInput").GetComponent<UIInput>();
        numberDialog.SetActive(false);
    }


    void Start() {
        InitShopWeapon();
    }

    public void TransformState() {
        if (isShow) {
            tween.PlayReverse(); isShow = false;
        } else {
            tween.PlayForward(); isShow = true;
        }
    }

    public void OnCloseButtonClick() {
        TransformState();
    }

    void InitShopWeapon() {//初始化武器商店的信息
        foreach (int id in weaponidArray) {
            GameObject itemGo = NGUITools.AddChild(grid.gameObject, weaponItem);
            grid.AddChild(itemGo.transform);
            itemGo.GetComponent<ShopWeaponItem>().SetId(id);
        }
    }

    //ok按钮点击的时候
    public void OnOkBtnClick() {
        int count = int.Parse( numberInput.value );
        ObjectInfo info = ObjectsInfo._instance.GetObjectInfoById(buy_id);
        int price = info.price_buy;
        int price_total = price * count;
        bool success = Inventory._instance.GetCoin(price_total);
        if (success) {//取款成功，可以购买
            if (count > 0) {
                Inventory._instance.GetId(buy_id, count);
            }
        }
		
        buy_id = 0;
        numberInput.value = "0";
        numberDialog.SetActive(false);
    }
    public void OnBuyClick(int id) {
        buy_id = id;
        numberDialog.SetActive(true);
        numberInput.value = "0";
    }

    public void OnClick() {
        numberDialog.SetActive(false);
    }


}
