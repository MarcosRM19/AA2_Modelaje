using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fabrik : MonoBehaviour
{
    public List<Transform> Joints;
    public Transform target;
    public float tolerance = 1.0f;
    public float maxIterations = 1e5f;
    public float distanceDrone;
    public ShipController shipController;
    public float moveSpeed;
    public float rotationSpeed;

    private float lambda;
    private Vector3[] Links;
    private int countIterations;
    private int numberOfJoints;
    private Vector3 initialPosition;
    private List<Vector3> initPosition;
    private bool findTarget = true;
    private ShipController.ShipStates changeState;

    // Start is called before the first frame update
    void Start()
    {
        numberOfJoints = Joints.Count;
        getLinks();
        initialPosition = Joints[0].position;
        initPosition = new List<Vector3>();

        for(int i = 0; i < Joints.Count; i++)
            initPosition.Add(Joints[i].position);

    }

    // Update is called once per frame
    void Update()
    {
        if(findTarget)
        {
            if (countIterations < maxIterations && Vector3.Distance(Joints[numberOfJoints - 1].position, target.position) > tolerance)
            {
                PerformFABRIK();
                countIterations++;
            }

            if (Vector3.Distance(Joints[Joints.Count - 1].transform.position, target.transform.position) < distanceDrone)
            {
                shipController.ChangeState(changeState);
            }
        }
        else
        {
            ReturnToOriginalPositions();
        }
    }
    void getLinks()
    {
        Links = new Vector3[numberOfJoints - 1];
        for (int i = 0; i < numberOfJoints - 1; i++)
        {
            Links[i] = Joints[i + 1].position - Joints[i].position;
        }
    }

    void ReturnToOriginalPositions()
    {
        for (int i = 0; i < numberOfJoints; i++)
        {
            Joints[i].position = Vector3.MoveTowards(
                Joints[i].position,
                initPosition[i],
                moveSpeed /2 * Time.deltaTime
            );
        }
    }

    void PerformFABRIK()
    {
        Forward();
        Backward();
        AlignLastJoint();
    }

    void Forward()
    {
        Joints[numberOfJoints - 1].position = Vector3.MoveTowards(Joints[numberOfJoints - 1].position,target.position,moveSpeed * Time.deltaTime);
        for (int i = numberOfJoints - 2; i >= 0; i--)
        {
            float distance = Vector3.Magnitude(Links[i]);
            float denominator = Vector3.Distance(Joints[i].position, Joints[i + 1].position);
            lambda = distance / denominator;
            Vector3 temp = lambda * Joints[i].position + (1 - lambda) * Joints[i + 1].position;
            Joints[i].position = temp;

        }
    }

    void Backward()
    {
        Joints[0].position = initialPosition;

        for (int i = 1; i < numberOfJoints; i++)
        {
            float distance = Vector3.Magnitude(Links[i - 1]);
            float denominator = Vector3.Distance(Joints[i - 1].position, Joints[i].position);
            lambda = distance / denominator;
            Vector3 temp = lambda * Joints[i].position + (1 - lambda) * Joints[i - 1].position;
            Joints[i].position = temp;
        }

    }

    void AlignLastJoint()
    {
        Vector3 directionToTarget = target.position - Joints[Joints.Count - 1].position;
        Quaternion desiredRotation = Quaternion.LookRotation(-directionToTarget);

        Joints[Joints.Count - 1].rotation = Quaternion.RotateTowards(
            Joints[Joints.Count - 1].rotation,
            desiredRotation,
            rotationSpeed * Time.deltaTime
            );
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void SetSatet(ShipController.ShipStates state)
    {
        changeState = state;
    }

    public void SetFindTarget(bool state)
    {
        findTarget = state;
    }
}
