using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CatalystSiteElement : MonoBehaviour {

    protected abstract IEnumerator InitializeCoroutine(SerializableCatalystSiteElement siteData);
    protected abstract IEnumerator ActivateCoroutine(SerializableCatalystSiteElement siteData);
    protected abstract IEnumerator DeactivateCoroutine();

    public Coroutine Activate(SerializableCatalystSiteElement siteData)
    {

        return StartCoroutine(ActivateCoroutine(siteData));

    }

    public Coroutine Deactivate()
    {

        return StartCoroutine(DeactivateCoroutine());

    }

    public Coroutine Initialize(SerializableCatalystSiteElement siteData)
    {

        return StartCoroutine(InitializeCoroutine(siteData));

    }

    protected void PrintIncorrectTypeError(string siteName, string dataType)
    {


        Debug.LogErrorFormat("Could not load site element {0} at site {1}: Incorrect data passed to Activate method", dataType, siteName);

    }
}

[System.Serializable]
public abstract class SerializableCatalystSiteElement
{

    public int id;
    public string name;
    public string description;


}
