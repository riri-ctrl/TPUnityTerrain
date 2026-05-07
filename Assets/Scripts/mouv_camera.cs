using UnityEngine;

public class mouv_camera : MonoBehaviour
{
    void Start()
    {
        
    }
     public float speed;
    public Vector3 axis;
    public Vector3 axis2;
	
	
	void Update () {
        
        float arrowsInput = Input.GetAxis("Horizontal");
        float arrowsInput2 = Input.GetAxis("Vertical");
        
        float resultingSpeed = speed * Time.deltaTime * arrowsInput;
        float resultingSpeed2 = speed * Time.deltaTime * arrowsInput2;
        
        transform.Rotate(axis * resultingSpeed);
        transform.Rotate(axis2 * resultingSpeed2);
    }
}
