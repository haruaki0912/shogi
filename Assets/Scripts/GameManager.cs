using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Analytics;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }



    public GameObject whiteCubePrefab;
    public GameObject blackCubePrefab;
    public GameObject highlightPrefab;
    public List<GameObject> highlights;
    public List<GameObject> PiecePrefabs;
    private float PieceZ;
    private List<GameObject> activePieces = new List<GameObject>();
    Vector3 prefabScale;

    private bool[,] canMoves { set; get; }
    public Piece[,] Pieces;
    private Piece selectedPiece;
    private int boardSize = 9;
    private float cameraHeight = 10f;

    private int selectionX = -1;
    private int selectionY = -1;

    private bool isBlackTurn;


    private void Start()
    {
        Instance = this;
        isBlackTurn = true;
        InitializePrefabs();
        CreateBoard();
        MoveCameraAboveBoard();
        
        highlights = new List<GameObject>();
        

        Debug.Log("Start Game");
        SpawnAllPieces(isBlackTurn);
    }

 
    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            UpdateSelect();

            if (selectionX >= 0 && selectionX < 9 && selectionY >= 0 && selectionY < 9)
            {
                if (selectedPiece == null)
                {
                    SelectPiece(selectionX, selectionY);
                }
                else
                {
                    Destination(selectionX, selectionY);
                }
            }
        }
    }

    private void UpdateSelect()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 25.0f))
        {
            selectionX = Mathf.RoundToInt(hit.point.x);
            selectionY = Mathf.RoundToInt(hit.point.y);
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void SelectPiece(int x, int y)
    {
        if (Pieces[x, y] == null)
        {
            return;
        }

        if ((isBlackTurn && Pieces[x, y].isBlack) || (!isBlackTurn && !Pieces[x, y].isBlack))
        {
            bool hasMove = false;
            canMoves = Pieces[x, y].PossibleMove();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (canMoves[i, j])
                    {
                        hasMove = true;

                    }
                }
            }

            if (!hasMove)
            {
                return;
            }

            HighlightAllowedMoves(canMoves);
            ResetPieceSizes();

            ChangePieceScale(x, y);

            selectedPiece = Pieces[x, y];


        }
    }

    private void Destination(int x, int y)
    {
        if (Pieces[x, y] != null && ((isBlackTurn && Pieces[x, y].isBlack) || (!isBlackTurn && !Pieces[x, y].isBlack)))
        {
            SelectPiece(x, y);
        }
        else if (canMoves[x, y])
        {
            MovePiece(x, y);
        }
        else
        {
            return;
        }
    }

    private void MovePiece(int x, int y)
    {

        if ((Pieces[x, y] != null) && (isBlackTurn != Pieces[x, y].isBlack)) // 移動先に敵の駒がいる場合
        {
            // 条件を満たすとゲーム終了
            if (Pieces[x, y].GetType() == typeof(Ousho))
            {
                EndGame();
                return;
            }

            activePieces.Remove(Pieces[x, y].gameObject);
            Destroy(Pieces[x, y].gameObject);
        }

        int Index = GetPieceIndex(selectedPiece.CurrentX, selectedPiece.CurrentY);
        bool isPromote = Pieces[selectedPiece.CurrentX, selectedPiece.CurrentY].isPromote;


        Destroy(Pieces[selectedPiece.CurrentX, selectedPiece.CurrentY].gameObject);
        Pieces[selectedPiece.CurrentX, selectedPiece.CurrentY] = null; // 移動元のマスを空にする

        if ((Index <= 5) && (!isPromote) && ((isBlackTurn && y >= 6) || (!isBlackTurn && y <= 2)))
        {
            isPromote = true;
        }

        SpawnPiece(Index, x, y, isBlackTurn, isPromote);
        ResetPieceSizes();
        ClearHighlights();
 

        selectedPiece = null; // 選択中の駒をリセット
        isBlackTurn = !isBlackTurn;

    }

 

    private void HighlightAllowedMoves(bool[,] canMoves)
    {
        ClearHighlights();

        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (canMoves[x, y])
                {
                    GameObject highlight = GetHighlightObject();
                    highlight.SetActive(true);
                    highlight.transform.position = new Vector3(x, y, PieceZ);
                }
            }
        }
    }

    private void ClearHighlights()
    {
        foreach (GameObject highlight in highlights)
        {
            highlight.SetActive(false);
        }
    }

    private GameObject GetHighlightObject()
    {
        GameObject go = highlights.Find(g => !g.activeSelf);

        if (go == null)
        {
            go = Instantiate(highlightPrefab);
            highlights.Add(go);
        }

        return go;
    }


    private int GetPieceIndex(int x, int y)
    {
        if (Pieces[x, y] != null)
        {
            return Pieces[x, y].pieceIndex;
        }
        return -1; // インデックスが存在しない場合
    }

    private void ChangePieceScale(int x, int y)
    {
        Vector3 newScale = Pieces[x, y].transform.localScale * 1.5f;
        Pieces[x, y].transform.localScale = newScale;
    }

    void ResetPieceSizes()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                if (Pieces[i, j] != null)
                {
                    Pieces[i, j].transform.localScale = prefabScale;
                }
            }
        }
    }

    private void InitializePrefabs()
    {

        whiteCubePrefab = Resources.Load<GameObject>("whiteCube");
        blackCubePrefab = Resources.Load<GameObject>("blackCube");
        highlightPrefab = Resources.Load<GameObject>("highlights");

        PiecePrefabs = new List<GameObject>
        {
            Resources.Load<GameObject>("Fu"),
            Resources.Load<GameObject>("Hisha"),
            Resources.Load<GameObject>("Kakugyo"),
            Resources.Load<GameObject>("Kyosha"),
            Resources.Load<GameObject>("Keima"),
            Resources.Load<GameObject>("Ginsho"),
            Resources.Load<GameObject>("Kinsho"),
            Resources.Load<GameObject>("Ousho"),
            Resources.Load<GameObject>("Gyokusho")
        };

        if (prefabScale == Vector3.zero)
        {
            // プレハブからスケールを取得
            prefabScale = PiecePrefabs[0].transform.localScale;
        }

        Bounds bounds = PiecePrefabs[0].GetComponent<Renderer>().bounds;
        PieceZ = bounds.size.z / 2 + 0.5f;
    }

    //cube盤面→オブジェクト盤面変更時にbox colliderに変更
    private void CreateBoard()
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                GameObject cubePrefab = (x + y) % 2 == 0 ? whiteCubePrefab : blackCubePrefab;
                GameObject cube = Instantiate(cubePrefab, new Vector3(x, y, 0), Quaternion.identity);
                cube.transform.parent = transform;
            }
        }
    }

    private void SpawnPiece(int index, int x, int y, bool isBlackTurn, bool isPromote)
    {
        x = (int)Mathf.Round(x);
        y = (int)Mathf.Round(y);

        Quaternion rotation = Quaternion.Euler(0, isPromote ? 180 : 0, isBlackTurn ? 0 : 180);


        GameObject newPiece = Instantiate(PiecePrefabs[index], new Vector3(x, y, PieceZ), rotation) as GameObject;
        newPiece.transform.SetParent(transform);

        Piece pieceComponent = newPiece.GetComponent<Piece>();
        pieceComponent.SetPosition(x, y);
        pieceComponent.isBlack = isBlackTurn ? true : false;
        pieceComponent.isPromote = isPromote ? true : false;
        pieceComponent.SetPieceIndex(index);

        activePieces.Add(newPiece);
        Pieces[x, y] = pieceComponent;
    }


    private void SpawnAllPieces(bool isBlackTurn)
    {
        activePieces = new List<GameObject>();
        Pieces = new Piece[9, 9];


        //Fu
        for (int i = 0; i < 9; i++)
            SpawnPiece(0, i, 2, isBlackTurn, false);
        //Hisha
        SpawnPiece(1, 1, 1, isBlackTurn, false);
        //Kaku
        SpawnPiece(2, 7, 1, isBlackTurn, false);
        //Kyo
        SpawnPiece(3, 0, 0, isBlackTurn, false);
        SpawnPiece(3, 8, 0, isBlackTurn, false);
        //Keima
        SpawnPiece(4, 1, 0, isBlackTurn, false);
        SpawnPiece(4, 7, 0, isBlackTurn, false);
        //Gin
        SpawnPiece(5, 2, 0, isBlackTurn, false);
        SpawnPiece(5, 6, 0, isBlackTurn, false);
        //Kin
        SpawnPiece(6, 3, 0, isBlackTurn, false);
        SpawnPiece(6, 5, 0, isBlackTurn, false);
        //Ou
        SpawnPiece(7, 4, 0, isBlackTurn, false);

        isBlackTurn = !isBlackTurn;

        for (int i = 0; i < 9; i++)
            SpawnPiece(0, i, 6, isBlackTurn, false);
        SpawnPiece(1, 7, 7, isBlackTurn, false);
        SpawnPiece(2, 1, 7, isBlackTurn, false);
        SpawnPiece(3, 0, 8, isBlackTurn, false);
        SpawnPiece(3, 8, 8, isBlackTurn, false);
        SpawnPiece(4, 1, 8, isBlackTurn, false);
        SpawnPiece(4, 7, 8, isBlackTurn, false);
        SpawnPiece(5, 2, 8, isBlackTurn, false);
        SpawnPiece(5, 6, 8, isBlackTurn, false);
        SpawnPiece(6, 3, 8, isBlackTurn, false);
        SpawnPiece(6, 5, 8, isBlackTurn, false);
        SpawnPiece(8, 4, 8, isBlackTurn, false);

    }

    private void MoveCameraAboveBoard()
    {

        Vector3 chessboardCenter = new Vector3((boardSize - 1) / 2f, (boardSize - 1) / 2f, 0);

        float cameraX = chessboardCenter.x;
        float cameraY = chessboardCenter.y;
        float cameraZ = cameraHeight;

        Camera.main.transform.position = new Vector3(cameraX, cameraY, cameraZ);
        Camera.main.transform.rotation = Quaternion.Euler(180f, 0f, 180f);
    }

    private void EndGame()
    {
        if (isBlackTurn)
            Debug.Log("Black Wins");
        else
            Debug.Log("White Wins");

        foreach (GameObject all in activePieces)
            Destroy(all);
        ClearHighlights();

        Start();
    }
}

