using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Tilemap groundMap;
    [SerializeField] private Tilemap objectMap;
    [SerializeField] private TileBase wallTile;
    [SerializeField] private TileBase boxTile;
    [SerializeField] private GameManager gameManager;
    private Animator anim;
    class MoveData
    {
        public Vector3 playerPos;

        public bool pushedBox;

        public Vector3Int oldBoxPos;
        public Vector3Int newBoxPos;
    }

    Stack<MoveData> moveHistory = new Stack<MoveData>();
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Move(Vector3Int.up);
            anim.Play("PlayerUp");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Move(Vector3Int.down);
            anim.Play("PlayerDown");
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector3Int.left);
            anim.Play("PlayerLeft");
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector3Int.right);
            anim.Play("PlayerRight");
        }
    }

    void Move(Vector3Int dir)
    {
        Vector3Int nextCell = groundMap.WorldToCell(transform.position) + dir;

        if (groundMap.GetTile(nextCell) == wallTile)
            return;

        MoveData data = new MoveData();

        data.playerPos = transform.position;
        data.pushedBox = false;

        if (objectMap.GetTile(nextCell) == boxTile)
        {
            Vector3Int boxNext = nextCell + dir;

            if (groundMap.GetTile(boxNext) == wallTile)
                return;

            if (objectMap.GetTile(boxNext) == boxTile)
                return;

            data.pushedBox = true;
            data.oldBoxPos = nextCell;
            data.newBoxPos = boxNext;

            objectMap.SetTile(nextCell, null);
            objectMap.SetTile(boxNext, boxTile);
        }

        moveHistory.Push(data);
        transform.position = groundMap.GetCellCenterWorld(nextCell);
        gameManager.CheckWin();
    }

    public void UndoMove()
    {
        if (moveHistory.Count == 0)
            return;
        MoveData data = moveHistory.Pop();
        transform.position = data.playerPos;
        if (data.pushedBox)
        {
            objectMap.SetTile(data.newBoxPos, null);
            objectMap.SetTile(data.oldBoxPos, boxTile);
        }
    }
}
