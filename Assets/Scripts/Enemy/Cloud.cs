using System.Collections;
using System.Collections.Generic;
using kurukuru;
using UnityEngine;

public class Cloud : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        play=GameObject.FindWithTag("GameController").GetComponent<Play>();
		gameManager=GameObject.FindWithTag("GameController").GetComponent<GameManager>();
       
		hp = mapData.Maps[ButtonManager.stageNumber-1].hp;          // 体力 HitPoint
		dec = mapData.Maps[ButtonManager.stageNumber-1].dec;       // 制限時間を減少させる値 Decrease
		inv = mapData.Maps[ButtonManager.stageNumber-1].inv;     // 暗闇にする個数 Invisible
		
		unt = mapData.Maps[ButtonManager.stageNumber-1].unt;     // 操作不可の個数 Untachable
		
		ct = mapData.Maps[ButtonManager.stageNumber-1].ct;        // 行動のクールタイム CoolTime
	
		rt = mapData.Maps[ButtonManager.stageNumber-1].rt;    // 行動の予測タイム ReadyTime
		actParameter=mapData.Maps[ButtonManager.stageNumber-1].actParameter;
		audioClip=mapData.Maps[ButtonManager.stageNumber-1].audioClip;
		audioSource=gameObject.GetComponent<AudioSource>();

		SetActPattern(actParameter);
		play.EnemyInit(hp);
		ctTemp=ct+UnityEngine.Random.Range(-2,2);
		 
    }

	protected override void Update()
	{
		base.Update();
		
		if(attackFlog)
		{
			enemyAnimator.SetTrigger("Atk");
			audioSource.PlayOneShot(audioClip);
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
