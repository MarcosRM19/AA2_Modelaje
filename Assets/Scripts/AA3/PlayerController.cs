using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public CCD[] taketarget;
    public Transform hand;   
    public Transform target;


    [Header("Parameters")]
    public float distance; 
    public Transform returnPosition;
    public float rotationSpeed;
    public float moveSpeed;

    private bool activeArms;   
    private bool droneInHand;
    private bool rotationComplete;

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < distance && !activeArms && !droneInHand)
        {
            activeArms = true;
        }

        if (activeArms && !droneInHand)
        {
            foreach (CCD t in taketarget)
            {
                t.enabled = true;
            }

            if (hand.childCount != 0)
            {
                droneInHand = true;
                StartCoroutine(RotatePlayer());
            }
        }

        if (droneInHand)
        {
            foreach (CCD t in taketarget)
            {
                t.enabled = false;
            }
        }
    }

    private IEnumerator RotatePlayer()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0, 180, 0); 
        float timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, timeElapsed);
            timeElapsed += Time.deltaTime * rotationSpeed;
            yield return null; 
        }

        transform.rotation = endRotation; 
        rotationComplete = true;

        StartCoroutine(MoveToPosition());
    }

    private IEnumerator MoveToPosition()
    {
        while (Vector3.Distance(transform.position, returnPosition.position) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, returnPosition.position, moveSpeed * Time.deltaTime);
            yield return null; 
        }

        transform.position = returnPosition.position;
    }
}

