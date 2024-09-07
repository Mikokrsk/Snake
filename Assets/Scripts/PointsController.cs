using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointsController : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public int points = 0;

    public static PointsController instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        textMeshPro.text = "Points: 0";
    }

    public void AddPoints()
    {
        points += 1;
        textMeshPro.text = $"Points: {points}";
    }
}