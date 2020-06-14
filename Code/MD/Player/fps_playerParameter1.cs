using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class fps_playerParameter1 : MonoBehaviour
{
    [HideInInspector]
    public Vector2 inputSmoothlook;
    [HideInInspector]
    public Vector2 inputMoveVector;
    [HideInInspector]
    public bool inputGrough;
    [HideInInspector]
    public bool inputJump;
    [HideInInspector]
    public bool inputSprint;
    [HideInInspector]
    public bool inputView;
    [HideInInspector]
    public bool inputReload;
    [HideInInspector]
    public bool NormalAttack;
    [HideInInspector]
    public bool Attack1;
    [HideInInspector]
    public bool Attack2;

}
