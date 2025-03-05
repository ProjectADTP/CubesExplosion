using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private const int MinSplitValue = 2;
    private const int MaxSplitValue = 7;

    [SerializeField] private GameObject _cubePrefab;
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private float _splitChance = 1f;

    private int _splitValue;

    private Transform _parentContainer;

    private void Start()
    {
        if (_cubePrefab == null) _cubePrefab = gameObject;

        _parentContainer = GameObject.Find("Cubs")?.transform;
    }

    private void OnMouseUpAsButton()
    {
        Split();
        Destroy(gameObject);
    }

    private void Split()
    {
        float randomValue = Random.value;

        if (randomValue <= _splitChance)
        {
            float newSpawnChance = _splitChance * 0.5f;

            _splitValue = Random.Range(MinSplitValue, MaxSplitValue);

            GameObject[] newCubes = new GameObject[_splitValue];

            for (int i = 0; i < _splitValue; i++)
            {
                Vector3 spawnPosition = transform.position + Random.insideUnitSphere * 0.5f;
                GameObject newCube = Instantiate(_cubePrefab, spawnPosition, Random.rotation, _parentContainer);

                newCube.transform.localScale = transform.localScale * 0.5f;
                newCube.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);

                newCubes[i] = newCube;

                if (newCube.TryGetComponent(out Cube spawner))
                {
                    spawner.SetSpawnChance(newSpawnChance);
                }
            }

            Explode(newCubes);
        }
    }

    private void Explode(GameObject[] cubes)
    {
        Instantiate(_effect, transform.position, transform.rotation);

        foreach (GameObject cube in cubes)
        { 
            if (cube.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }
    }

    public void SetSpawnChance(float newChance)
    {
        _splitChance = newChance;
    }
}


