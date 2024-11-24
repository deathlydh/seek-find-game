using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Shader _shader;
    [SerializeField]
    Texture2D _texture;

    void Start()
    {
        Image image = GetComponent<Image>();
        image.material = new Material(_shader);
        image.material.SetTexture(Shader.PropertyToID("_Text"), image.sprite.texture);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
