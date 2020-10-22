using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS;
using System.Runtime.InteropServices;

public class UserInput : MonoBehaviour
{
    public Transform selectionAreaTransform;
    public OrderPanelScript OPS;

    private Player player;
    private bool isDragging;
    private Vector3 firstPointBox, secondPointBox, firstMousePos;
  
    // Start is called before the first frame update
    void Start()
    {
        player = transform.root.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.human)
        {
            MoveCamera();
            MouseActivity();
            OPS.ReinitDisplay();
            if (player.SelectedObjects.Count == 1)
            {
                InConstructionBuilding ICB = player.SelectedObjects[0].gameObject.GetComponent<InConstructionBuilding>();
                if (player.SelectedObjects[0] is Building && ICB && !ICB.isConstruct)
                {
                        ICB.onGUI();
                }
                else
                {
                    player.SelectedObjects[0].onGUI();
                }
            }
        }
    }

    private void MoveCamera()
    {
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;
        Vector3 movement = Vector3.zero;
        Vector3 origin = Camera.main.transform.position;

        //HORIZONTAL CAMERA MOVEMENT
        if (xpos >= 0 && xpos < ResourceManager.ScroolWidth)
        {
            movement.x -= ResourceManager.ScrollSpeed;
        }
        else if (xpos <= Screen.width && xpos > Screen.width - ResourceManager.ScroolWidth)
        {
            movement.x += ResourceManager.ScrollSpeed;
        }
        //VERTICAL CAMERA MOVEMENT
        if (ypos >= 0 && ypos < ResourceManager.ScroolWidth)
        {
            movement.z += ResourceManager.ScrollSpeed;
        }
        else if (ypos <= Screen.height && ypos > Screen.height - ResourceManager.ScroolWidth)
        {
            movement.z -= ResourceManager.ScrollSpeed;
        }
        movement = Camera.main.transform.TransformDirection(movement);
        //HEIGHT CAMERA MOVEMENT
        movement.z = movement.y;        //  ???
        movement.y = 0;
        movement.y -= ResourceManager.ScrollSpeed  * Input.mouseScrollDelta.y;
        //CAMERA MOVE

        Vector3 destination = origin + movement;
        if (destination.y > ResourceManager.MaxCameraHeight) destination.y = ResourceManager.MaxCameraHeight;
        else if (destination.y < ResourceManager.MinCameraHeight) destination.y = ResourceManager.MinCameraHeight;
        if (destination != origin)
        {
            Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed * (origin.y / 2));
        }
    }

    private void MouseActivity()
    {
        if (MouseGameArea())
        {
            if (player.buildingInHand)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (player.buildingInHand.Drop())
                    {
                        player.AddMoney(-player.buildingInHand.cost);
                        int index = 0;
                        foreach (WorldObject SelectedObject in player.SelectedObjects)
                        {
                            if (SelectedObject is BuilderUnit)
                            {
                                BuilderUnit SelectedObjectBuilder = (BuilderUnit)SelectedObject;
                                SelectedObjectBuilder.target = player.buildingInHand.gameObject;
                                SelectedObjectBuilder.RightMouseClick(player.buildingInHand.gameObject, player.buildingInHand.gameObject.transform.position, player, index);
                            }
                            SelectedObject.SetSelection(false);
                            index++;
                        }
                        player.SelectedObjects.Clear();
                        player.buildingInHand.SetSelection(true);
                        player.SelectedObjects.Add(player.buildingInHand);
                        player.buildingInHand = null;
                    }
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    
                    GameObject.Destroy(player.buildingInHand.gameObject);
                    player.buildingInHand = null;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    isDragging = false;
                    firstMousePos = Input.mousePosition;
                    secondPointBox = Input.mousePosition;

                    Vector3 tmp = FindHitPoint();
                    if (tmp != ResourceManager.InvalidPosition)
                        firstPointBox = tmp;
                }
                else if (Input.GetMouseButton(0))
                {
                    if (!isDragging)
                    {
                        Vector3 hitPoint = FindHitPoint();
                        if ((hitPoint != ResourceManager.InvalidPosition) && Vector3.Distance(firstPointBox, hitPoint) > 1)
                        {
                            isDragging = true;
                            DrawDragBox();
                            selectionAreaTransform.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        Vector3 tmp = FindHitPoint();
                        if (tmp != ResourceManager.InvalidPosition)
                            secondPointBox = tmp;
                        DrawDragBox();
                    }
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    selectionAreaTransform.gameObject.SetActive(false);

                    if (!isDragging)
                    {
                        NotDraggedClick();

                    }
                    else
                    {
                        DraggedClick();
                    }
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    Vector3 tmp = FindHitPoint();
                    if (tmp != ResourceManager.InvalidPosition)
                        firstPointBox = tmp;

                    GameObject hitObject = FindHitObject();

                    if (hitObject && firstPointBox != ResourceManager.InvalidPosition)
                    {
                        //Le joueur avait deja selectionné quelque chose
                        if (player.SelectedObjects.Count > 0)
                        {
                            int index = 0;
                            foreach (WorldObject SelectedObject in player.SelectedObjects)
                            {
                                SelectedObject.RightMouseClick(hitObject.gameObject, firstPointBox, player, index);
                                index++;
                            }
                        }
                    }
                }


            }
        }else if (!player.buildingInHand)
        {
            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                selectionAreaTransform.gameObject.SetActive(false);
                DraggedClick();
            }
        }
        
    }

    private void DraggedClick()
    {
        List<WorldObject> hitObjects = FindHitUnitsOwnedByPlayer();
        foreach (WorldObject SelectedObject in player.SelectedObjects)
        {
            SelectedObject.SetSelection(false);
        }
        player.SelectedObjects = hitObjects;
        foreach (WorldObject SelectedObject in player.SelectedObjects)
        {
            SelectedObject.SetSelection(true);
        }
    }

    private void NotDraggedClick()
    {

        Vector3 tmp = FindHitPoint();
        if (tmp != ResourceManager.InvalidPosition)
            firstPointBox = tmp;
        if (FindHitObject())
        {
            WorldObject hitObject = FindHitObject().transform.root.GetComponent<WorldObject>();
            if (hitObject && firstPointBox != ResourceManager.InvalidPosition)
            {
                //Le joueur avait deja selectionné quelque chose
                if (player.SelectedObjects.Count > 0)
                {
                    List<WorldObject> staySelected = new List<WorldObject>();
                    foreach (WorldObject SelectedObject in player.SelectedObjects)
                    {
                        if (SelectedObject.MouseClick(hitObject.gameObject, player))
                            staySelected.Add(SelectedObject);
                    }
                    player.SelectedObjects = staySelected;
                    if (staySelected.Count == 0)
                    {
                        player.SelectedObjects.Add(hitObject);

                        hitObject.SetSelection(true);
                    }
                }
                //Le joueur n'avait rien selectionné
                else if (hitObject.tag != "Ground")
                {
                    WorldObject worldObject = hitObject.transform.root.GetComponent<WorldObject>();
                    if (worldObject)
                    {
                        player.SelectedObjects.Add(worldObject);

                        worldObject.SetSelection(true);
                    }
                }
            }
            else
            {
                foreach (WorldObject SelectedObject in player.SelectedObjects)
                {
                    SelectedObject.SetSelection(false);
                }
                player.SelectedObjects.Clear();
                Debug.Log("reset : " + player.SelectedObjects.Count);

            }
        }
    }

    public bool MouseGameArea()
    {
        Vector3 mousePos = Input.mousePosition;
        bool insideWitdh = mousePos.y >= 0 + ResourceManager.ORDERS_BAR_WIDTH && mousePos.y <= Screen.width;
        bool insideHeight = mousePos.y >= 0 && mousePos.y <= Screen.height - ResourceManager.RESOURCE_BAR_HEIGHT;
        return insideWitdh && insideHeight;
    }

    private GameObject FindHitObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    private List<WorldObject> FindHitUnitsOwnedByPlayer()
    {
        Vector3 lowerLeft = new Vector3(
       Mathf.Min(firstPointBox.x, secondPointBox.x), 0,
       Mathf.Min(firstPointBox.z, secondPointBox.z)
       );
        Vector3 upperRight = new Vector3(
            Mathf.Max(firstPointBox.x, secondPointBox.x), 0,
            Mathf.Max(firstPointBox.z, secondPointBox.z)
            );
        Vector3 center = lowerLeft + ((upperRight - lowerLeft) / 2) ;
        Vector3 halfExtents = (upperRight - lowerLeft) /2;
        halfExtents.y = 100;
        Collider[] hitcollider = Physics.OverlapBox(center, halfExtents);
        List<WorldObject> unitsOwnedCollider = new List<WorldObject>();
        for(int i = 0; i < hitcollider.Length; i++)
        {
            WorldObject wO = hitcollider[i].transform.root.GetComponent<WorldObject>();
            if (wO && wO.getPlayerName() == player.username && wO is Unit && unitsOwnedCollider.Count < ResourceManager.MAXUNITINCHARGE)
             unitsOwnedCollider.Add(wO);
        }
        return unitsOwnedCollider;
    }

    private Vector3 FindHitPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) return hit.point;
        return ResourceManager.InvalidPosition;
    }

    private void DrawDragBox()
    {
        Vector3 lowerLeft = new Vector3(
           Mathf.Min(firstPointBox.x, secondPointBox.x), 0,
           Mathf.Min(firstPointBox.z, secondPointBox.z)
           );
        Vector3 upperRight = new Vector3(
            Mathf.Max(firstPointBox.x, secondPointBox.x), 0,
            Mathf.Max(firstPointBox.z, secondPointBox.z)
            );
        selectionAreaTransform.position = lowerLeft + ((upperRight - lowerLeft) / 2);
        Vector3 localS = upperRight - lowerLeft;
        selectionAreaTransform.localScale = new Vector3(localS.x, localS.z, 1);
    }
}
