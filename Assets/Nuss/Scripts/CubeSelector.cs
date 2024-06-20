using UnityEngine;

public class CubeSelector : MonoBehaviour
{
    private RubiksCubeController rubiksCubeController;

    void Start()
    {
        rubiksCubeController = FindObjectOfType<RubiksCubeController>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectCube();
        }
    }

    void SelectCube()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Cube"))
            {
                rubiksCubeController.StartCubeRotation(hit.transform.gameObject, hit.point);
            }
        }
    }
}
