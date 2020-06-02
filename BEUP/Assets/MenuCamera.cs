using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 10f;

    [SerializeField] float bobHeight = 15f;
    [SerializeField] float bobFactor = 0;
    [SerializeField] float bobSpeed = 0;

    [SerializeField] GameObject lookAt = null;


    private void LateUpdate()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, rotateSpeed * Time.deltaTime);

        Vector3 bobPosition = transform.position;
        bobPosition.y = bobHeight + Mathf.Sin(Time.time * bobFactor) * bobSpeed;

        transform.position = bobPosition;

        transform.LookAt(lookAt.transform);

    }
}
