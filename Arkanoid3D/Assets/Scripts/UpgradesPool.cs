using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradePool : MonoBehaviour
{
    public static UpgradePool instance;

    [SerializeField] public GameObject[] upgradePrefabs;
    [SerializeField] public List<GameObject> upgradePool = new List<GameObject>();
    [SerializeField] public int poolSize;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < upgradePrefabs.Length; i++)
        {
            for(int j = 0; j < 5; j++)
            {
                AddUpgradesToPool(upgradePrefabs[i]);
            }
        }
    }

    private void AddUpgradesToPool(GameObject prefab)
    {
        GameObject upgrade = Instantiate(prefab);
        upgrade.SetActive(false);
        upgradePool.Add(upgrade);
    }

    public GameObject RequestUpgrade(Vector3 position)
    {
        int r = Random.Range(0, upgradePool.Count);
        upgradePool[r].transform.position = position;
        upgradePool[r].SetActive(true);
        return upgradePool[r];
    }
}
