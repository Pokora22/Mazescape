using UnityEngine;
 
 [AddComponentMenu("Camera-Control/Smooth Mouse Look")]
 
 public class scr_MouseLook : MonoBehaviour 
 {
     public Vector2 clampInDegrees = new Vector2(360, 180);
     public Vector2 sensitivity = new Vector2(2, 2);
     public Vector2 smoothing = new Vector2(3, 3);
     public Vector2 targetCharacterDirection;
     public Vector2 targetDirection;
     private Vector2 _mouseAbsolute;
     private Vector2 _smoothMouse;
 
     public GameObject characterBody;
 
     public bool lockCursor;
 
     void Start()
     {
         targetDirection = transform.localRotation.eulerAngles;
 
         if(characterBody) 
         {
             targetCharacterDirection = characterBody.transform.localRotation.eulerAngles;
         }
     }
 
     void Update()
     {
         Cursor.lockState = CursorLockMode.Locked;
 
         var targetOrientation = Quaternion.Euler(targetDirection);
         var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);
 
         var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
 
         // Scale input against the sensitivity setting and multiply that against the smoothing value.
         mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));
 
         _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
         _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);
 
         _mouseAbsolute += _smoothMouse;
 
         if(clampInDegrees.x < 360) 
         {
             _mouseAbsolute.x = Mathf.Clamp (_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);
         }
 
         var xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
 
         transform.localRotation = xRotation;
 
         if(clampInDegrees.y < 360) 
         {
             _mouseAbsolute.y = Mathf.Clamp (_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);
         }
 
         transform.localRotation *= targetOrientation;
 
         if(characterBody)
         {
             var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, characterBody.transform.up);
             characterBody.transform.localRotation = yRotation;
             characterBody.transform.localRotation *= targetCharacterOrientation;
         }
 
         else
         {
             var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
             transform.localRotation *= yRotation;
         }
     }
 }