using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Mover : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] public SpriteRenderer spriteRenderer;

   public int jumpForce = 3;

    private RunAnimation _animation;

    private void Awake()
    {
        _animation = GetComponent<RunAnimation>();
    }

    public void Movement()
    {
        _animation.AnimationRun();
    }
}
