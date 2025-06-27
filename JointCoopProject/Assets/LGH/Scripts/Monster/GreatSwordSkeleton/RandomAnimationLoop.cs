using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAnimationLoop : StateMachineBehaviour
{
    public string[] attackClips = { "Attack1_1", "Attack1_2"};
    private float timer = 0f;
    public float interval = 1f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayRandomAttack(animator);
        timer = 0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            PlayRandomAttack(animator);
            timer = 0f;
        }
    }

    public void PlayRandomAttack(Animator animator)
    {
        string clipName = attackClips[Random.Range(0, attackClips.Length)];
        animator.Play(clipName);
    }
}
