using System.Collections;
using System.Collections.Generic;
using kurukuru;
using UnityEngine;

public class Gas : Enemy
{
	
	
    // Start is called before the first frame update
    void Start()
    {
        play=GameObject.FindWithTag("GameController").GetComponent<Play>();
		gameManager=GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        //hp = 30;
		dec = 10;
		inv = 2;
		//invtime = 5;
		unt = 1;
		//unttime = 3;
		ct = 5.0f;
		rt=5.0f;
		SetActPattern(20, 55, 25);
		play.EnemyInit(hp);
		 
    }

	protected override void Update()
	{
		base.Update();
		
		if(attackFlog)
		{
			enemyAnimator.SetTrigger("Atk");
			attackFlog=false;
		}
		else if(missAtkFlog)
		{
			enemyAnimator.SetTrigger("MissAtk");
            play.Grd();
			missAtkFlog=false;
		}
		else if(dedFlog)
		{
			enemyAnimator.SetTrigger("Ded");
            subAnimator.SetTrigger("Ded");
			dedFlog=false;
		}
		else if(pauseFlog)
		{
			enemyAnimator.enabled=false;
			pauseFlog=false;
		}
	}
}
