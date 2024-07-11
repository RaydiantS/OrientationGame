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
             
                isWalking = true;
                ////animator.SetBool("isWalking", isWalking);
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

        isMoving = true;
       

        while ((transform.position - targetPosition).sqrMagnitude > Mathf.Epsilon)
        {

            Vector3 newPos = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            //uses a middle variable, and does not use targetPosition
            //this is to avoid changing the targetPosition to the player's position after only one iteration (which stops the loop)
            //Normally it takes about 35 loop for the player to move 1

            transform.position = newPos;
            //these two lines are the same as:
            //transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime

            yield return null;
        }


        isMoving = false;

    }

}