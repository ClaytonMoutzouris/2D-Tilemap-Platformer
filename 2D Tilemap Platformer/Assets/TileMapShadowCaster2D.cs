using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generates ShadowCaster2Ds for every continuous block of a tilemap on Awake, applying some settings.
/// </summary>
public class TileMapShadowCaster2D : MonoBehaviour
{
    [SerializeField]
    protected CompositeCollider2D m_TilemapCollider;

    [SerializeField]
    protected bool m_SelfShadows = true;

    protected virtual void Reset()
    {
        m_TilemapCollider = GetComponent<CompositeCollider2D>();
    }

    protected virtual void Awake()
    {
        ShadowCaster2DGenerator.GenerateTilemapShadowCasters(m_TilemapCollider, m_SelfShadows);
    }
}
