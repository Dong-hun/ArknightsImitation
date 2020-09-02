using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : NodeManager
{
    int m_TileX;
    int m_TileY;

    public int TileX
    {
        set
        {
            m_TileX = value;
        }
        get
        {
            return m_TileX;
        }
    }
    public int TileY
    {
        set
        {
            m_TileY = value;
        }
        get
        {
            return m_TileY;
        }
    }
}
