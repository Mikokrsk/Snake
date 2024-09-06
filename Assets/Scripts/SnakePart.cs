using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class SnakePart : MonoBehaviour
{
    public SnakePartName snakePartName;

    [SerializeField] public Tile tile;
    [SerializeField] public Vector3Int curentPosition;
    //   [SerializeField] public Vector3Int nextPosition;

}

public enum SnakePartName
{
    Head,
    Body,
    Tale
}