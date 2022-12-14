using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VariableButton :MonoBehaviour, IPressable
{
    private GameObject closedBox;
    private GameObject openBox;
    private GameObject landingArea;

    private void Start()
    {
        GetReferences();
    }

    protected void GetReferences()
    {
        openBox = transform.parent.Find("BoxOpen").gameObject;
        landingArea = transform.parent.Find("BoxOpen/LandingArea").gameObject;
        closedBox = transform.parent.Find("BoxClosed").gameObject;

    }

    public void OnPressed()
    {
        // check if the box is open
        Transform boxOpenTransform = transform.parent.Find("BoxOpen");


        // when a box is open - close it!
        if (boxOpenTransform != null && boxOpenTransform.gameObject.active == true)// if a box is already opened when the button is clicked
        {
            closedBox.SetActive(true);
            openBox.SetActive(false);
            landingArea.SetActive(false);

            transform.parent.GetComponent<IRepresentable>().setRepresentation("");
        }
        else// when the box is closed - open it!
        {
            
            closedBox.SetActive(false);
            openBox.SetActive(true);
            landingArea.SetActive(true);

            // get var name
            string varName = transform.Find("Text").GetComponent<TMP_Text>().text;

            // set representation in the container variable
            transform.parent.GetComponent<IRepresentable>().setRepresentation(varName);

            // check if level is complete
            GameObject gameManagerGameObject = GameObject.Find("GameManager");
            GameManager gameManager = gameManagerGameObject.GetComponent<GameManager>();
            gameManager.CheckLevelComplete();
        }
    }
}
