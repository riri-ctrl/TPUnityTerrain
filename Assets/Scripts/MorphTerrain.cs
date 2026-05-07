using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphTerrain : MonoBehaviour
{
    public KeyCode startMorphKey = KeyCode.Space;
    public float animTime = 2f;
    public float minScale = 0.01f;

    private GenerateMeshFromHeightMap meshGenerator;

    private void Start()
    {
        meshGenerator = GetComponent<GenerateMeshFromHeightMap>();
    }

    void Update()
    {
        if (Input.GetKeyDown(startMorphKey))
        {
            StopAllCoroutines();
            StartCoroutine(Morph());
        }
    }

    IEnumerator Morph()
    {
        float t            = 0f;
        float startScaleY  = transform.localScale.y;
        float targetScaleY = Random.Range(0.5f, 2f);

        
        while (t < 1f)
        {
            t += Time.deltaTime / animTime * 2f;
            float currentY = Curves.QuadEaseInOut(startScaleY, minScale, Mathf.Clamp01(t));
            transform.localScale = new Vector3(
                transform.localScale.x,
                currentY,
                transform.localScale.z
            );
            yield return null;
        }

        
        meshGenerator.GetRandomMap();
        meshGenerator.Generate();

        t = 0f;

        
        while (t < 1f)
        {
            t += Time.deltaTime / animTime * 2f;
            float currentY = Curves.QuadEaseInOut(minScale, targetScaleY, Mathf.Clamp01(t));
            transform.localScale = new Vector3(
                transform.localScale.x,
                currentY,
                transform.localScale.z
            );
            yield return null;
        }
    }
}