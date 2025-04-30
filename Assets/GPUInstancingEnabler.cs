using System.Collections;
using UnityEngine;

public class GPUInstancingEnabler : MonoBehaviour
{
    Renderer meshRenderer;
    MaterialPropertyBlock materialPropertyBlock;
    private void Awake()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        meshRenderer = GetComponent<Renderer>();
        meshRenderer.SetPropertyBlock(materialPropertyBlock);




    }

    private void OnEnable()
    {
        StartCoroutine(changeEmisiion());
    }

    IEnumerator changeEmisiion()
    {
        while (true)
        {
            //meshRenderer.material.SetColor("_EmissionValue", new Color(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10), 0));

            materialPropertyBlock.SetColor("_EmissionValue", new Color(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10), 0));
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
