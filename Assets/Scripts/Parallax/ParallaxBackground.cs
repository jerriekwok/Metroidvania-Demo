using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//视差背景
public class ParallaxBackground : MonoBehaviour
{

    private Camera mainCamera;
    private float lastCameraPositionX;//记录上一帧相机的X位置
    private float cameraHalfWidth;

    [SerializeField] private ParallaxLayer[] backgroundLayers;//用数组保存所有背景层

    private void Awake()
    {
        mainCamera = Camera.main;

        //得到正交相机的半宽度
        cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;//半宽度 = 半高度 * 宽高比
        InitializeLayers();
    }

    private void FixedUpdate()
    { 
        float currentCameraPositionX = mainCamera.transform.position.x;//获取当前相机位置
        float distanceToMove = currentCameraPositionX - lastCameraPositionX;//计算相机相比上一帧移动的距离
        lastCameraPositionX = currentCameraPositionX;//更新

        float cameraLeftEdge = currentCameraPositionX - cameraHalfWidth;//得到相机的左边缘
        float cameraRightEdge = currentCameraPositionX + cameraHalfWidth;//得到相机的右边缘

        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.Move(distanceToMove);
            layer.LoopBackground(cameraLeftEdge, cameraRightEdge);
        }
    }

    private void InitializeLayers()
    {
        foreach (ParallaxLayer layer in backgroundLayers)
        {
            layer.CalculateImageWidth();//计算图像的宽度
        }
    }
}
