using UnityEngine;

public class CameraRotater : MonoBehaviour
{
    Rigidbody rb;

    [Header("Rotate")]
    public float mouseSpeed;
    float yRotation;
    float xRotation;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  
        Cursor.visible = false;                    

        rb = GetComponent<Rigidbody>();            
        rb.freezeRotation = true;                   

    }

    void Update()
    {
         Rotate();
    }

    void Rotate()   
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSpeed * Time.deltaTime;

        yRotation += mouseX;   
        xRotation -= mouseY;   

        xRotation = Mathf.Clamp(xRotation, -150f, 90f);  
        transform.localRotation = Quaternion.Euler(0, yRotation, 0);            
    }
}
