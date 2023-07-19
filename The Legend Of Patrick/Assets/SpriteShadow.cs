using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpriteShadow : MonoBehaviour
{

    public Vector2 offset = new Vector2(-3,-3);

    SpriteRenderer sprRndCaster;
    SpriteRenderer sprRndShadow;

    Transform transCaster;
    Transform transShadow;

    public Color shadowColor;


    void Start()
    {
        transCaster = transform;
        transShadow = new GameObject().transform;
        transShadow.localScale = new Vector3(transCaster.localScale.x, transCaster.localScale.y, 1f);
        transShadow.parent = transCaster;
  
        transShadow.gameObject.name = "Shadow";
        transShadow.localRotation = Quaternion.identity;
        
        sprRndCaster = GetComponent<SpriteRenderer>();
        sprRndShadow = transShadow.gameObject.AddComponent<SpriteRenderer>();

        sprRndShadow.sortingLayerName = sprRndCaster.sortingLayerName;
        sprRndShadow.sortingOrder = sprRndCaster.sortingOrder - 1;

        sprRndShadow.color = shadowColor;

    }

    void LateUpdate()
    {
        
        transShadow.position = new Vector2(transCaster.position.x + offset.x, transCaster.position.y + offset.y);

        sprRndShadow.sprite = sprRndCaster.sprite;

        

    }
}
