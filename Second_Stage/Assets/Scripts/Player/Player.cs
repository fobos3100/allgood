using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour {

    public float maxJumpHeight = 3;
    public float minJumpHeight = 1;
    public float timeToJumpApex = .3f;
    public float moveSpeed = 6;
    private float _accelerationTimeAirborne = .2f;
    private float _accelerationTimeGrounded = .1f;

    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    float timeToWallUnstick;

    private float _gravity;
    private float _maxJumpVelocity;
    private float _minJumpVelocity;
    private Vector3 _moveAmount;
    private float _velocityXSmoothing;

    Controller2D controller;

    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;

    //DASH
    Vector2 dashDirection;
    Vector2 dashInput;
    public const float maxDashTime = 1.0f;
    public float dashDistance = 10;
    public float dashStoppingSpeed = 0.1f;
    public float dashSpeed = 6;
    float currentDashTime = maxDashTime;

    //DOUBLE JUMP
    [SerializeField]
    int nrOfAlowedDJumps = 1; // New vairable
    int dJumpCounter = 0;     // New variable

    //Crouch
    public float crouchSlowMult=0.5f;
    public float crouchSizeMultY = 0.5f;

    private float _crouchMoveSpeed;
    private float _noCrouchMoveSpeed;

    private Vector2 _crouchColSize;
    private Vector2 _crouchColOffset;
    private Vector2 _noCrouchColSize;
    private Vector2 _noCrouchColOffset;

    private void Start() {
        controller = GetComponent<Controller2D>();

        _gravity = -(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        _maxJumpVelocity = Mathf.Abs(_gravity) * timeToJumpApex;
        _minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(_gravity) * minJumpHeight);

        _crouchMoveSpeed = moveSpeed * crouchSlowMult;
        _noCrouchMoveSpeed = moveSpeed;

        _noCrouchColSize = controller.collider.size;
        _noCrouchColOffset = controller.collider.offset;

        _crouchColSize = new Vector2(controller.collider.size.x, controller.collider.size.y * crouchSizeMultY);
        _crouchColOffset = new Vector2(controller.collider.offset.x, controller.collider.offset.y - (controller.collider.size.y * Mathf.Pow(crouchSizeMultY, 2)));

        print("Gravity: " + _gravity + "Jump Velocity: " + _maxJumpVelocity);
    }

    private void Update() {
        CalculateVelocity();
        HandWallSliding();

        controller.Move(_moveAmount * Time.deltaTime, directionalInput);

        if (controller.collisions.above || controller.collisions.below) {
            if (controller.collisions.slidingDownMaxSlope) {
                _moveAmount.y += controller.collisions.slopeNormal.y * -_gravity * Time.deltaTime;
            }
            else {
                _moveAmount.y = 0;
            }
        }
    }

    public void SetDirectionalInput(Vector2 input) {
        directionalInput = input;
    }

    public void OnJumpInputDown() {
        if (wallSliding) {
            if (wallDirX == directionalInput.x) {
                _moveAmount.x = -wallDirX * wallJumpClimb.x;
                _moveAmount.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0) {
                _moveAmount.x = -wallDirX * wallJumpOff.x;
                _moveAmount.y = wallJumpOff.y;
            }
            else {
                _moveAmount.x = -wallDirX * wallLeap.x;
                _moveAmount.y = wallLeap.y;
            }
        }
        if (controller.collisions.below) {
            dJumpCounter=0; //djump
            if (controller.collisions.slidingDownMaxSlope) {
                if (directionalInput.x != -Mathf.Sign(controller.collisions.slopeNormal.x)) {
                    _moveAmount.y = _maxJumpVelocity * controller.collisions.slopeNormal.y;
                    _moveAmount.x = _maxJumpVelocity * controller.collisions.slopeNormal.x;
                }
            } else {
                _moveAmount.y = _maxJumpVelocity;
            }
        }
        if (!controller.collisions.below && dJumpCounter < nrOfAlowedDJumps && !wallSliding) {
            _moveAmount.y = _maxJumpVelocity;
            dJumpCounter++;//djump
        }
    }

    public void OnJumpInputUp() {
        if (_moveAmount.y > _minJumpVelocity)
            _moveAmount.y = _minJumpVelocity;
    }

    void HandWallSliding() {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && _moveAmount.y < 0) {
            wallSliding = true;

            if (_moveAmount.y < -wallSlideSpeedMax) {
                _moveAmount.y = -wallSlideSpeedMax;
            }

            if (timeToWallUnstick > 0) {
                _velocityXSmoothing = 0;
                _moveAmount.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0) {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    void CalculateVelocity() {
        float targetVelocityX = directionalInput.x * moveSpeed;
        _moveAmount.x = Mathf.SmoothDamp(_moveAmount.x, targetVelocityX, ref _velocityXSmoothing, (controller.collisions.below) ? _accelerationTimeGrounded : _accelerationTimeAirborne);
        _moveAmount.y += _gravity * Time.deltaTime;
    }

    public void Dash() {
        currentDashTime = 0;
        dashInput=directionalInput;            
        if (currentDashTime < maxDashTime) {
            dashDirection = dashInput * dashDistance;
            currentDashTime += dashStoppingSpeed;
        } else {
            dashDirection = Vector2.zero;
        }
        controller.Move(dashDirection * dashSpeed * Time.deltaTime,false);
    }

    public void CrouchDown() {
        Vector2 curSize = controller.collider.size;
        Vector2 curOffset = controller.collider.offset;

        moveSpeed = _crouchMoveSpeed;
        controller.collider.size = _crouchColSize;
        controller.collider.offset = _crouchColOffset;
        controller.CalculateRaySpacing();
    }

    public void CrouchUp() {
        if (!controller.collisions.above) {
            Vector2 curSize = controller.collider.size;
            Vector2 curOffset = controller.collider.offset;

            moveSpeed = _noCrouchMoveSpeed;
            controller.collider.size = _noCrouchColSize;
            controller.collider.offset = _noCrouchColOffset;
            controller.CalculateRaySpacing();
        }
    }
}
