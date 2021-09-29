// Fall 2021 - CS294-137 HW2
// Siyu Zhang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using cs294_137.hw2;

public class GameController : MonoBehaviour
{
    public bool AtttEnabled;  // 0: original tic-tac-toe; 1: augmented tic-tac-toe
    public int xoTurnInfo;   // whose turn it is 0 = x and 1 = o
    public int turnCounter;  // counts the number of turns
    public GameObject[] turnBarIcons;  // the bar indicating whose turn it is
    public GameObject[] gamePlayerUI;  // x on the left and o on the right
    public Material[] xoIcon;  // 0 = x and 1 = o 2 = None
    public CellButton[] boardCells;  // 9 cells on the board
    public bool gameStarted; // indicating a started game
    public Text appMessageText; 
    public Text gameMssageText;
    public GameObject[] winningLines;  // display the line of the winner
    public GameObject gameBoard;
    // game algorithm
    public int[] markedCells;  // indicates which cell is marked by which player

    // keep the old position info
    public Vector3 oldXPosition;
    public Vector3 oldOPosition;

    // augmented mode additional UI components
    public Sprite[] uiSprites;  // 0: small; 1: medium: 2: large
    public GameObject[] uiBoxes;  // at the bottom of the UI
    public Text[] markerCountUI;  
    public int selectedUIbox;  // 0: small; 1: medium: 2: large
    // TODO: change to private
    public int[] augmentedBoardCellRecord = new int[9];  // keep track of the icon size on the board
    public int[] numRemainingMarkers;  // recording the number of remianing markers of each player; 3 3 3 3 3 3 at start

    private PlaceGameBoard placeGameBoard;
    private Animator xAnimator;
    private Animator oAnimator;
    private int[] smallestRemainIndex;  // should be updated for default selection

    private string[] markerName = {"Small", "Medium", "Large"};

    // Start is called before the first frame update
    void Start()
    {
        AtttEnabled = false;
        //GameSetup();
        placeGameBoard = GetComponent<PlaceGameBoard>();
        xAnimator = gamePlayerUI[0].GetComponent<Animator>();
        oAnimator = gamePlayerUI[1].GetComponent<Animator>();
        // xAnimator.enabled
        xAnimator.enabled = false;
        oAnimator.enabled = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        // if(!gameBoard.activeSelf)
        //     DisableCells();
        // else
    }

    // initialization
    public void GameSetup()
    {
        xoTurnInfo = 0;
        turnCounter = 0;
        gameStarted = false;
        xAnimator.enabled = false;
        oAnimator.enabled = false;
        gamePlayerUI[0].transform.eulerAngles = new Vector3(0, 0, 0);
        gamePlayerUI[1].transform.eulerAngles = new Vector3(0, 0, 0);
        gamePlayerUI[0].transform.localPosition =oldXPosition;
        gamePlayerUI[1].transform.localPosition = oldOPosition;
        gamePlayerUI[0].SetActive(false);
        gamePlayerUI[1].SetActive(false);
        turnBarIcons[0].SetActive(false); 
        turnBarIcons[1].SetActive(false);

        smallestRemainIndex = new int[]{0, 0};  // default selecting the small marker

        // initialize button to non-material
        for(int i = 0; i < boardCells.Length; ++i)
        {
            // boardCell reset
            boardCells[i].isInteractable = true;
            boardCells[i].cellMarkCube.GetComponent<MeshRenderer>().material = xoIcon[2];
            // augmented game mode reset
            augmentedBoardCellRecord[i] = -1;  // for augmented mode; -1 means no object on current cell
            if(i < 3)
            {
                uiBoxes[i].GetComponent<Button>().interactable = true;
                markerCountUI[i].text = markerName[i] + " x 3";
            }
            if(i < 6)  // 3 for x player; 3 for y player
                numRemainingMarkers[i] = 3;
            // marked cell reset
            markedCells[i] = -100;
            // winning line reset
            if(i != 8)
                winningLines[i].SetActive(false);
        }
    }


    public void showStartMessage()
    {
        GameSetup();
        if(placeGameBoard.Placed())
        {
            gamePlayerUI[0].SetActive (true);
            gamePlayerUI[1].SetActive(true);
            turnBarIcons[0].SetActive(true); 
            turnBarIcons[1].SetActive(false);
            gameStarted = true;
            appMessageText.gameObject.SetActive(true);
            appMessageText.text = AtttEnabled ? "AUGMENTED Tic-Tac-Toe Game Start!" : "ORIGINAL Tic-Tac-Toe Game Start!";        

            ShowAtttUIs(AtttEnabled);
        }
    }

    public void TicTacToeButton(int cellID)
    {
        if(AtttEnabled)  // augmented game mode 
        {
            int offset = xoTurnInfo == 0 ? 0 : 3;
            if(numRemainingMarkers[selectedUIbox + offset] <= 0)
                selectedUIbox = smallestRemainIndex[xoTurnInfo];
            if(selectedUIbox == -1)  // current player used all grids
            {
                winnerDisplay(-2);
                return;
            }
            ShowSelectingBox(selectedUIbox);
            if(selectedUIbox > augmentedBoardCellRecord[cellID]) // is able to overwrite
            {
                if(selectedUIbox == 2)  // largest marker
                {   
                    boardCells[cellID].isInteractable = false;
                }
                // set board cell material
                int matIndex, remainIndex;
                string remainInfo;
                if(xoTurnInfo == 0)
                {
                    matIndex = 3 + selectedUIbox;
                    remainIndex = offset + selectedUIbox;
                    remainInfo = "Player X's " +  markerName[selectedUIbox];
                }
                else
                {
                    matIndex = 6 + selectedUIbox;
                    remainIndex = offset + selectedUIbox;
                    remainInfo = "Player O's " +  markerName[selectedUIbox];
                }
                string indexlog = "material index: " + matIndex;
                Debug.Log(indexlog);
                boardCells[cellID].cellMarkCube.GetComponent<MeshRenderer>().material = xoIcon[matIndex];
                // update records for the game
                markedCells[cellID] = xoTurnInfo + 1;  // xoTurnInfo = {0, 1}  -- markedCells = {1, 2}
                augmentedBoardCellRecord[cellID] = selectedUIbox;
                // prompting the remaining info
                if(--numRemainingMarkers[remainIndex] == 0)
                    updateDefaultMarker(remainIndex);
                gameMssageText.gameObject.SetActive(true);
                gameMssageText.text = remainInfo + " -1 ";
                markerCountUI[selectedUIbox].text = markerName[selectedUIbox] + " x " + numRemainingMarkers[remainIndex];;
                // current player used all grids
                if(smallestRemainIndex[xoTurnInfo] == -1)  
                {
                    winnerDisplay(-2);
                    return;
                }
                // both players used large markers
                if(numRemainingMarkers[2] == 0 && numRemainingMarkers[5] == 0)
                {
                    if(winnerDisplay(-3))
                        return;
                }
                turnCounter++;
            }
            else   // is not able to overwrite
            {
                appMessageText.gameObject.SetActive(true);
                appMessageText.text = "CANNOT overwrite! Try greater markers?";
                return;
            }

            // check result
            if(turnCounter > 4)
            {
                if(winnerCheck()) 
                    return;
            }
        }
        else // original game mode
        {
            boardCells[cellID].cellMarkCube.GetComponent<MeshRenderer>().material = xoIcon[xoTurnInfo];
            boardCells[cellID].isInteractable = false;

            markedCells[cellID] = xoTurnInfo + 1;  // xoTurnInfo = {0, 1}  -- markedCells = {1, 2}
            turnCounter++;

            // check result
            if(turnCounter > 4)
            {
                if(winnerCheck()) 
                    return;
            }

            if(turnCounter == 9)
            {
                winnerDisplay(-1);
                return;
            }
        }

        // Turn Update
        if(xoTurnInfo == 0)
        {
            xoTurnInfo = 1;
            turnBarIcons[0].SetActive(false); 
            turnBarIcons[1].SetActive(true);
        }
        else
        {
            xoTurnInfo = 0;
            turnBarIcons[0].SetActive(true); 
            turnBarIcons[1].SetActive(false);
        }
        if(AtttEnabled)
            UpdateAtttUIs();
    }

    public void DisableCells()
    {
        for(int i = 0; i < boardCells.Length; ++i)
        {
            boardCells[i].isInteractable = false;
        }
    }

    void DisableTurnBars()
    {
        turnBarIcons[0].SetActive(false); 
        turnBarIcons[1].SetActive(false);
    }

    bool winnerDisplay(int indexLine)
    {
        gameMssageText.gameObject.SetActive(true);
        // both run out of large marker
        // check whether we have marker left greater than the markers on  the board
        if(indexLine == -3)  
        {
            bool getATie = true;
            for(int i = 0; i < augmentedBoardCellRecord.Length; ++i)
            {
                bool xMediumCheck = (numRemainingMarkers[1] > 0 && 1 > augmentedBoardCellRecord[i]);
                bool oMediumCheck = (numRemainingMarkers[4] > 0 && 1 > augmentedBoardCellRecord[i]);
                if(xMediumCheck || oMediumCheck)
                    return false;  // no tie; can still place markers
                bool xSmallCheck = (numRemainingMarkers[0] > 0 && 0 > augmentedBoardCellRecord[i]);
                bool oSmallCheck = (numRemainingMarkers[3] > 0 && 0 > augmentedBoardCellRecord[i]);
                if(xSmallCheck || oSmallCheck)
                    return false;  // no tie; can still place markers
            }
            if(getATie)
            {    
                gameMssageText.gameObject.SetActive(true);
                gameMssageText.text = "----Tied----";
            }
        }
        else if(indexLine == -2)  // one player run out of markers, check cell ownership
        {
            int xCell = 0, oCell = 0;
            for(int i = 0; i < markedCells.Length; i++)
            {
                if(markedCells[i] == 1) xCell++;
                else if(markedCells[i] == 2) oCell++;
            }
            if(xCell == oCell)
            {                
                gameMssageText.gameObject.SetActive(true);
                gameMssageText.text = "----Tied----";
            }
            string winnerInfo = xCell > oCell ? "Player X Wins (Due to More Cells)!" : "Player O Wins (Due to More Cells)!";
            gameMssageText.text = winnerInfo; 
            if(xoTurnInfo == 0)
                xAnimator.enabled = true;
            else
                oAnimator.enabled = true;
            
        }
        else if(indexLine != -1)  // we have a winner!
        {
            string winnerInfo = xoTurnInfo == 0 ? "Player X Wins!" : "Player O Wins";
            gameMssageText.text = winnerInfo; 
            winningLines[indexLine].SetActive(true); // activate the line
            if(xoTurnInfo == 0)
                xAnimator.enabled = true;
            else
                oAnimator.enabled = true;

        }
        else  // we have a tie
        {
            gameMssageText.gameObject.SetActive(true);
            gameMssageText.text = "----Tied----";
        }
        DisableCells();
        DisableTurnBars();
        gameStarted = false;
        return true;
    }

    bool winnerCheck()
    {
        int sol1 = markedCells[0] + markedCells[1] + markedCells[2];
        int sol2 = markedCells[3] + markedCells[4] + markedCells[5];
        int sol3 = markedCells[6] + markedCells[7] + markedCells[8];
        int sol4 = markedCells[0] + markedCells[3] + markedCells[6];
        int sol5 = markedCells[1] + markedCells[4] + markedCells[7];
        int sol6 = markedCells[2] + markedCells[5] + markedCells[8];
        int sol7 = markedCells[0] + markedCells[4] + markedCells[8];
        int sol8 = markedCells[2] + markedCells[4] + markedCells[6];

        var solutions = new int[] {sol1, sol2, sol3, sol4, sol5, sol6, sol7, sol8};
        for(int i = 0; i < solutions.Length; ++i)
        {
            // x: 3 * (0 + 1) = 3 --- x win
            // o: 3 * (1 + 1) = 3 --- o  win
            if(solutions[i] == 3 * (xoTurnInfo + 1))  
            {
                winnerDisplay(i);
                return true;
            }
        }
        return false;
    }

    public void ShowAtttUIs(bool AtttEnabled)
    {
        if(AtttEnabled)
        {
            int offset = xoTurnInfo == 0 ? 0 : 3;

            for(int i = 0; i < 3; ++i)
            {
                uiBoxes[i].SetActive(true);
                uiBoxes[i].GetComponent<Image>().sprite = uiSprites[i + offset];
            }
            if(smallestRemainIndex[xoTurnInfo] != -1)
                ShowSelectingBox(smallestRemainIndex[xoTurnInfo]); // default selecting smallest? TODO: change to the existing smallest value
        }
        else
        {
            uiBoxes[0].SetActive(false);
            uiBoxes[1].SetActive(false);
            uiBoxes[2].SetActive(false);
        }
    }

    public void UpdateAtttUIs()
    {
        int offset = xoTurnInfo == 0 ? 0 : 3;

        for(int i = 0; i < 3; ++i)
        {
            uiBoxes[i].SetActive(true);
            uiBoxes[i].GetComponent<Image>().sprite = uiSprites[i + offset];

            int markerNum = numRemainingMarkers[offset + i];
            if(markerNum <= 0)
            {   
                uiBoxes[i].GetComponent<Button>().interactable = false;
                uiBoxes[i].GetComponent<Image>().color = Color.gray;
            }
            else
                uiBoxes[i].GetComponent<Button>().interactable = true;
            markerCountUI[i].text = markerName[i] + " x " + markerNum;
        }
        if(smallestRemainIndex[xoTurnInfo] != -1)
            ShowSelectingBox(smallestRemainIndex[xoTurnInfo]);
    }

    // high light the button of current selecting box
    public void ShowSelectingBox(int boxId)
    {
        if(AtttEnabled)
        {
            if(boxId != -1)
            {
                for(int i = 0; i < 3; ++i)
                {
                    if(i == boxId)
                        uiBoxes[i].GetComponent<Image>().color = Color.green;
                    else
                        uiBoxes[i].GetComponent<Image>().color = Color.white;
                }
                selectedUIbox = boxId;
            }
            
        }
    }

    private void updateDefaultMarker(int emptyIndex)
    {   
        for(int i = 0; i < 3; ++i)
        {
            if(i != emptyIndex && numRemainingMarkers[i] > 0)
            {
                smallestRemainIndex[xoTurnInfo] = i;
                return; // find the smallest index of positive num markers
            }
        }
        smallestRemainIndex[xoTurnInfo] = -1;
    }
}
