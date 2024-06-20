using UnityEngine;

public class RubiksCubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab; // Prefab for individual cube
    public float spacing = 1.1f; // Distance between the cubes

    private GameObject[,,] rubiksCubes = new GameObject[3, 3, 3];

    void Start()
    {
        GenerateRubiksCube();
    }

    void GenerateRubiksCube()
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                for (int z = 0; z < 3; z++)
                {
                    Vector3 position = new Vector3(
                        (x - 1) * spacing,
                        (y - 1) * spacing,
                        (z - 1) * spacing
                    );

                    GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
                    cube.transform.parent = this.transform;
                    rubiksCubes[x, y, z] = cube;
                }
            }
        }
    }
}
