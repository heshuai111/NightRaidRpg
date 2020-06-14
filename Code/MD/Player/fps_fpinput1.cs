using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fps_fpinput1 : MonoBehaviour
{


    public bool LockCursor
    {

        get { return Cursor.lockState == CursorLockMode.Locked ? true : false; }

            set{


         //   Cursor.visible = value;
           // Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;

            }


        }
    private fps_playerParameter1 parameter;
    private fps_input input;

    private void Start()
    {
        LockCursor = false;
        parameter = this.GetComponent<fps_playerParameter1>();
        input = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<fps_input>();
        
    }
    private void Update()
    {
        InitInput();
    }
    private void InitInput()
    {
        parameter.inputMoveVector =  new Vector2(input.GetAxis("Horizontal"), input.GetAxis("Vertical"));
        parameter.inputSmoothlook =  new Vector2(input.GetAxisRaw("Mouse X"), input.GetAxisRaw("Mouse Y"));
        parameter.inputGrough = input.GetButton("Grouch");
        parameter.inputJump = input.GetButton("Jump");
        //parameter.inputFire = input.GetButton("Fire");
        parameter.inputReload = input.GetButtonDown("Reload");
        parameter.inputSprint = input.GetButton("Sprint");
        parameter.NormalAttack = input.GetButton("NormalAttack");
        parameter.Attack1 = input.GetButton("Attack1");
        parameter.Attack2 = input.GetButton("Attack2");
        parameter.inputView = input.GetButton("View");



    }
}
