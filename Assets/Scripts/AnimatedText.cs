using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimatedText : MonoBehaviour
{

    public Animator animator;
    public TextMeshProUGUI text;

    public void Play(string message)
    {
        text.text = message;
        gameObject.SetActive(true);
        animator.SetTrigger("play");
    }

    public void AnimationComplete()
    {
        gameObject.SetActive(false);
    }

}
