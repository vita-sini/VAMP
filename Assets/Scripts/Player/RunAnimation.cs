using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RunAnimation : MonoBehaviour
{
    private const string Run = "Run";

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void AnimationRun()
    {
        _animator.SetFloat(Run, Mathf.Abs(Input.GetAxisRaw("Horizontal")));
    }
}
