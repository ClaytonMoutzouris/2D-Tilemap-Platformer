﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MultiCamera : MonoBehaviour
{
    /// <summary>
    /// A reference to the the player.
    /// </summary>

    /// <summary>
    /// The position.
    /// </summary>
    public Camera mCamera;
    /// <summary>
    /// The map. Assigned from editor.
    /// </summary>
    public GameGrid mMap;



    public float mZoomSpeed = 10f;
    public float mMinOrthographicSize = 3;
    public float mBoundingBoxPadding = 1;

    private List<PlayerController> players = new List<PlayerController>();
    private Vector3 velocity = Vector3.zero;
    public float cameraSpeed = 10F;

    void Awake()
    {
        mCamera = GetComponent<Camera>();

        //mCamera.orthographicSize = mMinOrthographicSize;
        //mPosition = transform.position;
    }

    public void LateUpdate()
    {

        Rect boundingBox = CalculateTargetsBoundingBox();

        transform.position = Vector3.Lerp(transform.position, CalculateCameraPosition(boundingBox), cameraSpeed * Time.deltaTime);
        mCamera.orthographicSize = CalculateOrthographicSize(boundingBox);
        StayWithinBounds();
    }

    void StayWithinBounds()
    {
        var cameraPos = transform.position;
        var halfHeight = mCamera.orthographicSize;
        var halfWidth = mCamera.aspect * halfHeight;
        //Keep the camera within the bounds of the maps width
        if (cameraPos.x - halfWidth < 0)
        {
            cameraPos.x = halfWidth;
        }
        else if (cameraPos.x + halfWidth > mMap.mapSizeX)
        {
            cameraPos.x = mMap.mapSizeX - halfWidth;
        }

        //Keep the camera within the bounds of the maps height
        if (cameraPos.y - halfHeight < 0)
        {
            cameraPos.y = halfHeight;
        }
        else if (cameraPos.y + halfHeight > mMap.mapSizeY)
        {
            cameraPos.y = mMap.mapSizeY - halfHeight;
        }

        transform.position = new Vector3(cameraPos.x, cameraPos.y, transform.position.z);
        //transform.position = Vector3.SmoothDamp(transform.position, new Vector3(cameraPos.x, cameraPos.y, transform.position.z), ref velocity, smoothTime);
    }

    Rect CalculateTargetsBoundingBox()
    {
        float minX = mMap.mapSizeX;
        float maxX = 0;
        float minY = mMap.mapSizeY;
        float maxY = 0;

        foreach (PlayerController player in players)
        {
            Vector3 position = player.transform.position;

            minX = Mathf.Min(minX, position.x);
            minY = Mathf.Min(minY, position.y);
            maxX = Mathf.Max(maxX, position.x);
            maxY = Mathf.Max(maxY, position.y);
        }

        if (players.Count == 0)
        {
            minX = Mathf.Min(minX, mMap.mapSizeX / 2);
            minY = Mathf.Min(minY, mMap.mapSizeY / 2);
            maxX = Mathf.Max(maxX, mMap.mapSizeX / 2);
            maxY = Mathf.Max(maxY, mMap.mapSizeY / 2);
        }


        return Rect.MinMaxRect(minX - mBoundingBoxPadding, maxY + mBoundingBoxPadding, maxX + mBoundingBoxPadding, minY - mBoundingBoxPadding);
    }

    float CalculateOrthographicSize(Rect boundingBox)
    {
        float orthographicSize = mCamera.orthographicSize;
        Vector3 topRight = new Vector3(boundingBox.x + boundingBox.width, boundingBox.y, 0f);
        Vector3 topRightAsViewport = mCamera.WorldToViewportPoint(topRight);

        if (topRightAsViewport.x >= topRightAsViewport.y)
            orthographicSize = Mathf.Abs(boundingBox.width) / mCamera.aspect / 2f;
        else
            orthographicSize = Mathf.Abs(boundingBox.height) / 2f;

        return Mathf.Clamp(Mathf.Lerp(mCamera.orthographicSize, orthographicSize, Time.deltaTime * mZoomSpeed), mMinOrthographicSize, Mathf.Infinity);
    }

    Vector3 CalculateCameraPosition(Rect boundingBox)
    {
        Vector2 boundingBoxCenter = boundingBox.center;

        return new Vector3(boundingBoxCenter.x, boundingBoxCenter.y, mCamera.transform.position.z);
    }

    public void ZoomIn()
    {
        mMinOrthographicSize -= mZoomSpeed;
        mMinOrthographicSize = Mathf.Clamp(mMinOrthographicSize, 3.0f, 10.0f);
    }

    public void ZoomOut()
    {
        mMinOrthographicSize += mZoomSpeed;
        mMinOrthographicSize = Mathf.Clamp(mMinOrthographicSize, 3.0f, 10.0f);

    }

    public void AddPlayer(PlayerController player)
    {
        players.Add(player);
    }

    public void RemovePlayer(PlayerController player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }

    }

}
