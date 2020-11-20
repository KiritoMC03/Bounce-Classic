using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Animator _animator;
    private Collider2D _collider;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();

        if (_collider != null)
        {
            Debug.Log("!");
            _collider.enabled = false;
        }
    }

    public void Open()
    {
        _animator.SetBool("RingsEnded", true);
        _collider.enabled = true;
    }
}
