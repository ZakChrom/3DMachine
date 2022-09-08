using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CellFunctions : MonoBehaviour
{	
	public GameObject gridManager;
	public GameObject player;
	private static GameObject[] inventory;
	
	void Start() {
		inventory = player.GetComponent<Placing>().inventory;
		gridHeight = 100;
		gridWidth = 100;
		gridLength = 100;
		gridManager.GetComponent<GridManager>().InitGridSize();
	}
	void Update() {
		if (Input.GetKeyDown("r")) {
			GridManager.playSimulation = !GridManager.playSimulation;
		}
	}
	public void Reset() {
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("cell")) {
			g.GetComponent<Cell>().transform.position = g.GetComponent<Cell>().spawnPosition;
		}
		gridManager.GetComponent<GridManager>().Reset();
	}
	public void Save() {
		GUIUtility.systemCopyBuffer = Worlds.Save();
	}
	public void Load() {
		Worlds.Load();
	}
	
	public static Direction_e[] directionUpdateOrder = { Direction_e.RIGHT, Direction_e.LEFT, Direction_e.UP, Direction_e.DOWN , Direction_e.FRONT , Direction_e.BACK };
    public static CellType_e[] cellUpdateOrder = {CellType_e.MOVER, CellType_e.FIXEDROTATOR};
    public static Dictionary<CellType_e, CellUpdateType_e> cellUpdateTypeDictionary = new Dictionary<CellType_e, CellUpdateType_e>
    {
        [CellType_e.MOVER] = CellUpdateType_e.TRACKED,
        [CellType_e.WALL] = CellUpdateType_e.BASE,
        [CellType_e.TRASH] = CellUpdateType_e.BASE,
		[CellType_e.FIXEDROTATOR] = CellUpdateType_e.TRACKED,
    };

    //An disctionary defining the specialized ID's used in sorting, for tracked and 
    public static Dictionary<CellType_e, int> steppedCellIdDictionary = new Dictionary<CellType_e, int>();

    public static int gridWidth = 1;
    public static int gridHeight = 1;
	public static int gridLength = 1;
    //Used to check which cell might be at location x, y, z.
    public static Cell[,,] cellGrid;
    //Used to check if x, y, z is considered a placeable tile.
    public static bool[,,] placeableTiles;

    public static LinkedList<Cell> cellList;
    //Cells made during the simulation
    public static LinkedList<Cell> generatedCellList;

    //Cells that need to be updated but not in a specific order.
    //tickedCellList[CellType];
    public static LinkedList<Cell>[] tickedCellList;

    //Cells that need to be updated but in a specific order (Depending on direction).
    //trackedCells[TrackedCell ID][Direction, Distince];
    //public static LinkedList<Cell>[][,] trackedCells;
    // changed to [,][]
    // the jagged array is 2 dimensional
    public static LinkedList<Cell>[,][] trackedCells;


    //trackedCellRotationUpdateQueue[CellType] Cell type must be sorted into a new direction queue if it has been rotated since it was last sorted
    public static LinkedList<Cell>[] trackedCellRotationUpdateQueue;
    //trackedCellPositionUpdateQueue[CellType, Cell Direction] Cell type "X" facing direction "Y" must be sorted before Cells of type X facing direction Y are updated
    public static LinkedList<Cell>[,] trackedCellPositionUpdateQueue;

    public static int GetSteppedCellId(CellType_e type) {
		return steppedCellIdDictionary[type];
    }

    public static CellUpdateType_e GetUpdateType(CellType_e type) {
        return cellUpdateTypeDictionary[type];
    }
	
	public static CellType_e PrefabToCellType_e(GameObject prefab) {
		CellType_e r = CellType_e.WALL;
		if (prefab == inventory[0]) {
			r = CellType_e.MOVER;
		} else if (prefab == inventory[1]) {
			
		} else {
			throw new NotImplementedException("Me when u forgor to update this if :skull:");
		}
		return r;
	}
}
