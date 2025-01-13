using UnityEngine;

public class TriggerAnimationRound2 : MonoBehaviour
{
    public Animator Round2;

    public void PlayScaleAnimationRound2()
    {
        Round2.SetTrigger("ScaleRound2");
    }
}
