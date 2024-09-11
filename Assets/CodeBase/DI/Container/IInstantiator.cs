using UnityEngine;

namespace CodeBase.DI
{
    public interface IInstantiator
    {
        TImplementation InstantiatePrefabForComponent<TImplementation>(GameObject prefab, Vector3 position, Quaternion rotation, bool searchInChildren = false)
            where TImplementation : Component;
    }
}
