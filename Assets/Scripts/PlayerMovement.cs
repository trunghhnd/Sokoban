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
    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    [SerializeField] private float swipeThreshold = 50f;
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
        HandleSwipe();
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
    private void HandleSwipe()
    {
        if (Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                touchStartPos = touch.position;
                break;

            case TouchPhase.Ended:
                touchEndPos = touch.position;

                Vector2 swipe = touchEndPos - touchStartPos;

                if (swipe.magnitude < swipeThreshold)
                    return;

                if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                {
                    if (swipe.x > 0)
                    {
                        Move(Vector3Int.right);
                        anim.Play("PlayerRight");
                    }
                    else
                    {
                        Move(Vector3Int.left);
                        anim.Play("PlayerLeft");
                    }
                }
                else
                {
                    if (swipe.y > 0)
                    {
                        Move(Vector3Int.up);
                        anim.Play("PlayerUp");
                    }
                    else
                    {
                        Move(Vector3Int.down);
                        anim.Play("PlayerDown");
                    }
                }
                break;
        }
    }
}
