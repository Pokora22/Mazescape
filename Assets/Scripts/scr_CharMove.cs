 using UnityEngine;
 
 public class scr_CharMove : MonoBehaviour 
 {
     private float jumpSpeed = 8.0F;
     private float gravity = 20.0F;
     private float speed = 10.0F;
 
     private Vector3 moveDirection = Vector3.zero;
 
     private CharacterController controller;
 
     void Start()
     {
         controller = GetComponent<CharacterController>();
     }
 
     void Update() 
     {
         CheckForWalk();
         CheckForSprint();
     }
         
     void CheckForWalk()
     {
         // Is the controller on the ground?
         if(controller.isGrounded) 
         {
             // Feed moveDirection with input.
             moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
             moveDirection = transform.TransformDirection(moveDirection);
 
             // Multiply it by speed.
             moveDirection *= speed;
 
             // Jumping.
             if(Input.GetButton("Jump")) 
             {
                 moveDirection.y = jumpSpeed;
             }
         }
 
         // Applying gravity to the controller.
         moveDirection.y -= gravity * Time.deltaTime;
 
         // Making the character move.
         controller.Move(moveDirection * Time.deltaTime);
     }
 
     void CheckForSprint()
     {
         if(controller.isGrounded && Input.GetKey(KeyCode.LeftShift)) 
         {
             speed = 11.0F;
 
             // Feed moveDirection with input.
             moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
             moveDirection = transform.TransformDirection (moveDirection);
 
             // Multiply it by speed.
             moveDirection *= speed;
 
             // Jumping.
             if(Input.GetKey(KeyCode.LeftShift)) 
             {
                 moveDirection.y = jumpSpeed;
             }
         } 
 
         else if(!Input.GetKey(KeyCode.LeftShift))
         {
             speed = 10.0F;
         }
     }
 }