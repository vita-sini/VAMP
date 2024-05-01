using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(RunAnimation))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Vampirism))]
public class ReadingInput : MonoBehaviour
{
    private Animator _animator;
    private RunAnimation _animation;
    private Movement _movement;
    private Vampirism _vampirism;

    private void Start()
    {
        _animation = GetComponent<RunAnimation>();
        _animator = GetComponent<Animator>();
        _movement = GetComponent<Movement>();
        _vampirism = GetComponent<Vampirism>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _animator.Play("Attack1");
        }

        _movement.Move();

        if (Input.GetKey(KeyCode.Q))
        {
            _vampirism.ActivatingAbility();
        }
    }
}
