using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ParallaxLayer 
{
    [SerializeField] private Transform background;
    [SerializeField] private float parallaxMultiplier;//ÒÆ¶¯±ÈÀý  
    [SerializeField] private float imageWidthOffset = 10;//±³¾°Æ«ÒÆÁ¿

    private float imageFullWidth;//±³¾°¿í¶È
    private float imageHalfWidth;

    public void CalculateImageWidth()
    {
        imageFullWidth = background.GetComponent<SpriteRenderer>().bounds.size.x;
        imageHalfWidth = imageFullWidth / 2;
    }

    public void Move(float distanceToMove)
    {
        background.position += Vector3.right * (distanceToMove * parallaxMultiplier);
    }

    public void LoopBackground(float cameraLeftEdge,float cameraRightEdge)
    {
        float imageRightEdge = ( background.position.x + imageHalfWidth ) - imageWidthOffset;//±³¾°ÓÒ±ßÔµ
        float imageLeftEdge = ( background.position.x - imageHalfWidth ) + imageWidthOffset;//±³¾°×ó±ßÔµ

        if (imageRightEdge < cameraLeftEdge)
        {
            background.position += Vector3.right * imageFullWidth;
        }
        else if (imageLeftEdge > cameraRightEdge)
        {
            background.position += Vector3.right * -imageFullWidth;
        }
    }
}
