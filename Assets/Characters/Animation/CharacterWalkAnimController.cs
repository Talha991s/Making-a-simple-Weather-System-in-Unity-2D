using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardinalDirection
{
    NORTH   = 0,
    EAST    = 1,
    SOUTH   = 2,
    WEST    = 3
}


public class CharacterWalkAnimController : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    CardinalDirection walkDirection = CardinalDirection.SOUTH;

    // Update is called once per frame
    void Update()
    {
     
       //Vector2 vel = rb.velocity;
       Vector2 vel = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // if we are moving at all
        bool isWalking = vel.magnitude > float.Epsilon;

       if (isWalking)
       {
            //
            bool isMovingHorizontally = Mathf.Abs(vel.x) > Mathf.Abs(vel.y);
           if (isMovingHorizontally) //moving left/right more than up/down
           {
               if (vel.x < 0) //moving left/WEST
               {
                   walkDirection = CardinalDirection.WEST;
               }
               else //moving right/EAST
               {
                   walkDirection = CardinalDirection.EAST;
               }
           }
           else // moving more vertically
           {
               if (vel.y < 0) //moving down/SOUTH
               {
                   walkDirection = CardinalDirection.SOUTH;
               }
               else //moving up/NORTH
               {
                   walkDirection = CardinalDirection.NORTH;
               }
           }
       } // if is walking

       animator.SetBool("IsWalking", isWalking);
       animator.SetInteger("WalkDirection", (int)walkDirection);
    }
}
