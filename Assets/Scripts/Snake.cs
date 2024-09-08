using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Snake : MonoBehaviour
{
    public float gameSpeed;
    public bool isAlive = true;
    public List<GameObject> snakePartsList = new List<GameObject>();
    public GameObject snakeHeadPref;
    public GameObject snakeBodyPref;
    [SerializeField] protected Tilemap tilemap;
    [SerializeField] protected Vector3Int nextDirection;
    [SerializeField] protected Vector3Int currentDirection;
    [SerializeField] private Animator animator;
    private readonly string snakeGameOverName = "GameOver";
    private void Start()
    {
        nextDirection = Vector3Int.left;
        currentDirection = Vector3Int.left;

        StartCoroutine(Game());
    }

    private void Update()
    {
        RotateHead(snakePartsList.First(), GetDirection());
    }

    private Vector3Int GetDirection()
    {
        var joysticDir = GameManager.instance.joystick.Direction;
        var dirX = joysticDir.x;
        var dirY = joysticDir.y;
        var direction = currentDirection;

        if (dirX != 0 && dirY != 0)
        {
            if (Mathf.Abs(dirX) >= Mathf.Abs(dirY))
            {
                direction = dirX > 0 ? Vector3Int.right : Vector3Int.left;
            }
            else
            {
                direction = dirY > 0 ? Vector3Int.up : Vector3Int.down;
            }
        }

        return direction;
    }

    private void RotateHead(GameObject snakePart, Vector3Int direction)
    {
        if (direction != -currentDirection)
        {
            nextDirection = direction;
        }
        else
        {
            return;
        }

        if (direction == Vector3Int.left)
        {
            snakePart.transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (direction == Vector3Int.right)
        {
            snakePart.transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else if (direction == Vector3Int.up)
        {
            snakePart.transform.eulerAngles = new Vector3(0, 0, -90);
        }
        else if (direction == Vector3Int.down)
        {
            snakePart.transform.eulerAngles = new Vector3(0, 0, 90);
        }
    }

    public IEnumerator Game()
    {
        while (isAlive)
        {
            yield return new WaitForSeconds(gameSpeed);

            var nextPosition = Vector3Int.zero;

            for (int i = 0; i < snakePartsList.Count && isAlive; i++)
            {
                SnakePart snakePart;
                if (snakePartsList[i].TryGetComponent<SnakePart>(out snakePart))
                {
                    if (snakePart.snakePartName == SnakePartName.Head)
                    {
                        nextPosition = snakePart.curentPosition;
                        MovingHead(snakePart);
                    }
                    else
                    {
                        var newPos = nextPosition;
                        nextPosition = snakePart.curentPosition;
                        MovingBody(snakePart, newPos);
                    }
                }
            }
            currentDirection = nextDirection;
        }
    }

    public void MovingHead(SnakePart snakePart)
    {
        var newSnakePosition = snakePart.curentPosition + nextDirection;

        TileBase tileAtNextPosition = tilemap.GetTile(newSnakePosition);

        if (tileAtNextPosition != null && (tileAtNextPosition.name == "Wall" || tileAtNextPosition.name == "SnakeBody"))
        {
            isAlive = false;
            GameManager.instance.GameOver();
            animator.SetTrigger(snakeGameOverName);
            return;
        }

        if (tileAtNextPosition != null && tileAtNextPosition.name == "Food")
        {
            PointsController.instance.AddPoints();
            AddNewBody(snakePart.curentPosition);
        }

        tilemap.SetTile(snakePart.curentPosition, null);

        snakePart.curentPosition = newSnakePosition;
        snakePart.transform.position = new Vector3(tilemap.GetCellCenterWorld(newSnakePosition).x, tilemap.GetCellCenterWorld(newSnakePosition).y, snakePart.transform.position.z);
        tilemap.SetTile(newSnakePosition, snakePart.tile);
    }

    public void MovingBody(SnakePart snakePart, Vector3Int newSnakePosition)
    {
        tilemap.SetTile(snakePart.curentPosition, null);

        snakePart.curentPosition = newSnakePosition;
        snakePart.transform.position = new Vector3(tilemap.GetCellCenterWorld(newSnakePosition).x, tilemap.GetCellCenterWorld(newSnakePosition).y, snakePart.transform.position.z);
        tilemap.SetTile(newSnakePosition, snakePart.tile);
    }

    public void AddNewBody(Vector3Int position)
    {
        var newBody = Instantiate(snakeBodyPref, transform);
        var snakePart = newBody.GetComponent<SnakePart>();
        snakePart.curentPosition = position;
        snakePart.transform.position = tilemap.GetCellCenterWorld(snakePart.curentPosition);
        tilemap.SetTile(snakePart.curentPosition, snakePart.tile);
        snakePartsList.Add(newBody);
        FoodSpawner.instance.SpawnFood();
    }
}