using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    public Animator Round1;

    public void PlayScaleAnimationRound1()
    {
        Round1.Play("Round1_Anim");
        print("trigger");
    }

        public void PlayScaleAnimationRound2()
    {
		Round1.Play("Round2_Anim");
    }
}

