using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CCD : MonoBehaviour
{
    [Header("Joints")]
    public List<Transform> joints;
    public List<Vector3> links;

    [Header("Target")]
    public Transform target;

    [Header("CCD Parameters")]
    public float tolerance = 1.0f;
    public float maxIterations = 1e5f;
    public float speed = 1.0f;

    // Private Parameters
    private int iterationCount = 0;
    private float rotation;
    private Vector3 axis;
    private int index = 0;

    private void Start()
    {
        SetLinks();
    }

    private void Update()
    {
        if (Vector3.Distance(joints.Last().position, target.position) > tolerance)
        {
            IncreaseJointCCD();
        }
    }

    #region CCD

    private void IncreaseJointCCD()
    {
        if (iterationCount < maxIterations)
        {
            // Ajustar índice dinámico.
            if (index >= joints.Count - 1) index = 0;
            else index++;

            Vector3 currentJoint = joints[index].position;

            Vector3[] referenceVectors = GetReferenceVectors(currentJoint);

            rotation = GetRotationAngle(referenceVectors);
            axis = GetRotationAxis(referenceVectors);

            UpdateChildJointsPositions();

            iterationCount++;
        }
    }

    private void UpdateChildJointsPositions()
    {
        Quaternion temporalQuaternion = Quaternion.AngleAxis(rotation * Mathf.Rad2Deg, axis);

        for (int i = index; i < joints.Count - 1; i++)
        {
            Vector3 targetPosition = joints[i].position + temporalQuaternion * links[i];

            joints[i + 1].position = Vector3.MoveTowards(
                joints[i + 1].position,
                targetPosition,
                speed * Time.deltaTime);

            Vector3 direction = (joints[i + 1].position - joints[i].position).normalized;
            joints[i + 1].position = joints[i].position + direction * links[i].magnitude;
        }

        UpdateJoints();
    }

    #endregion

    #region Support Functions

    private void SetLinks()
    {
        links.Clear();
        for (int i = 1; i < joints.Count; i++)
        {
            links.Add(joints[i].position - joints[i - 1].position);
        }
    }

    private Vector3[] GetReferenceVectors(Vector3 currentJointPosition)
    {
        Vector3[] referenceVectors = new Vector3[2];

        referenceVectors[0] = Vector3.Normalize(joints.Last().position - currentJointPosition);
        referenceVectors[1] = Vector3.Normalize(target.position - currentJointPosition);

        return referenceVectors;
    }

    private float GetRotationAngle(Vector3[] referenceVectors)
    {
        return Mathf.Acos(
            Mathf.Clamp(
                Vector3.Dot(referenceVectors[0], referenceVectors[1]),
                -1.0f,
                1.0f));
    }

    private Vector3 GetRotationAxis(Vector3[] referenceVectors)
    {
        return Vector3.Normalize(Vector3.Cross(referenceVectors[0], referenceVectors[1]));
    }

    private void UpdateJoints()
    {
        SetLinks();
    }

    #endregion
}
