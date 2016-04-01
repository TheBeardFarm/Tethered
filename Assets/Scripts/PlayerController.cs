using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    private Animator animator;

	// Use this for initialization
	void Start () {
        //animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        //ANIMATOR CODE IS FOR A 2D PROJECT - Uncomment if this is going to be a 2D game
        //And attach an animator with a "direction" int parameter.
        var leftKey = Input.GetKey("left");
        var rightKey = Input.GetKey("right");

        var move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        var speed = 5.0f;

        //If holding both keys, don't move
        if (leftKey && rightKey)
        {
            //animator.SetInteger("Direction", 0);
        }
        else if (leftKey)
        {
            //animator.SetInteger("Direction", -1);
            transform.position += move * speed * Time.deltaTime;
        }
        else if (rightKey)
        {
            //animator.SetInteger("Direction", 1);
            transform.position += move * speed * Time.deltaTime;
        }
        //If no keys down, don't move
        else
        {
            //animator.SetInteger("Direction", 0);
        }
	}
}
