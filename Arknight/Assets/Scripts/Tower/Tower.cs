using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int m_TileX;
    public int m_TileY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTileNumber(int x, int y)
    {
        m_TileX = x;
        m_TileY = y;
    }
}
