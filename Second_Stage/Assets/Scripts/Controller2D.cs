using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : RaycastController {

    float maxClimbAngle = 80;
    float maxDescendAngle = 80;

    private SpriteRenderer _sr;
    public CollisionInfo collisions;
    [HideInInspector]
    public Vector2 playerInput;

    public override void Start() {
        base.Start();
        collisions.faceDir = 1;
        _sr = GetComponent<SpriteRenderer>();
    }

    public void Move(Vector2 _moveAmount, bool standingOnPlatform) {
        Move(_moveAmount, Vector2.zero, standingOnPlatform);
    }

    public void Move(Vector2 _moveAmount, Vector2 input, bool standingOnPlatform=false) {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.moveAmountOld = _moveAmount;
        playerInput = input;

        if (_moveAmount.y < 0) {
            DescendSlope(ref _moveAmount);
        }

        if (_moveAmount.x != 0) {
            collisions.faceDir = (int)Mathf.Sign(_moveAmount.x);
        }

        HorizontalCollisions(ref _moveAmount);
        if (_moveAmount.y != 0){
            VerticalCollisions(ref _moveAmount);
        }

        transform.Translate(_moveAmount);

        if (standingOnPlatform) {
            collisions.below = true;
        }
    }

    void HorizontalCollisions(ref Vector2 moveAmount) {
        float directionX = collisions.faceDir;
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        if (Mathf.Abs(moveAmount.x) < skinWidth) {
            rayLength = 2 * skinWidth;
        }

        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit){

                if (hit.distance == 0) {
                    continue;
                }
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxClimbAngle) {
                    if (collisions.descendingSlope) {
                        collisions.descendingSlope = false;
                        moveAmount = collisions.moveAmountOld;
                    }
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisions.slopeAngleOld) {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref moveAmount, slopeAngle, hit.normal);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
        //Flip
        if (directionX < 0) {
            _sr.flipX = true;
        } else if (directionX > 0) {
            _sr.flipX = false;
        }
    }

    void VerticalCollisions(ref Vector2 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLenght = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLenght, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit) {
                if (hit.collider.tag == "Through") {
                    if (directionY == 1 || hit.distance==0) {
                        continue;
                    }
                    if (collisions.fallingThroughPlatform) {
                        continue;
                    }
                    if (playerInput.y == -1) {
                        collisions.fallingThroughPlatform = true;
                        Invoke("ResetFallingThroughPlatform", .1f); //change .5f to .1f
                        continue;
                    }
                }

                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLenght = hit.distance;

                if (collisions.climbingSlope) {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
            
        }

        if (collisions.climbingSlope) {
            float directionX = Mathf.Sign(velocity.x);
            rayLenght = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLenght, collisionMask);

            if (hit) {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle) {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                    collisions.slopeNormal = hit.normal;
                }
            }
        }
    }

    void ClimbSlope(ref Vector2 moveAmount, float slopeAngle,Vector2 slopeNormal)
    {
        float moveDistance = Mathf.Abs(moveAmount.x);
        float climbVelocityY= Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (moveAmount.y <= climbVelocityY) {         
        moveAmount.y = climbVelocityY;
        moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
        collisions.below = true;
        collisions.climbingSlope=true;
        collisions.slopeNormal = slopeNormal;
        }
    }

    void DescendSlope(ref Vector2 moveAmount) {
        float directionX = Mathf.Sign(moveAmount.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit) {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
                if (Mathf.Sign(hit.normal.x) == directionX) {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x)) {
                        float moveDistance = Mathf.Abs(moveAmount.x);
                        float descendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                        moveAmount.y -= descendmoveAmountY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    void ResetFallingThroughPlatform() {
        collisions.fallingThroughPlatform = false;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public bool slidingDownMaxSlope;

        public float slopeAngle, slopeAngleOld;
        public Vector2 slopeNormal;
        public Vector2 moveAmountOld;
        public int faceDir;
        public bool fallingThroughPlatform;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slidingDownMaxSlope = false;
            slopeNormal = Vector2.zero;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

}
