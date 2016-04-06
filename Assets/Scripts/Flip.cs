using UnityEngine;

public class Flip : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		if (animator.transform.eulerAngles.y <= 180)
		{
			animator.transform.Rotate(0, 180, 0);
		}
	}
}