using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLight : MonoBehaviour {

    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = FindMeshRenderer();
    }

    private void Start()
    {
        meshRenderer.enabled = false;
        //StartCoroutine(RunAfterCertainSeconds(0.0195f, 0.020f));
        StartCoroutine(RunAfterCertainSeconds(0.0001f, 0.020f));
    }

    IEnumerator RunAfterCertainSeconds(float first, float second)
    {
        yield return new WaitForSeconds(first);
        meshRenderer.enabled = true;
        yield return new WaitForSeconds(second);
        //meshRenderer.enabled = false;
        Destroy(this.gameObject, 0f);
    }


    private MeshRenderer FindMeshRenderer()
    {
        MeshRenderer meshRenderer = new MeshRenderer();

        // If parent object does not have component "MeshRenderer" find it from children.
        if (this.GetComponent<MeshRenderer>() == null)
        {
            Transform[] t = this.GetComponentsInChildren<Transform>();
            for (int i = 0; i < t.Length; i++)
            {
                if (t[i].GetComponent<MeshRenderer>())
                {
                    meshRenderer = t[i].GetComponent<MeshRenderer>();
                    break;
                }
            }
        }
        else
        {
            // If parent object has "MeshRenderer" component
            meshRenderer = this.GetComponent<MeshRenderer>();
        }

        return meshRenderer;
    }
}
