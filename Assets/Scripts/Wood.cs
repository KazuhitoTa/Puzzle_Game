using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wood : Enemy
{
    // Start is called before the first frame update
    void Start()
    {
        gameManager=GameObject.FindWithTag("GameController").GetComponent<Play>();
        hp = 30;
		dec = 10;
		inv = 2;
		//invtime = 5;
		unt = 1;
		//unttime = 3;
		ct = 5.0f;
		rt=5.0f;
		SetActPattern(20, 25, 55);

		StartCoroutine(ActRoutine());  
    }

}
