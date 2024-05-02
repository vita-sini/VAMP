using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(RunAnimation), typeof(Mover))]
[RequireComponent(typeof(Vampirism))]
public class ReadingInput : MonoBehaviour
{
    private Animator _animator;
    private RunAnimation _animation;
    private Mover _movement;
    private Vampirism _vampirism;

    private void Start()
    {
        _animation = GetComponent<RunAnimation>();
        _animator = GetComponent<Animator>();
        _movement = GetComponent<Mover>();
        _vampirism = GetComponent<Vampirism>();
    }

    private void Update()
    {
        _movement.Movement();

        if (Input.GetMouseButton(0))
        {
            _animator.Play("Attack1");
        }

        if (Input.GetKey(KeyCode.Q))
        {
            _vampirism.ActivatingAbility();
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(_movement.speed * Time.deltaTime, 0, 0);
            _movement.spriteRenderer.flipX = false;
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(_movement.speed * Time.deltaTime * -1, 0, 0);
            _movement.spriteRenderer.flipX = true;
        }

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(0, _movement.speed * Time.deltaTime * _movement.jumpForce, 0);
        }
    }
}
