using UnityEngine;
public class AnimationManager : StaticInstance<AnimationManager>{
    public void PlayAnimation(ref Animator anim, string name)
    {
        anim.Play(name);
    }
    public void StopAnimation(ref Animator anim)
    {
        anim.StopPlayback();
    }
    public void ChangeAnimationSpeed(ref Animator anim, float speed)
    {
        anim.speed = speed;
    }
    public void Flip(bool isLeft,ref Animator anim)
    {
        if(isLeft)
        {
        anim.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        
    }
    //TODO make a state for hands
    
}