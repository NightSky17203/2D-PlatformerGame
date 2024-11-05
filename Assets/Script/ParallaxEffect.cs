using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera Camera;

    public Transform FollowTarget;

    Vector2 StartingPosition;

    float StartingZ;
    Vector2 camMovesinceStart => (Vector2)Camera.transform.position - StartingPosition;

    float zDistanceFromTarget => transform.position.z - FollowTarget.transform.position.z;

    float clippingPlane => (Camera.transform.position.z + (zDistanceFromTarget > 0 ? Camera.farClipPlane : Camera.nearClipPlane));

    float ParallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    // Start is called before the first frame update
    void Start()
    {
        StartingPosition = transform.position;
        StartingZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newPosition = StartingPosition + camMovesinceStart * ParallaxFactor;
        transform.position = new Vector3(newPosition.x, newPosition.y, StartingZ);
    }
}
