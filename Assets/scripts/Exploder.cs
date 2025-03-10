using UnityEngine;

public class Exploder : MonoBehaviour
{
    [SerializeField] private float _explosionRadius;
    [SerializeField] private float _explosionForce;
    [SerializeField] private ParticleSystem _effect;

    public void Explode(Cube[] cubes)
    {
        Instantiate(_effect, cubes[0].transform.position, cubes[0].transform.rotation);

        foreach (Cube cube in cubes)
        {
            if (cube.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }
    }
}
