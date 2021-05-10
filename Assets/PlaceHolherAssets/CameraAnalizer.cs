using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnalizer : MonoBehaviour
{
    public Camera Camera;
    public float DistanceOfCamera = -10;
    public float PixelPerUnity=10;

    public float CameraOffSet;

    // Update is called once per frame
    void Update()
    {
      Debug.Log(Camera.aspect+ " du coup l'angle de la camera est de "+Camera.fieldOfView*Camera.aspect);


      float distance = (10 / (Camera.fieldOfView*Camera.aspect/ 2))*DistanceOfCamera;
      
      //10 / (Camera.farClipPlane/(Camera.pixelWidth / PixelPerUnity));
      transform.position = new Vector3(0, 0, -distance+CameraOffSet);
      //Debug.Log( distance +" avec une FOV de "+ Camera.focalLength / 2 );
    }
}
