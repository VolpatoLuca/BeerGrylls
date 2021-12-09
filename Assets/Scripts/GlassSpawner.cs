using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassSpawner : MonoBehaviour
{
    [SerializeField] private List<Glass> glasses = new List<Glass>();
    [SerializeField] private GameObject glassPrefab;
    [SerializeField] private Transform spawnPos;

    private void Update()
    {
        if (GameManager.instance.hasGameStarted)
        {
            if (glasses.Count == 0)
            {
                GameObject newGlass = Instantiate(glassPrefab, spawnPos.position, spawnPos.rotation);
                glasses.Add(newGlass.GetComponent<Glass>());
            }
        }

    }

    public void AddGlass(Glass newGlass)
    {
        if (!glasses.Contains(newGlass))
        {
            glasses.Add(newGlass);
        }
    }

    public void RemoveGlass(Glass oldGlass)
    {

        if (glasses.Contains(oldGlass))
        {

            glasses.Remove(oldGlass);
        }

    }

}
