using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraPostProcessing : MonoBehaviour
{
    public Sprite[] bg_textures;
    public Camera camera_bg;
    //private int bg_counter = -1;
    private int bg_counter = 0;
    
    public Material material;

    private RenderTexture bg_view;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        Graphics.Blit(source, destination, material);
    }

    void Awake()
    {
        //bg_view = new RenderTexture(624, 350, 16, RenderTextureFormat.ARGB32);
        bg_view = new RenderTexture(512, 290, 16, RenderTextureFormat.ARGB32);
        bg_view.filterMode = FilterMode.Point;
        bg_view.Create();
        camera_bg.targetTexture = bg_view;

        material = new Material(Shader.Find("Custom/SpaceInvadersBG"));
        material.SetTexture("_Mask", bg_view);
        set_mask();

        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            set_mask();
        }
    }

    void set_mask()
    {
        bg_counter = (bg_counter + 1) % bg_textures.Length;
        ((SpriteRenderer)GameObject.Find("bg mask").GetComponent(typeof(SpriteRenderer))).sprite = bg_textures[bg_counter];

    }
}
