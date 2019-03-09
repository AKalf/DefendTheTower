using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    [SerializeField]
    float zoomSpeed = 1.0f;
    [SerializeField]
    float sensitivityX = 5.0f;
    [SerializeField]
    float sensitivityY = 5.0f;
    [SerializeField]
    float minimumX = -360.0f;
    [SerializeField]
    float maximumX = 360.0f;
    [SerializeField]
    float minimumY = -60.0f;
    [SerializeField]
    float maximumY = 60.0f;
    float rotationX = 0.0f;
    float rotationY = 0.0f;
    private List<float> rotArrayX = new List<float>();
    float rotAverageX = 0.0f;
    private List<float> rotArrayY = new List<float>();
    float rotAverageY = 0.0f;
    public float frameCounter = 60;
    Quaternion originalRotation;

    Camera camera;

    // Use this for initialization
    void Start () {
        
        originalRotation = transform.rotation;
        camera = GetComponent<Camera>();
    }
	
	// Update is called once per frame
	void Update () {
        RotateFace();
        Zoom();
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);
    }
    private void RotateFace()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            //Resets the average rotation
            rotAverageY = 0f;
            rotAverageX = 0f;

            float xInput = Input.GetAxis("Mouse Y");
            float yInput = Input.GetAxis("Mouse X");
            //Gets rotational input from the mouse
            rotationY += xInput * sensitivityY;
            rotationX += yInput * sensitivityX;
            //if (camera.fieldOfView < 90)
            //{
                //Adds the rotation values to their relative array
                rotArrayY.Add(rotationY);
                rotArrayX.Add(rotationX);

                //If the arrays length is bigger or equal to the value of frameCounter remove the first value in the array
                if (rotArrayY.Count >= frameCounter)
                {
                    rotArrayY.RemoveAt(0);
                }
                if (rotArrayX.Count >= frameCounter)
                {
                    rotArrayX.RemoveAt(0);
                }

                //Adding up all the rotational input values from each array
                for (int j = 0; j < rotArrayY.Count; j++)
                {
                    rotAverageY += rotArrayY[j];
                }
                for (int i = 0; i < rotArrayX.Count; i++)
                {
                    rotAverageX += rotArrayX[i];
                }

                //Standard maths to find the average
                rotAverageY /= rotArrayY.Count;
                rotAverageX /= rotArrayX.Count;

                //Clamp the rotation average to be within a specific value range
                rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
                rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

                //Get the rotation you will be at next as a Quaternion
                Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
                Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
                //Rotate
                transform.rotation = originalRotation * xQuaternion * yQuaternion;
            //}
            //else
            //{
            //    Debug.Log(rotArrayY.Count + ", " + rotArrayX.Count);
            //    if (rotArrayY.Count == 59 && rotArrayX.Count == 59)
            //    {

            //        //Clamp the rotation average to be within a specific value range
            //        rotationY = ClampAngle(rotationY, minimumY, maximumY);
            //        rotationX = ClampAngle(rotationX, minimumX, maximumX);

            //        //Get the rotation you will be at next as a Quaternion
            //        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
            //        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            //        //Rotate
            //        transform.rotation = originalRotation * xQuaternion * yQuaternion;
            //    }
            //    else
            //    {
            //        Debug.Log("Some");
            //        //If the arrays length is bigger or equal to the value of frameCounter remove the first value in the array
            //        if (rotArrayY.Count >= frameCounter)
            //        {
            //            rotArrayY.RemoveAt(0);
            //        }
            //        if (rotArrayX.Count >= frameCounter)
            //        {
            //            rotArrayX.RemoveAt(0);
            //        }

            //        //Adding up all the rotational input values from each array
            //        for (int j = 0; j < rotArrayY.Count; j++)
            //        {
            //            rotAverageY += rotArrayY[j];
            //        }
            //        for (int i = 0; i < rotArrayX.Count; i++)
            //        {
            //            rotAverageX += rotArrayX[i];
            //        }

            //        //Standard maths to find the average
            //        rotAverageY /= rotArrayY.Count;
            //        rotAverageX /= rotArrayX.Count;

            //        //Clamp the rotation average to be within a specific value range
            //        rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
            //        rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);

            //        //Get the rotation you will be at next as a Quaternion
            //        Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
            //        Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
            //        //Rotate
            //        transform.rotation = originalRotation * xQuaternion * yQuaternion;

            //    }
            //}
            
        }
        else if (axes == RotationAxes.MouseX)
        {
            rotAverageX = 0f;
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotArrayX.Add(rotationX);
            if (rotArrayX.Count >= frameCounter)
            {
                rotArrayX.RemoveAt(0);
            }
            for (int i = 0; i < rotArrayX.Count; i++)
            {
                rotAverageX += rotArrayX[i];
            }
            rotAverageX /= rotArrayX.Count;
            rotAverageX = ClampAngle(rotAverageX, minimumX, maximumX);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotAverageX, Vector3.up);
            transform.rotation = originalRotation * xQuaternion;
        }
        else
        {
            rotAverageY = 0f;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotArrayY.Add(rotationY);
            if (rotArrayY.Count >= frameCounter)
            {
                rotArrayY.RemoveAt(0);
            }
            for (int j = 0; j < rotArrayY.Count; j++)
            {
                rotAverageY += rotArrayY[j];
            }
            rotAverageY /= rotArrayY.Count;
            rotAverageY = ClampAngle(rotAverageY, minimumY, maximumY);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotAverageY, Vector3.left);
            transform.rotation = originalRotation * yQuaternion;
        }


    }
    public static float ClampAngle(float angle, float min, float max)
    {
        angle = angle % 360;
        if ((angle >= -360F) && (angle <= 360F))
        {
            if (angle < -360F)
            {
                angle += 360F;
            }
            if (angle > 360F)
            {
                angle -= 360F;
            }
        }
        return Mathf.Clamp(angle, min, max);
    }
    private void Zoom() {
        if (Input.GetMouseButton(1) && camera.fieldOfView > 45)
        {
            camera.fieldOfView -= Time.deltaTime * zoomSpeed;
        }
        else if (!Input.GetMouseButton(1) && camera.fieldOfView < 90) {
            camera.fieldOfView += Time.deltaTime * (zoomSpeed * 1.5f);
        }
    }
    public void SetSensitivity(float value) {
        sensitivityX = value;
        sensitivityY = value;
    }
    public float GetSensitivity() {
        return sensitivityX;
    }
}
