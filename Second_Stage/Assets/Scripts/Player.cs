using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

    public float jumpHeight=3;
    public float timeToJumpApex= .3f;
    public float moveSpeed = 6;

    private float _accelerationTimeAirborne=.2f;
    private float _accelerationTimeGrounded= .1f;
    private float _gravity;
    private float _jumpVelocity;
    private Vector3 _velocity;
    private float _velocityXSmoothing;

    Controller2D controller;

    //mine
    public float fallMultiplier=2.5f;
    public float lowJumpMultiplier=2f;

    void Start () {
        controller = GetComponent<Controller2D>();

        _gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        _jumpVelocity = Mathf.Abs(_gravity) * timeToJumpApex;
        print("Gravity: " + _gravity + "Jump Velocity: " + _jumpVelocity);
	}

	void Update () {

        if (controller.collisions.above || controller.collisions.below)
        {
            _velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetButton("Jump") && controller.collisions.below)
        {
            _velocity.y = _jumpVelocity;
        }

        float targetVelocityX = input.x * moveSpeed;
        _velocity.x = Mathf.SmoothDamp(_velocity.x, targetVelocityX, ref _velocityXSmoothing, (controller.collisions.below)?_accelerationTimeGrounded:_accelerationTimeAirborne);
        _velocity.y += _gravity * Time.deltaTime;
        controller.Move (_velocity * Time.deltaTime);

        //BetterJump();
	}

    void BetterJump()
    {
        if (_velocity.y < 0)
        {
            _velocity.y += _gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (_velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _velocity.y += _gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
