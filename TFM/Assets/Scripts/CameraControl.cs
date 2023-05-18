using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public void MoveCamera(Transform newMapPosition)
    {
        transform.position = new Vector3(newMapPosition.position.x, newMapPosition.position.y, transform.position.z);
    }
}
