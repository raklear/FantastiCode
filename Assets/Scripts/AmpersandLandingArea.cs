using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AmpersandLandingArea : MonoBehaviour, ILandingArea
{
    string varName;
    private void Start()
    {
        // get current variable name
        varName = transform.parent.Find("Button/Text").GetComponent<TMP_Text>().text;
    }

    // check if the object that is being released on the landing area is supposed to be released there
    // for ex. address can't be released on an int
    public bool OnDraggableReleased(GameObject go)
    {
        // how do i check if the game object is an int???
        AmpersandOperator op = go.GetComponent<AmpersandOperator>();
        if (op != null)
            return true;
        else
            return false;
    }

    //once the object has been releaased
    public void OnLandingArea(GameObject go)
    {
        go.transform.parent = transform;

        StartCoroutine( CaptureAddressInsertToReturnValue(go));

        // update code logger


        // log in code
        CodeLogger codeLogger = GetCodeLogger();// 
        codeLogger.SetExtra("&" + varName);
    }

    protected virtual CodeLogger GetCodeLogger()
    {
        return GameObject.Find("CodeLogger").GetComponent<CodeLogger>();
    }

    protected IEnumerator CaptureAddressInsertToReturnValue(GameObject go)
    {
        Laso laso = go.transform.Find("Laso").GetComponent<Laso>();

        // fetch the address by streching and shortening 
        yield return laso.Fetch();

        // destroy amperdand on landing area
        Destroy(transform.Find("Ampersand").gameObject);

        // delete all children of return value
        Transform returnValueLandingAreaTransform = GetReturnValueLandingAreaTransform();
        foreach (Transform t in returnValueLandingAreaTransform)
            Destroy(t.gameObject);

        //change ticket to be the child of return value
        Transform ticketTransform = transform.Find("Ticket");
        ticketTransform.parent = returnValueLandingAreaTransform;

        // send the address to the return value
        yield return SendToReturnValue(ticketTransform);

        // update the representation in the return value
        returnValueLandingAreaTransform.GetComponent<ReturnValueLandingArea>().representation = "&" + varName;

        // check if the level is complete
        GameObject gameManagerGameObject = GameObject.Find("GameManager");
        GameManager gameManager = gameManagerGameObject.GetComponent<GameManager>();
        gameManager.CheckLevelComplete();

    }

    private IEnumerator SendToReturnValue(Transform ticket)
    {
        float timePassed = 0;

        Vector3 startLocalPos = ticket.localPosition;
        Vector3 endLocalPos = new Vector3(0f, 0, 0);

        float fraction = 0;
        float animTime = 1f;

        while (fraction < 1)
        {
            timePassed += Time.deltaTime;
            fraction = timePassed / animTime;

            ticket.localPosition = Vector3.Lerp(startLocalPos, endLocalPos, fraction);
            yield return null;
        }

        // destroy all children on return value
    }
    protected virtual Transform GetReturnValueLandingAreaTransform()
    {
        Transform returnValueLandingAreaTransform = GameObject.Find("ReturnValue/LandingArea").transform;
        return returnValueLandingAreaTransform;
    }
}
