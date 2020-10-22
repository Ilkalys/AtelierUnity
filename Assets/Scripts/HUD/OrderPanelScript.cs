using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RTS;

public class OrderPanelScript : MonoBehaviour
{
    public Image DisplaySprite;
    public GameObject[] button;
    public GameObject[] queueButton;
    public Slider pourcentSlider;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
    }


    public void ActualiseDisplaySprite(Sprite sp)
    {
        DisplaySprite.gameObject.SetActive(true);
        DisplaySprite.sprite = sp;
    }
    public void ActualiseBuildDisplay(string[] canCreate, string[] queue, float pourcent, Player controller)
    {
        if (player.username == controller.username)
        {
            for (int i = 0; i < canCreate.Length; i++)
            {
                if (canCreate[i] != "")
                {
                    button[i].SetActive(true);
                    button[i].GetComponent<Image>().sprite = ResourceManager.GetBuildImage(canCreate[i]);
                }
            }
            for(int i = 0; i< Mathf.Min(queueButton.Length, queue.Length); i++)
            {
                if(queue[i] != "")
                {
                    queueButton[i].SetActive(true);
                    queueButton[i].GetComponent<Image>().sprite = ResourceManager.GetBuildImage(queue[i]);
                }
            }
            pourcentSlider.gameObject.SetActive(true);
            pourcentSlider.value = pourcent;
        }
    }

    public void ActualiseUnitDisplay(string[] canCreate, Player controller)
    {
        if (player.username == controller.username)
        {
            for (int i = 0; i < canCreate.Length; i++)
            {
                if (canCreate[i] != "")
                {
                    button[i].SetActive(true);
                    button[i].GetComponent<Image>().sprite = ResourceManager.GetBuildImage(canCreate[i]);
                }
            }
        }
    }

    public void ReinitDisplay()
    {
        for (int i = 0; i < button.Length; i++)
        {
            button[i].SetActive(false);
        }
        for (int i = 0; i < queueButton.Length; i++)
        {
            queueButton[i].SetActive(false);
        }
        DisplaySprite.gameObject.SetActive(false);
        pourcentSlider.gameObject.SetActive(false);
    }
}
