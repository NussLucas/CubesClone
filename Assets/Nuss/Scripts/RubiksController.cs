using UnityEngine;

public class RubiksCubeController : MonoBehaviour
{
    public float rotationSpeed = 200f;
    private bool isRotating = false;
    private Vector3 rotationAxis;
    private Vector3 startDragPosition;
    private Vector3 currentDragPosition;
    private GameObject selectedCube;
    private Transform rotationGroup;
    private Vector3 previousMousePosition;
    private Plane rotationPlane;

    void Update()
    {
        if (isRotating)
        {
            if (Input.GetMouseButton(0))
            {
                currentDragPosition = Input.mousePosition;
                Vector3 dragVector = currentDragPosition - previousMousePosition;

                if (dragVector.magnitude > 1) // Threshold to start rotation
                {
                    RotateGroup(dragVector);
                    previousMousePosition = currentDragPosition;
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                EndCubeRotation();
            }
        }
    }

    public void StartCubeRotation(GameObject cube, Vector3 hitPoint)
    {
        if (isRotating)
            return;

        selectedCube = cube;
        startDragPosition = Input.mousePosition;
        previousMousePosition = startDragPosition;
        rotationGroup = new GameObject("RotationGroup").transform;

        // Determine the rotation axis and plane
        Vector3 localHitPoint = cube.transform.InverseTransformPoint(hitPoint);
        rotationAxis = DetermineRotationAxis(localHitPoint);
        rotationPlane = new Plane(rotationAxis, cube.transform.position);

        // Add cubes to rotation group
        AddCubesToRotationGroup();

        isRotating = true;
    }

    void AddCubesToRotationGroup()
    {
        foreach (Transform child in transform)
        {
            if (Mathf.RoundToInt(Vector3.Dot(rotationAxis, child.position - selectedCube.transform.position)) == 0)
            {
                child.SetParent(rotationGroup);
            }
        }
    }

    void RotateGroup(Vector3 dragVector)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float enter;
        if (rotationPlane.Raycast(ray, out enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            Vector3 rotationVector = hitPoint - selectedCube.transform.position;
            float angle = Vector3.SignedAngle(startDragPosition - selectedCube.transform.position, rotationVector, rotationAxis);
            rotationGroup.Rotate(rotationAxis, angle, Space.World);
            startDragPosition = hitPoint;
        }
    }

    Vector3 DetermineRotationAxis(Vector3 localHitPoint)
    {
        if (Mathf.Abs(localHitPoint.x) > Mathf.Abs(localHitPoint.y) && Mathf.Abs(localHitPoint.x) > Mathf.Abs(localHitPoint.z))
        {
            return selectedCube.transform.right;
        }
        else if (Mathf.Abs(localHitPoint.y) > Mathf.Abs(localHitPoint.x) && Mathf.Abs(localHitPoint.y) > Mathf.Abs(localHitPoint.z))
        {
            return selectedCube.transform.up;
        }
        else
        {
            return selectedCube.transform.forward;
        }
    }

    void EndCubeRotation()
    {
        // Snap rotation to 90 degrees
        Vector3 eulerAngles = rotationGroup.eulerAngles;
        eulerAngles.x = Mathf.Round(eulerAngles.x / 90) * 90;
        eulerAngles.y = Mathf.Round(eulerAngles.y / 90) * 90;
        eulerAngles.z = Mathf.Round(eulerAngles.z / 90) * 90;
        rotationGroup.eulerAngles = eulerAngles;

        // Reassign cubes to main transform
        foreach (Transform child in rotationGroup)
        {
            child.SetParent(transform);
            child.localPosition = RoundVector3(child.localPosition);
            child.localEulerAngles = RoundVector3(child.localEulerAngles);
        }

        Destroy(rotationGroup.gameObject);
        isRotating = false;
    }

    Vector3 RoundVector3(Vector3 vector)
    {
        vector.x = Mathf.Round(vector.x);
        vector.y = Mathf.Round(vector.y);
        vector.z = Mathf.Round(vector.z);
        return vector;
    }
}
