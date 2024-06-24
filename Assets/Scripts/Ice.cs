
using System;
using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Ice: MonoBehaviour, iBurnable
{
    public UniversalRendererData universalRendererData;
    public ParticleSystem steam;
    private Material _IceShader;
    private MeshCollider _meshCollider;
    private static readonly int Melt1 = Shader.PropertyToID("_Melt");
    private BoxCollider _boxCollider;
    public Animator Chest;
    public bool OnFire { get; set; }

    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        //_meshCollider = gameObject.GetComponentInParent<MeshCollider>();
        OnFire = false;
        _IceShader = gameObject.GetComponentInParent<Renderer>().material;
    }

    public iBurnable.Effect Burning()
    {
        print("Melting");
        OnFire = true;
        StartCoroutine(Melt());
        return null;
    }

    private IEnumerator Melt()
    {
        steam.Play();
        yield return new WaitForSeconds(2f);
        float elapsedTime = 0;
        const int time = 4;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            var value = Mathf.Lerp(0f, 11f, elapsedTime / time);
            print(value + "   " + elapsedTime / time);
            _IceShader.SetFloat(Melt1, value);
            yield return null;
        }
        //Chest.Play("Chest");
        Chest.SetBool("Open", true);
        //Chest.Play("Open");
        _boxCollider.enabled = false;
        steam.Stop();
        //universalRendererData.rendererFeatures.SetActive(true);
        //universalRendererData.rendererFeatures.Find("FullScreenPassRendererFeature").SetActive(true);
        //_meshCollider.enabled = false;

    }
}