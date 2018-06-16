using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour {

    static public Transform target;

    public float distance = 4.0f;
    public float height = 1.0f;
    public float smoothLag = 0.2f;
    public float maxSpeed = 10.0f;
    public float snapLag = 0.3f;
    public float clampHeadPositionScreenSpace = 0.75f;

    LayerMask lineOfSightMask = 0;
    Vector3 headOffset = Vector3.zero;
    Vector3 centerOffset = Vector3.zero;

   // bool isSnapping = false;
    Vector3 velocity = Vector3.zero;
    float targetHeight = 100000.0f;

    void Apply(Transform dummyTarget, Vector3 dummyCenter)
    {
        Vector3 targetCenter = target.position + centerOffset;
        Vector3 targetHead = target.position + headOffset;

        targetHeight = targetCenter.y + height;

        //if(Input.GetButton("Fire2") && !isSnapping)
        //{
        //    velocity = Vector3.zero;
        //    isSnapping = true;
        //}

        //if (isSnapping)
        //{
        //    ApplySnapping(targetCenter);
        //}
        //else
        //{
        ApplyPositionDamping(new Vector3(targetCenter.x, targetHeight, targetCenter.z));
        //}

        SetupRotation(targetCenter, targetHead);
    }

    private void ApplyPositionDamping(Vector3 targetCenter)
    {
        Vector3 position = transform.position;
        Vector3 offset = position - targetCenter;
        offset.y = 0;
        Vector3 newTargetPosition = offset.normalized * distance + targetCenter;

        Vector3 newPosition;
        newPosition.x = Mathf.SmoothDamp(position.x, newTargetPosition.x, ref velocity.x, smoothLag, maxSpeed);
        newPosition.z = Mathf.SmoothDamp(position.z, newTargetPosition.z, ref velocity.z, smoothLag, maxSpeed);
        newPosition.y = Mathf.SmoothDamp(position.y, targetCenter.y, ref velocity.y, smoothLag, maxSpeed);

        newPosition = AdjustLineOfSight(newPosition, targetCenter);
        transform.position = newPosition;
    }

    private void ApplySnapping(Vector3 targetCenter)
    {
        Vector3 position = transform.position;
        Vector3 offset = position - targetCenter;
        offset.y = 0;
        float currentDistance = offset.magnitude;

        float targetAngle = target.eulerAngles.y;
        float currentAngle = transform.eulerAngles.y;

        currentAngle = Mathf.SmoothDampAngle(currentAngle, targetAngle, ref velocity.x, snapLag);
        currentDistance = Mathf.SmoothDamp(currentDistance, distance, ref velocity.z, snapLag);

        Vector3 newPosition = targetCenter;
        newPosition += Quaternion.Euler(0, currentAngle, 0) * Vector3.back * currentDistance;
        newPosition.y = Mathf.SmoothDamp(position.y, targetCenter.y + height, ref velocity.y, smoothLag, maxSpeed);
        newPosition = AdjustLineOfSight(newPosition, targetCenter);
        transform.position = newPosition;

        if(AngleDistance(currentAngle, targetAngle) < 3.0)
        {
            //isSnapping = false;
            velocity = Vector3.zero;
        }
    }

    private double AngleDistance(float currentAngle, float targetAngle)
    {
        currentAngle = Mathf.Repeat(currentAngle, 360);
        targetAngle = Mathf.Repeat(targetAngle, 360);

        return Mathf.Abs(targetAngle - currentAngle);
    }

    void SetupRotation(Vector3 centerPosition, Vector3 headPosition)
    {
        Vector3 cameraPosition = transform.position;
        Vector3 offsetToCenter = centerPosition - cameraPosition;

        Quaternion yRotation = Quaternion.LookRotation(new Vector3(offsetToCenter.x, 0.0f, offsetToCenter.z));

        Vector3 relativeOffset = Vector3.forward * distance + Vector3.down * height;
        transform.rotation = yRotation * Quaternion.LookRotation(relativeOffset);

        Ray centerRay = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 1.0f));
        Ray topRay = GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, clampHeadPositionScreenSpace, 1.0f));

        Vector3 centerRayPosition = centerRay.GetPoint(distance);
        Vector3 topRayPosition = topRay.GetPoint(distance);
        float centerToTopAngle = Vector3.Angle(centerRay.direction, topRay.direction);
        float heightToAngle = centerToTopAngle / (centerRayPosition.y - topRayPosition.y);
        float extraLookAngle = heightToAngle * (centerRayPosition.y - centerPosition.y);

        if(extraLookAngle < centerToTopAngle)
        {
            extraLookAngle = 0;
        }
        else
        {
            extraLookAngle = extraLookAngle - centerToTopAngle;
            transform.rotation = Quaternion.Euler(-extraLookAngle, 0, 0);
        }
    }

    private Vector3 AdjustLineOfSight(Vector3 newPosition, Vector3 target)
    {
        RaycastHit hit;
        if (Physics.Linecast(target, newPosition, out hit, lineOfSightMask.value))
        {
            velocity = Vector3.zero;
            return hit.point;
        }
        return newPosition;
    }

    private void LateUpdate()
    {
        if (target)
        {
            Apply(null, Vector3.zero);
        }
    }

    Vector3 GetCenterOffset()
    {
        return centerOffset;
    }

    void SetTarget(Transform t)
    {
        target = t;
    }
}
