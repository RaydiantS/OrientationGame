using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    private float moveSpeed = 5f;

    public bool isMoving;

    public bool isWalking;

    private Vector2 input;

    //private Animator animator;

    public LayerMask solidLayer;

    private int debugTurningTimes = 0; //debugVar 2

    //Detect Collisions and store the boolean value inside this variable
    public bool isWalkable(Vector3 targetPosition)
    {
        //Detect Collisions
        if (Physics2D.OverlapCircle(targetPosition, 0.2f, solidLayer) != null)
        {
            isMoving = false;
            return false;
        }
        else
        {
            return true;
        }
    }


    //public void Awake()
    //{
    //    animator = GetComponent<Animator>();
    //}

    private void Update()
    {
        //When isMoving is false, dl the following (isMoving is always switching between true & false)
        if (!isMoving)
        {
            //Geting horizontal & vertical inputs
            input.x = Input.GetAxisRaw("Horizontal") / 4;
            input.y = Input.GetAxisRaw("Vertical") / 4;

            //Switch btw walking & static animations (Use isWalking instead of isMoving because isWalking
            //stays true or false for a long time while isMoving oscillates btw true and false frequently
            if (input.x != 0 || input.y != 0)
            {
                //Debug.Log("This is input.x: " + input.x);
                //Debug.Log("And this is input.y: " + input.y);
                isWalking = true;
                ////animator.SetBool("isWalking", isWalking);
                //Debug.Log("isWalking is TRUE");
            }
            else
            {
                isWalking = false;
                ////animator.SetBool("isWalking", isWalking);
                //Debug.Log("isWalking is FALSE");
            }

            //Switching directions of animations and detect movement when there is input
            if (input != Vector2.zero)
            {
                //To switch direction (of animations)
                ////animator.SetFloat("moveX", input.x);
                ////animator.SetFloat("moveY", input.y);

                //Locate where the player is going to
                var targetPosition = transform.position + new Vector3(input.x, input.y, 0);

                //set isMoving to true to stop getting input temporarily
                isMoving = true;

                //If there is no collisions, start the coroutine to move there
                if (isWalkable(targetPosition))
                {
                    StartCoroutine(Move(targetPosition));
                    //Debug.Log("### start coroutine ###");
                }


            }
        }

    }

    IEnumerator Move(Vector3 targetPosition)
    {
        //Debug.Log("@@@ Move from --- " + transform.position + " --- to --- " + targetPosition);

        isMoving = true;
        //Debug.Log("*** isMoving SET TRUE");

        //Debug.Log("*** START move");

        //int dbgLpCnt = 0; //debugVar 1

        while ((transform.position - targetPosition).sqrMagnitude > Mathf.Epsilon)
        {
            //dbgLpCnt++; //debugVar 1

            // Debug.Log("*** IN while loop ***");

            Vector3 newPos = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            //uses a middle variable, and does not use targetPosition
            //this is to avoid changing the targetPosition to the player's position after only one iteration (which stops the loop)
            //Normally it takes about 35 loop for the player to move 1

            transform.position = newPos;
            //these two lines are the same as:
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime

            yield return null;
        }

        //Debug.Log("*** while loop " + dbgLpCnt + "x"); //debugVar 1

        //yield return new WaitForSeconds(1f);

        isMoving = false;
        //Debug.Log("*** isMoving SET FALSE");

        //Debug.Log("*** STOP move");

        //yield return null;


    }

}
//Bug Description: When the player collides into something, it will stop, but will keep walking into the collider without further control. 