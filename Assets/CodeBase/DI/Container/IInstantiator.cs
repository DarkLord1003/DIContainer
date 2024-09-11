using UnityEngine;

namespace CodeBase.DI
{
    public interface IInstantiator
    {
        GameObject Instatiate(GameObject prefab);
        GameObject Instatiate(GameObject prefab, Vector3 position, Quaternion rotation);
    }
}
