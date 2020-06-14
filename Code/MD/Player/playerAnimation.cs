using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimation : MonoBehaviour
{

    private Player_contrller move;
    private CharacterAttackSystem attack;
    Animation animation;
    // Start is called before the first frame update
    void Start()
    {
        move = this.GetComponent<Player_contrller>();
        animation = this.GetComponent<Animation>();
        attack = this.GetComponent<CharacterAttackSystem>(); 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (attack.state==PlayerAttackState.ctrlWalk )
        {
            if(move.state==playerState.Walk)
            playerAnim("n2017_walk");
            else if(move.state == playerState.Idle)
            {
                playerAnim("n2017_idle");
            }
        }else if (attack.state == PlayerAttackState.NormalAttack)
        {
            if(attack.attackState==AttackState.moving)
            playerAnim("n2017_run");
        }
    }
    void playerAnim(string Name)
    {

     //   animation.CrossFade(Name);

    }
}
