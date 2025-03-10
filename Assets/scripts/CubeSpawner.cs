using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    private const int MinSplitValue = 2;
    private const int MaxSplitValue = 7;

    [SerializeField] private float _splitChance = 1f;
    [SerializeField] private Cube _cubePrefab;
    [SerializeField] private Exploder _exploder;
    [SerializeField] private ColorChanger _colorChanger;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnCube(transform.position + Random.insideUnitSphere, _splitChance);
        }
    }

    private Cube SpawnCube(Vector3 position, float spawnChance)
    {
        Cube newCube = Instantiate(_cubePrefab, position, Random.rotation);
        newCube.Initialize(this, spawnChance);

        return _colorChanger.ChangeColor(newCube);
    }

    public void Split(Cube cube)
    {
        if (Random.value > cube.SplitChance) return;

        float newSpawnChance = cube.SplitChance * 0.5f;
        int splitCount = Random.Range(MinSplitValue, MaxSplitValue);

        Cube[] newCubes = new Cube[splitCount];

        for (int i = 0; i < splitCount; i++)
        {
            Vector3 spawnPosition = cube.transform.position + Random.insideUnitSphere * 0.5f;
            Cube newCube = SpawnCube(spawnPosition, newSpawnChance);
            newCube.transform.localScale = cube.transform.localScale * 0.5f;

            newCubes[i] = newCube;
        }

        _exploder.Explode(newCubes);
    }
}