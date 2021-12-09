using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassSpawnPoint : MonoBehaviour
{
    private GlassSpawner glassSpawner;

    private void Start()
    {
        glassSpawner = GetComponentInParent<GlassSpawner>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Glass glass))
        {
            glassSpawner.AddGlass(glass);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Glass glass))
        {
            glassSpawner.RemoveGlass(glass);
        }
    }
}
