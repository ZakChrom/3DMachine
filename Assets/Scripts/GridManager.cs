using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

	public GameObject player;
	
    public static int currentLevel;
    public static int enemyCount;
    public static int initialEnemyCount;

    public static float MSPT;

    public GameObject[] cellPrefabs;

    public static float animationLength = .2f;
    float timeSinceLastUpdate;

    public static bool playSimulation = false;
    public static bool stepSimulation = false;
    public static bool subTick        = false;
    public static bool clean          = true;

    //Used for sub ticking
    int cellUpdateIndex = 0;
    int rotationUpdateIndex = 0;

	int stepCount = 0;

    private void setStepCount(int value)
    {
        stepCount = value;
    }

	public void ClearAllTiles() {
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("cell")) {
			Destroy(g);
		}
	}

    private void InitBackgroundTiles() {
        ClearAllTiles();
    }

    private void Awake()
    {
        playSimulation = false;
        stepSimulation = false;
        subTick = false;
        clean = true;
        enemyCount = 0;

        instance = this;
		
        CellFunctions.cellList = new LinkedList<Cell>();
        CellFunctions.generatedCellList = new LinkedList<Cell>();

        int trackedId = 0;
        int tickedId = 0;

        foreach (CellType_e cellType in CellFunctions.cellUpdateOrder) {
            if (CellFunctions.cellUpdateTypeDictionary[cellType].Equals(CellUpdateType_e.TRACKED))
            {
                CellFunctions.steppedCellIdDictionary[cellType] = trackedId;
                trackedId++;
            }else {
                CellFunctions.steppedCellIdDictionary[cellType] = tickedId;
                tickedId++;
            }
        }

        //GameObject.Find("TutorialText").GetComponent<TextMeshProUGUI>().text = "";

    }

    private void InitSteppedCellQueues()
    { 
        int trackedCellClassCount = 0;
        int totalSteppedCellClassCount = 0;

        foreach (CellType_e cellType in CellFunctions.cellUpdateOrder)
        {
            if (CellFunctions.cellUpdateTypeDictionary[cellType].Equals(CellUpdateType_e.TRACKED))
            {
                trackedCellClassCount++;
            }
            totalSteppedCellClassCount++;
        }

        CellFunctions.tickedCellList = new LinkedList<Cell>[totalSteppedCellClassCount - trackedCellClassCount];
        for (int i = 0; i < CellFunctions.tickedCellList.Length; i++)
        {
            CellFunctions.tickedCellList[i] = new LinkedList<Cell>();
        }

        CellFunctions.trackedCellRotationUpdateQueue = new LinkedList<Cell>[trackedCellClassCount];
        CellFunctions.trackedCellPositionUpdateQueue = new LinkedList<Cell>[trackedCellClassCount, 6];
        for (int steppedCellId = 0; steppedCellId < trackedCellClassCount; steppedCellId++) {
            CellFunctions.trackedCellRotationUpdateQueue[steppedCellId] = new LinkedList<Cell>();
            for (int dir = 0; dir < 6; dir++) {
                CellFunctions.trackedCellPositionUpdateQueue[steppedCellId, dir] = new LinkedList<Cell>();
            }
        }

        CellFunctions.trackedCells = new LinkedList<Cell>[totalSteppedCellClassCount,6][];
        for (int steppedCellId = 0; steppedCellId < trackedCellClassCount; steppedCellId++)
        {
            for (int dir = 0; dir < 6; dir++)
            {
				int maxDistince = dir % 2 == 0 ? CellFunctions.gridWidth : CellFunctions.gridHeight;
				if (dir % 4 == 0) {
					int maxDistance = CellFunctions.gridLength;
				}
                CellFunctions.trackedCells[steppedCellId, dir] = new LinkedList<Cell>[maxDistince];
                for (int distance = 0; distance < maxDistince; distance++)
                {
                    CellFunctions.trackedCells[steppedCellId, dir][distance] = new LinkedList<Cell>();  
                }
            }
        }
    }

    private void Start()
    {
        initialEnemyCount = enemyCount;
    }

    public Cell SpawnCell(CellType_e cellType, Vector3 position, Vector3 rotation, Direction_e rotation2, bool generated) {
        Cell cell = Instantiate(player.GetComponent<Placing>().inventory[(int)cellType]).GetComponent<Cell>();
        cell.transform.position = new Vector3(position.x, position.y, position.z);
        cell.Setup(position, rotation2, generated);
        cell.oldPosition = position;
        cell.oldRotation = (int)rotation2;
        cell.transform.eulerAngles = rotation;
        cell.name = CellFunctions.cellList.Count + "";

        return cell;
    }
	public Cell SpawnCell(GameObject c, Vector3 position, Vector3 rotation, Direction_e rotation2, bool generated) {
        Cell cell = Instantiate(c).GetComponent<Cell>();
        cell.transform.position = new Vector3(position.x, position.y, position.z);
        cell.Setup(position, rotation2, generated);
        cell.oldPosition = position;
        cell.oldRotation = (int)rotation2;
        cell.transform.eulerAngles = rotation;
        cell.name = CellFunctions.cellList.Count + "";

        return cell;
    }

    public void InitGridSize()
    {
        //does everything that is dependent of the size of the grid
        CellFunctions.cellGrid = new Cell[CellFunctions.gridWidth, CellFunctions.gridHeight, CellFunctions.gridLength];
        InitSteppedCellQueues();
        InitBackgroundTiles();
    }

    private void UpdateTicked(CellType_e cellType) {
        foreach (TickedCell cell in CellFunctions.tickedCellList[CellFunctions.GetSteppedCellId(cellType)]) {
            if (cell.suppresed)
            {
                cell.suppresed = false;
                continue;
            }
            cell.Step();
        }
    }

    private void UpdateTracked(CellType_e cellType, Direction_e dir) {
		//Debug.Log("UpdateTracked");
        int maxDistance;
		if ((int)dir % 4 == 0) {
			maxDistance = CellFunctions.gridLength;
		}
        else if ((int)dir % 2 == 0)
        {
            maxDistance = CellFunctions.gridWidth;
        }
        else {
            maxDistance = CellFunctions.gridHeight;
        }
        for (int distance = 0; distance < maxDistance; distance++) {

            LinkedListNode<Cell> selectedNode = CellFunctions.trackedCells[CellFunctions.GetSteppedCellId(cellType), (int)dir][distance].First;
            TrackedCell cell;
			//Debug.Log("selectedNode null check");
            while (selectedNode != null)
            {
                cell = (TrackedCell)selectedNode.Value;
                selectedNode = selectedNode.Next;

                if (cell.suppresed)
                {
                    cell.suppresed = false;
                }
				else
                {
					//Debug.Log("Step should be called (GridManager.cs)");
                    cell.Step();
                }
            }
        }
    }

    private void MetaSortTrackedCells(CellType_e cellType)
    {
        int steppedCellId = CellFunctions.GetSteppedCellId(cellType);

        LinkedListNode<Cell> selectedNode = CellFunctions.trackedCellRotationUpdateQueue[steppedCellId].First;
        TrackedCell cell;
        while (selectedNode != null)
        {
            cell = (TrackedCell)selectedNode.Value;
            selectedNode = selectedNode.Next;

            cell.queuedForRotationSorting = false;
            cell.trackedCellNode.List.Remove(cell.trackedCellNode);
            CellFunctions.trackedCellPositionUpdateQueue[steppedCellId, (int)cell.getDirection()].AddLast(cell.trackedCellNode);
        }
    }

    private void SortTrackedCells(CellType_e cellType, Direction_e dir) {
        int steppedCellId = CellFunctions.GetSteppedCellId(cellType);
        LinkedListNode<Cell> selectedNode = CellFunctions.trackedCellPositionUpdateQueue[steppedCellId, (int)dir].First;
        TrackedCell cell;
        while (selectedNode != null)
        {
            cell = (TrackedCell)selectedNode.Value;
            selectedNode = selectedNode.Next;

            cell.queuedForPositionSorting = false;

            cell.trackedCellNode.List.Remove(cell.trackedCellNode);
            CellFunctions.trackedCells[steppedCellId, (int)dir][cell.GetDistanceFromFacingEdge()].AddLast(cell.trackedCellNode);
        }
    }

    public void Reset()
    {
        playSimulation = false;
        stepSimulation = false;
        clean = true;
        cellUpdateIndex = 0;
        rotationUpdateIndex = 0;

        setStepCount(0);
    }

    private void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;

        //Animate every cells rotation and transformation from last rotation and last position.

        foreach (Cell cell in CellFunctions.cellList) {
            if (cell.animate)
            {
                cell.transform.position = Vector3.Lerp(
					cell.oldPosition,
					cell.position,
					timeSinceLastUpdate / animationLength
                );
                cell.transform.rotation = Quaternion.Lerp(
                    Quaternion.Euler(Placing.rotations[cell.oldRotation]),
                    Quaternion.Euler(Placing.rotations[cell.rotation]),
                    timeSinceLastUpdate / animationLength
                );
            }
        }
        if (timeSinceLastUpdate > animationLength && (playSimulation || stepSimulation)) {

            setStepCount(stepCount + 1);

            timeSinceLastUpdate = 0;
            stepSimulation = false;
            clean = false;
            MSPT = System.DateTime.Now.Ticks;
            foreach (Cell cell in CellFunctions.cellList)
            {
                cell.oldPosition = cell.position;
                cell.oldRotation = cell.rotation;
            }
            if (subTick)
            {
                if (CellFunctions.GetUpdateType((CellType_e)cellUpdateIndex).Equals(CellUpdateType_e.TRACKED))
                {
                    if (rotationUpdateIndex == 0)
                    {
                        MetaSortTrackedCells((CellType_e)cellUpdateIndex);
                    }
                    SortTrackedCells((CellType_e)cellUpdateIndex, CellFunctions.directionUpdateOrder[rotationUpdateIndex]);
                    UpdateTracked((CellType_e)cellUpdateIndex, CellFunctions.directionUpdateOrder[rotationUpdateIndex]);
                    rotationUpdateIndex++;
                    if (rotationUpdateIndex > 3) {
                        cellUpdateIndex++;
                        rotationUpdateIndex = 0;
                    }
                }
                else {
                    UpdateTicked((CellType_e)cellUpdateIndex);
                    cellUpdateIndex++;
                }
                if (cellUpdateIndex >= CellFunctions.cellUpdateOrder.Length) {
                    cellUpdateIndex = 0;
                }
            }
            else {
                foreach (CellType_e cellType in CellFunctions.cellUpdateOrder)
                {
                    if (CellFunctions.GetUpdateType(cellType).Equals(CellUpdateType_e.TRACKED))
                    {
                        MetaSortTrackedCells((CellType_e)cellType);
                        foreach (Direction_e dir in CellFunctions.directionUpdateOrder)
                        {
                            SortTrackedCells(cellType, dir);
                            UpdateTracked(cellType, dir);
                        }
                    }
                    else
                    {
                        UpdateTicked(cellType);
                    }
                }
            }
            if (enemyCount == 0)
            {
                PlayerPrefs.SetInt("Level" + currentLevel, 1);
            }
            MSPT = System.DateTime.Now.Ticks - MSPT;
        }
    }
}
