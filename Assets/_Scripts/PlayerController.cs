using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    PlayerControls _inputs;
    Vector2 _move;

    [Header("Player parameters")]
    [SerializeField] float _speed;
    [SerializeField] float _gravity = -30.0f;
    [SerializeField] CharacterController _controller;
    [SerializeField] float _jumpHeight = 3.0f;
    [SerializeField] Vector3 _velocity;
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundRadius = 0.5f;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] bool _isGrounded;

    private void Awake()
    {
        _inputs = new PlayerControls();
        _inputs.Player.Move.performed += context => _move = context.ReadValue<Vector2>();
        _inputs.Player.Jump.performed += context => jump();
        _controller = GetComponent<CharacterController>();
    }

    private void OnEnable() => _inputs.Enable();

    private void OnDisable() => _inputs.Disable();

    private void jump()
    {
        if (_isGrounded) { _velocity.y = Mathf.Sqrt(_jumpHeight * -2.0f * _gravity); }
        //Debug.Log("is jumping!");
    }

    private void FixedUpdate()
    {
        MyMovement();
        
    }

    private void MyMovement()
    {
        Vector2 moveInput = _inputs.Player.Move.ReadValue<Vector2>();
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }
        movement *= _speed * Time.fixedDeltaTime;
        _velocity.y += _gravity * Time.fixedDeltaTime;
        movement += _velocity * Time.fixedDeltaTime;
        _controller.Move(movement);
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundRadius, _groundMask);
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
    }
}
