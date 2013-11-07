using UnityEngine;
using System.Collections;

public class SpriteBillboard : MonoBehaviour
{
    private Transform cameraTransform;

	void Start ()
    {
        cameraTransform = Camera.mainCamera.transform;
	}
	
	void Update ()
    {
        transform.rotation = cameraTransform.rotation;
	}
}
