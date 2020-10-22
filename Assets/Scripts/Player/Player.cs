using RTS;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool human;
    public string username;
    public TextMeshProUGUI TMP;
    public InConstructionBuilding buildingInHand;

    public List<WorldObject> SelectedObjects { get; set; }

    public int money;

    private void Start()
    {
        SelectedObjects = new List<WorldObject>();
        money = 100;
        TMP.text = ": " + money;
    }

    public void AddUnit(string unitName, Vector3 spawnPoint, Quaternion rotation)
    {
        GameObject Unit = ResourceManager.GetUnit(unitName);
        Unit.transform.position = spawnPoint;
        Unit.transform.rotation = rotation;
        Unit.GetComponent<Unit>().player = this;
        GameObject.Instantiate(Unit);
    }

    public GameObject AddBuilding(string unitName)
    {
        GameObject buildingPrefab = ResourceManager.GetBuilding(unitName);
        buildingPrefab.GetComponent<Building>().player = this;
        buildingPrefab.GetComponent<InConstructionBuilding>().player = this;
        GameObject buildingGameObject = GameObject.Instantiate(buildingPrefab);
        buildingInHand = buildingGameObject.GetComponent<InConstructionBuilding>();
        return buildingGameObject;
    }
    public void AddMoney(int price)
    {
        money += price;
        if (money < 0) Debug.LogError("argent négatif, pas normal");
        TMP.text = ": " + money;
    }
}
