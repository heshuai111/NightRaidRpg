using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  enum playerState
{
    None,
    Idle,
    Walk,
    Crouch,
    Run,
    Jump
}
public class Player_contrller : MonoBehaviour
{
  public playerState state = playerState.None;
    public playerState State{

        get
        {
            if (runing)
                state = playerState.Run;
            else if (walking)
                state = playerState.Walk;
            else if (crouching)
                state = playerState.Crouch;
            else if (jumpping)
                state = playerState.Run;
                state = playerState.Idle;
            
                
            return state;

        }


        }
    public float sprintSpeed = 10.0f;
    public float sprintJumpSpeed = 8.0f;
    public float normalSpeed = 6.0f;
    public float normalJumpSpeed = 7.0f;
    public float crouchSpeed = 2.0f;
    public float crouchJumpSpeed = 5.0f;
    public float crouchDeltaHeight = 0.5f;
    public float gravity = 20.0f;
    public float cameraMoveSpeed = 8.0f;
    public AudioClip jumpAudio;
    public Animator animator;
    private float speed;
    private float jumpSpeed;
    private Transform mainCamera;
    private float standardCamHeight;
    private float crouchingCamHeight;
    public bool grounded = false;
    private bool walking = false;
    private bool crouching = false;
    private bool stopCrouching = false;
    private bool runing = false;
    private bool idleing = true;
    private bool jumpping = false;
    private bool NormalAttacking = false;
    private bool Attack1ing = false;
    private bool Attack2ing = false;
    private CharacterAttackSystem attack;
    private Vector3 normalControllerCenter = Vector3.zero;
    private float normalCollerHeight = 0.0f;
    private float timer = 0;
    private CharacterController controller;
    private AudioSource audioSource;
    private fps_playerParameter1 parameter;
    private Vector3 moveDirection = Vector3.zero;
   public Animation animaton;
     void Start()
    {
        attack = this.GetComponent<CharacterAttackSystem>();
        animator = this.GetComponent<Animator>();
        crouching = false;
        walking = false;
        runing = false;
        jumpping = false;
        speed = normalSpeed;
        jumpSpeed = normalJumpSpeed;
        mainCamera = GameObject.FindGameObjectWithTag(Tags.mainCamera).transform;
        animaton =GameObject.FindGameObjectWithTag("Player").GetComponent<Animation>();
        standardCamHeight = mainCamera.localPosition.y;
        crouchingCamHeight = standardCamHeight - crouchDeltaHeight;
       // audioSource = this.GetComponent<AudioSource>();
        controller = this.GetComponent<CharacterController>();
        parameter = this.GetComponent<fps_playerParameter1>();
        normalControllerCenter = controller.center;
        normalCollerHeight = controller.height;
       

        
    }
    public void FixedUpdate()
    {
        UpdateMove();
        
    }
    private void  UpdateMove()
    {

        if (grounded)
        {
            moveDirection = new Vector3(parameter.inputMoveVector.x, 0, parameter.inputMoveVector.y);
            
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if(parameter.inputJump)
            {
                moveDirection.y = jumpSpeed;
              //  AudioSource.PlayClipAtPoint(jumpAudio, transform.position);
                CurrenSpeed();
            }
            
        }
        moveDirection.y -= gravity * Time.deltaTime;
   CollisionFlags  flags = controller.Move(moveDirection * Time.deltaTime);
        grounded = (flags & CollisionFlags.CollidedBelow) != 0;
        if(Mathf.Abs(moveDirection.x)>0&&grounded||Mathf.Abs(moveDirection.z)>0&&grounded)
        {
            if (parameter.inputSprint)
            {

                walking = true;
                idleing = false;
                runing = true;
                crouching = false;
                jumpping = false;

            }else if (parameter.inputGrough)
            {
                crouching = true;
                walking = true;
                idleing = false;
                runing = false;
                jumpping = false;

            } else if (parameter.inputJump)
            {
                walking = true;
                runing = true;
                jumpping = true;
                idleing = false;
                crouching = false;
                NormalAttacking = false;
                Attack1ing = false;
                Attack2ing = false;
            

            }
           
            
            else
            {
                walking = true;
                crouching = false;
                runing = false;
                jumpping = false;
                idleing = false;
            }
        }
        else
        {

            if (walking)
                walking = false;
            if (runing)
                runing = false;
            if (parameter.inputGrough)
                crouching = true;
            else
                crouching = false;

        }
        if(crouching)
        {
            controller.height = normalCollerHeight - crouchDeltaHeight;
            controller.center = normalControllerCenter - new Vector3(0, crouchDeltaHeight / 2, 0);
          
        }
        else
        {
            controller.height = normalCollerHeight;
            controller.center = normalControllerCenter;
        }
        if (walking)
        {

            animaton.CrossFade("n2017_walk");
           // animator.SetBool("walk", true);

        }else if(runing)
        {

          //  animaton.CrossFade("n2017_run");
          
        }
        else
        {
            animaton.CrossFade("n2017_idle");
        }
        
        if (runing)
        {

            //animator.SetBool("run", true);
        }
        else
        {
            //animator.SetBool("run", false);

        }
        if (jumpping)
        {

            //animator.SetTrigger("jump");
        }

      
        UpdateCrouch();
        CurrenSpeed();

    }
    private void CurrenSpeed()
    {

        switch (State)
        {
            case playerState.Idle:
                speed = normalSpeed;
                jumpSpeed = normalJumpSpeed;
              
                break;
            case playerState.Walk:
                speed = normalSpeed;
                jumpSpeed = normalJumpSpeed;
               
                break;
            case playerState.Crouch:
                speed = crouchSpeed;
                jumpSpeed = crouchJumpSpeed;
                break;
            case playerState.Run:
                speed = sprintSpeed;
                jumpSpeed = sprintJumpSpeed;
                break;


        }

    }

    private void AudioManagement()
    {
        if (State == playerState.Walk)
        {

            audioSource.pitch = 1.0f;
            if (!audioSource.isPlaying)
                audioSource.Play();

        }
        else if (State == playerState.Run)
        {

            audioSource.pitch = 1.3f;
            if (!audioSource.isPlaying)
                audioSource.Play();

        }
       // else

          //  audioSource.Stop();


    }
    private void UpdateCrouch()
    { if (crouching) {
            if (mainCamera.localPosition.y > crouchingCamHeight)
            {
                if (mainCamera.localPosition.y - (crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed) < crouchingCamHeight)
                    mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, crouchingCamHeight, mainCamera.localPosition.z);
                else
                    mainCamera.localPosition -= new Vector3(0, crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed, 0);

            }
            else
                mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, crouchingCamHeight, mainCamera.localPosition.z);

        }
        else
        {
            if (mainCamera.localPosition.y < standardCamHeight)
            {

                if (mainCamera.localPosition.y + (crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed) > standardCamHeight)
                    mainCamera.localPosition = new Vector3(mainCamera.localPosition.x, standardCamHeight, mainCamera.localPosition.z);
                else
                    mainCamera.localPosition += new Vector3(0, crouchDeltaHeight * Time.deltaTime * cameraMoveSpeed, 0);
            }
            else
                mainCamera.localPosition  = new Vector3(mainCamera.localPosition.x, standardCamHeight, mainCamera.localPosition.z);
            

        }

    }
    public  void SimpleMove(Vector3 targetpos)
    {
        transform.LookAt(targetpos);
        controller.SimpleMove(transform.forward * speed);

    }

}
