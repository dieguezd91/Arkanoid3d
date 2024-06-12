using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradePool : MonoBehaviour
{
    public static UpgradePool instance;

    [SerializeField] GameObject[] upgradePrefabs;
    [HideInInspector] public List<GameObject> upgradePool;
    [SerializeField] int _poolSize;

    public void Initialize()
    {
        instance = this;
        upgradePool = new List<GameObject>();
        for (int i = 0; i < upgradePrefabs.Length; i++)
        {
            for (int j = 0; j < _poolSize / upgradePrefabs.Length; j++)
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
        upgrade.transform.parent = transform;
    }

    public GameObject RequestUpgrade(Vector3 position)
    {
        int r = Random.Range(0, upgradePool.Count);
        upgradePool[r].transform.position = position;
        GameManager.instance.Upgrades.Add(upgradePool[r].GetComponent<Upgrade>());
        upgradePool[r].SetActive(true);
        return upgradePool[r];
    }
}
