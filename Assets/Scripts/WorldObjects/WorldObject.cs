using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;

public class WorldObject : MonoBehaviour
{
    public string objectName;
    public GameObject SelectionRing;
    public Sprite buildImage;
    public int cost, sellValue, hitPoints, maxHitPoints;

    public Player player;
    protected string[] actions = { };
    protected bool currentlySelected = false;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public virtual void onGUI()
    {
        GameObject.Find("OrderPanel").GetComponent<OrderPanelScript>().ActualiseDisplaySprite(buildImage);
    }

    public void SetSelection(bool selected)
    {
        currentlySelected = selected;
        SelectionRing.SetActive(selected);
    }
    public string [] GetActions()
    {
        return actions;
    }

    public virtual void PerformAction(string actionToPerform)
    {

    }

    public virtual void RightMouseClick(GameObject hitObject, Vector3 hitpoint, Player controller, int index)
    {
        
    }
    protected virtual void Animate()
    {

    }

    public virtual bool MouseClick(GameObject hitObject, Player controller)
    {
        if(currentlySelected && hitObject)
        {
            WorldObject worldObject = hitObject.transform.root.GetComponent<WorldObject>();
            if (worldObject) return ChangeSelection(worldObject, controller);
        }
        return true;
    }

    private bool ChangeSelection(WorldObject worldObject, Player controller)
    {
        if (controller && controller.username == player.username)
        {
            SetSelection(false);
            return false;
        }
        else return true;
    }

    public string getPlayerName()
    {
        return player.username;
    }
}
