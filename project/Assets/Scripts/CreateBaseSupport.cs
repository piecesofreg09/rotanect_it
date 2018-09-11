using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreateBaseSupport : MonoBehaviour
{

    // public parameters
    // odd number rows, even number columns
    public int numOfRows = 12;
    public int numOfCols = 12;
    public float sizeCubes = 0.39f;
    public float intCubes = 0.01f;
    //public Material materialsRefInactive;
    //public Material materialsRefActive;
    public Text scoreText;
    public Text scoreText1;
    // UI Text time
    public Text timeText;
    public Text timeText1;
    // UI Text average points
    public Text aveText;
    public Text aveText1;
    string aveTextStr;

    // The broadness of top cubes is brn
    private float brn = 0.05f;

    // Both top and base cubes are created
    private GameObject[,] cubeCarved;
    private GameObject[] cubeCarvedParent;
    // Base cubes Y positions and top cubes Y positions are global variables
    private float baseYHeight = 0.05f;
    private float topYHeight = 0.25f;
    /*
    private Color planeColor = new Color32(189, 211, 251, 1);
    private Color addPlaneColor;
    // base cube color
    private Color baseColor = new Color32(255, 232, 133, 1);
    // The top cube ori color
    private Color topOriColor = new Color32(255, 116, 14, 1);
    // the top cube color when rotation
    private Color topRotColor = new Color32(255, 116, 14, 1);
    // the top cube color when destroyed
    private Color topAftColor = new Color32(164, 142, 245, 1);
    */
    private Color planeColor = new Color32(255, 255, 255, 1);
    private Color addPlaneColor;
    // base cube color
    private Color baseColor = new Color32(125, 210, 234, 1);
    // The top cube ori color
    private Color topOriColor = new Color32(7, 161, 205, 1);
    // the top cube color when rotation
    private Color topRotColor = new Color32(7, 161, 205, 1);
    // the top cube color when destroyed
    private Color topAftColor = new Color32(246, 231, 0, 1);

    // rend is the Renderer reference for every cube
    private Renderer rend;

    // Label the type of the cubes
    private int[] cubeType;
    // Label the state of the cubes
    private int[] cubeStat;
    // Label the connenction of the cubes, is a binary number
    private byte[,] conStat;

    // rotateTag labels that some cube is rotating
    // cubeRotate is the cube that is rotating
    // rotCount is the number of frames to finish the cube rotation
    static int rotCount = 10;
    private int[] countInUse;
    private int[] rotateTag;
    // check the summation of rotations and keep the value of last frame
    private int rotTagSumTemp = 0;
    private int rotTagSum = 0;
    private GameObject[] cubeRotate;
    // Rotation mode
    private int rotMode = 0;

    // Neighbour list storage
    private int[,] neiList;
    private int[,] conNeiList;
    // Cube list store
    ArrayList[] cbList;
    private int listNum;
    private int[] listClosed;
    private int[] cubeCalYet;
    private int overFlowLength = 200;

    // parameters to check destroy list 
    private int[] desListId;
    private int desListNum = 0;
    // record the points
    private int pointsEarned = 0;
    // record the average points
    private float pointsEarnedAve = 0.0f;
    private float pointsEarnedAveShow = 0.0f;

    // Mouse input forbidden par, true means forbidden, false means input allowed
    private bool mouIpForbid = true;
    private int mouIpForPulse;
    // this length should be a little longer than the animation time
    private int mouIFPLength = 2;
    // Label cal
    private bool calTing = false;
    private bool calDone = false;
    // Label destroy
    private bool desYing = false;
    private bool desProDone = false;
    private bool desDone = false;
    private bool creDone = false;
    private ArrayList desCubeId;
    private int desCubes = 0;
    private int desTime = 0;
    private int desDeltaTime = 10;
    private int tDesI = 0;
    // Label used for create (before create)
    private int sedBfCreate = 50;
    private int sedBfCreateIt = 0;

    // Time
    private float startTime;
    string textTime;
    private float pastTime = 0.0f;
    private float guiTime = 0.0f;
    // add the button listener
    Button myButton;
    // add the listerner to the save done process
    private UnityEvent saveDone;
    // the scene this scene is changing to when save done
    public string sceneChangeTo;

    // Use this for initialization
    void Start()
    {
        // load saved scores
        SaveLoadRecords.Load();
        Score.savedScores = SaveLoadRecords.savedScore;

        // get plane and set color
        GameObject x = GameObject.Find("Plane");
        rend = x.GetComponent<Renderer>();
        rend.material.color = planeColor;
        GameObject y = GameObject.Find("AddLayer");
        rend = y.GetComponent<Renderer>();
        addPlaneColor = planeColor;
        addPlaneColor.a = 0.9f;
        rend.material.color = addPlaneColor;

        // initiate rotation cubes
        countInUse = new int[numOfCols * numOfRows];
        rotateTag = new int[numOfCols * numOfRows];
        cubeRotate = new GameObject[numOfCols * numOfRows];


        // load saved loadornot flag
        LoadOrNotSave.Load();
        LoadOrNot.loadOr = LoadOrNotSave.loadOri;
        Debug.Log("This is loaded from saved data: " + LoadOrNot.loadOr);

        if (LoadOrNot.loadOr == true)
        {
            SaveLoad.Load();
            Game.current = SaveLoad.savedGame;
            // initiate label of connenctions
            cubeType = new int[numOfCols * numOfRows];
            cubeStat = new int[numOfCols * numOfRows];
            conStat = new byte[numOfCols * numOfRows, 4];
            cubeType = Game.current.cubeType;
            cubeStat = Game.current.cubeStat;
            pointsEarned = Game.current.score;
            pastTime = Game.current.pastTime;
            pointsEarnedAveShow = Game.current.aveScore;
            createBaseCubes(numOfCols, numOfRows, sizeCubes, intCubes, baseYHeight);
            createTopCubesFromGD(numOfCols, numOfRows, sizeCubes, intCubes, baseYHeight, topYHeight);
        }
        else if (LoadOrNot.loadOr == false)
        {
            // initiate label of connenctions
            cubeType = new int[numOfCols * numOfRows];
            cubeStat = new int[numOfCols * numOfRows];
            conStat = new byte[numOfCols * numOfRows, 4];
            // create base cubes
            createBaseCubes(numOfCols, numOfRows, sizeCubes, intCubes, baseYHeight);
            // randomly generating small cubes
            createTopCubes(numOfCols, numOfRows, sizeCubes, intCubes, baseYHeight, topYHeight);
        }


        // Iniation of the neighbour list
        neiList = new int[numOfCols * numOfRows, 4];
        conNeiList = new int[numOfCols * numOfRows, 3];

        // Initiation of cube list
        cbList = new ArrayList[numOfCols * numOfRows];
        cubeCalYet = new int[numOfCols * numOfRows];

        listNum = 0;
        listClosed = new int[numOfCols * numOfRows];
        for (var i = 0; i < numOfCols * numOfRows; i++)
        {
            conNeiList[i, 0] = 0;
            conNeiList[i, 1] = -1;
            conNeiList[i, 2] = -1;
        }
        desCubeId = new ArrayList();
        getIniNeighbor();
        getConnectNeigbor();
        createGlobalConList();
        checkClose();
        //changeColorClosedLists();
        desClosedLists();
        calDone = false;
        desDone = false;
        // enable mouse
        mouIpForbid = false;
        mouIpForPulse = mouIFPLength;
        // set create time
        sedBfCreateIt = sedBfCreate;
        // Set score
        scoreText.text = "Points:   " + pointsEarned;
        scoreText1.text = "Points:   " + pointsEarned;
        // set time
        startTime = Time.time;
        

        // add the button listener
        myButton = GameObject.Find("Close").GetComponent<Button>();
        myButton.onClick.AddListener(saveGameData);

        saveDone = new UnityEvent();
        saveDone.AddListener(() => { SceneManager.LoadScene(sceneChangeTo); Debug.Log("change scene to:" + sceneChangeTo); });

        // lift camera height
        Camera.main.enabled = true;
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 5.5f;
    }
    // Update is called once per frame
    void Update()
    {
        showingTimeAScore();
        if (mouIpForbid == false)
        {
            // Change the color of totated cubes
            checkRightMouseClicked(sizeCubes, intCubes);

            // Make the rotate process continue
            rotationContinue();

            // Check the cubes that are still rotating
            rotTagSumTemp = rotTagSum;
            int tt = 0;
            for (var i = 0; i < numOfCols * numOfRows; i++)
            {
                tt += rotateTag[i];
            }
            rotTagSum = tt;
            // Debug.Log(" Former Rot: " + rotTagSumTemp + " Current Rot: " + rotTagSum);

            // only when the rotation stops, or the whole world change from rotation to still, that the program check the state : when the updating happens, forbid mouse input
            if ((rotTagSum == 0) && (rotTagSumTemp > 0))
            {
                mouIpForbid = true;
                // the update use the same function and calculate the list again
                // either start coroutine or direct start it.
                StartCoroutine("updateGlobalConList");
                //updateGlobalConList();
            }

        }
        else if (mouIpForbid == true)
        {
            // Count down for mouse input
            mouIpForPulse -= 1;
            if (mouIpForPulse < 0)
            {
                mouIpForPulse = 0;
            }
            if ((mouIpForPulse <= 0) && (creDone == true))
            {
                mouIpForbid = false;
                mouIpForPulse = mouIFPLength;
                creDone = false;
            }
            // Debug information
            //debugOutputScripts();
            // Check whether the recalculation is done, if the calculation is done, begin destroy process
            if (calDone == true && desYing == false && desDone == false)
            {
                checkClose();
                //changeColorClosedLists();
                desClosedListsDetermination();
                if (desCubes > 0)
                {
                    desYing = true;
                }
                else
                {
                    mouIpForbid = false;
                }

            }
            else if (calDone == true && desYing == false && desDone == true)
            {

                createClosedListNew();
                calDone = false;
                desDone = true;
                creDone = true;
                sedBfCreateIt = 50;

            }
            // Check when destroying
            if (desYing == true)
            {
                desContinue();
                if (desProDone == true)
                {
                    desProDone = false;
                    desDone = true;
                    desYing = false;
                }
            }

        }
    }
    void saveGameData()
    {
        Game.current.cubeType = cubeType;
        Game.current.cubeStat = cubeStat;
        Game.current.score = pointsEarned;
        Game.current.pastTime = guiTime;
        Game.current.aveScore = (int)pointsEarnedAveShow;
        SaveLoad.Save();
        LoadOrNot.loadOr = true;
        LoadOrNotSave.Save();
        saveDone.Invoke();
        
        Debug.Log("Game saved, load sign changed to:"+LoadOrNotSave.loadOri);
        LoadOrNotSave.loadOri = false;
        Debug.Log("Load flag cleared, load sign changed to:" + LoadOrNotSave.loadOri);
        LoadOrNotSave.Load();
        Debug.Log("Game saved, load sign changed to:" + LoadOrNotSave.loadOri);
        SaveLoad.savedGame = new Game();
        Debug.Log("Deleta saveload class, past time changed to: " + SaveLoad.savedGame.pastTime);
        SaveLoad.Load();
        Debug.Log("Load data, past time changed to: " + SaveLoad.savedGame.pastTime);
        Debug.Log("Load data, show type 10: "+SaveLoad.savedGame.cubeType[10]+" stat 10: "+ SaveLoad.savedGame.cubeType[10]);
        
    }
    void showingTimeAScore()
    {
        guiTime = Time.time - startTime + pastTime;

        int hours = (int)(guiTime / 3600);
        int minutes = (int)((guiTime % 3600) / 60);
        int seconds = (int)(guiTime % 60);
        //int fraction = (int)((guiTime * 10) % 10);

        textTime = string.Format(" {0:00} : {1:00} : {2:00}", hours, minutes, seconds);
        timeText.text = "Time:" + textTime;
        timeText1.text = "Time:" + textTime;
        if (guiTime < 1)
        {
            guiTime = 1.0f;
        }
        pointsEarnedAve = pointsEarned / guiTime * 60;
        if (pointsEarnedAve > pointsEarnedAveShow)
        {
            pointsEarnedAveShow = pointsEarnedAve;
            var tt = Score.savedScores[5];
            Score.savedScores[5] = (int)pointsEarnedAveShow;
            SaveLoadRecords.Save();
            SaveLoadRecords.Load();
            Debug.Log(" " + SaveLoadRecords.savedScore[0] + " " + SaveLoadRecords.savedScore[1] + " " + SaveLoadRecords.savedScore[2] + " " + SaveLoadRecords.savedScore[3] + " " + SaveLoadRecords.savedScore[4] + " " + SaveLoadRecords.savedScore[5]);
        }
        aveTextStr = string.Format(" {0} ", (int)pointsEarnedAveShow);
        aveText.text = "Ave Points:" + aveTextStr + "p/m";
        aveText1.text = "Ave Points" + aveTextStr + "p/m";
    }
    private void createBaseCubes(int numOfCols, int numOfRows, float sizeCubes, float intCubes, float baseYHeight)
    {
        int itX = 0;
        int itZ = 0;
        int nameCube = 0;
        GameObject[] cube = new GameObject[numOfCols * numOfRows];
        for (itX = 0; itX < numOfCols; itX++)
        {
            for (itZ = 0; itZ < numOfRows; itZ++)
            {
                nameCube = itX * numOfRows + itZ;
                cube[nameCube] = GameObject.CreatePrimitive(PrimitiveType.Cube);

                cube[nameCube].name = "Cube" + nameCube;
                cube[nameCube].transform.localScale += new Vector3((sizeCubes - 1), -(1 - baseYHeight), (sizeCubes - 1));
                cube[nameCube].transform.position = new Vector3(itX * (sizeCubes + intCubes) + sizeCubes / 2, baseYHeight / 2, itZ * (sizeCubes + intCubes) + sizeCubes / 2);
                rend = cube[nameCube].GetComponent<Renderer>();
                rend.material.color = baseColor;
            }
        }
    }
    private void createTopCubes(int numOfCols, int numOfRows, float sizeCubes, float intCubes, float baseYHeight, float topYHeight)
    {
        int nameCube = 0;
        cubeCarved = new GameObject[numOfCols * numOfRows, 2];
        cubeCarvedParent = new GameObject[numOfCols * numOfRows];

        for (var i = 0; i < numOfCols; i++)
        {
            for (var j = 0; j < numOfRows; j++)
            {
                nameCube = i * numOfRows + j;
                //generate random number 0 and 1 to represent different type of cubes, 0 is line, 1 is L
                int typeI = Random.Range(0, 4);
                if (typeI != 0)
                {
                    typeI = 1;
                }
                cubeType[nameCube] = typeI;
                Vector3 posVec = new Vector3(i * (sizeCubes + intCubes) + sizeCubes / 2, baseYHeight / 2, j * (sizeCubes + intCubes) + sizeCubes / 2);
                createParentCubes(ref cubeCarvedParent[nameCube], nameCube, posVec);
                createChildCubes(ref cubeCarved[nameCube, 0], ref cubeCarved[nameCube, 1], ref cubeCarvedParent[nameCube], nameCube, posVec, typeI);

            }
        }
    }
    // create top cubes from loaded/saved data
    private void createTopCubesFromGD(int numOfCols, int numOfRows, float sizeCubes, float intCubes, float baseYHeight, float topYHeight)
    {
        int nameCube = 0;
        cubeCarved = new GameObject[numOfCols * numOfRows, 2];
        cubeCarvedParent = new GameObject[numOfCols * numOfRows];

        for (var i = 0; i < numOfCols; i++)
        {
            for (var j = 0; j < numOfRows; j++)
            {
                nameCube = i * numOfRows + j;
                //generate random number 0 and 1 to represent different type of cubes, 0 is line, 1 is L
                int typeI = cubeType[nameCube];
                int statI = cubeStat[nameCube];
                cubeType[nameCube] = typeI;
                Vector3 posVec = new Vector3(i * (sizeCubes + intCubes) + sizeCubes / 2, baseYHeight / 2, j * (sizeCubes + intCubes) + sizeCubes / 2);
                createParentCubes(ref cubeCarvedParent[nameCube], nameCube, posVec);
                createChildCubesFromGD(ref cubeCarved[nameCube, 0], ref cubeCarved[nameCube, 1], ref cubeCarvedParent[nameCube], nameCube, posVec, typeI, statI);

            }
        }
    }
    private void createChildCubesFromGD(ref GameObject GO1, ref GameObject GO2, ref GameObject GOP, int Id, Vector3 posVec, int typeI, int statI)
    {
        // if cube is line, still combined by two line object
        if (typeI == 0)
        {
            // Children Gameobject 0
            GO1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GO1.name = "CubeCarvedChild" + Id + "_0";
            GO1.transform.localScale += new Vector3(-(1 - brn / 2), -(1 - topYHeight), sizeCubes - 1 - brn / 2);
            GO1.transform.position = posVec + new Vector3(-brn / 4, 0, 0);
            rend = GO1.GetComponent<Renderer>();
            rend.material.color = topOriColor;
            GO1.transform.parent = GOP.transform;

            // Children Gameobject 1
            GO2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GO2.name = "CubeCarvedChild" + Id + "_1";
            GO2.transform.localScale += new Vector3(-(1 - brn / 2), -(1 - topYHeight), sizeCubes - 1 - brn / 2);
            GO2.transform.position = posVec + new Vector3(brn / 4, 0, 0); ;
            rend = GO2.GetComponent<Renderer>();
            rend.material.color = topOriColor;
            GO2.transform.parent = GOP.transform;

            //now change the state of the cubes from the saved data: 0 vetical, 1 horizontal
            setCubeAConStat(Id, typeI, statI);
            if (statI == 1)
            {
                GOP.transform.eulerAngles = new Vector3(0, 90, 0);
            }
        }
        //if the cube is L shape, one vertical and one horizontal
        else
        {
            // Children Gameobject 0, vertical part
            GO1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GO1.name = "CubeCarvedChild" + Id + "_0";
            GO1.transform.localScale += new Vector3(-(1 - brn), -(1 - topYHeight), sizeCubes / 2 - brn / 2 + brn / 2 - 1);
            GO1.transform.position = posVec + new Vector3(0, 0, (sizeCubes / 2 - brn / 2 - brn / 2) / 2);
            rend = GO1.GetComponent<Renderer>();
            rend.material.color = topOriColor;
            GO1.transform.parent = GOP.transform;

            // Children Gameobject 1
            GO2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GO2.name = "CubeCarvedChild" + Id + "_1";
            GO2.transform.localScale += new Vector3(sizeCubes / 2 - brn / 2 - 1, -(1 - topYHeight), -(1 - brn));
            GO2.transform.position = posVec + new Vector3((sizeCubes / 2 - brn / 2) / 2, 0, 0);
            rend = GO2.GetComponent<Renderer>();
            rend.material.color = topOriColor;
            GO2.transform.parent = GOP.transform;

            //now change the state of the cubes from the saved data: 0 vetical, 1 horizontal
            setCubeAConStat(Id, typeI, statI);
            if (statI != 0)
            {
                GOP.transform.eulerAngles = new Vector3(0, statI * 90, 0);
            }
        }
    }
    // create parenet cubes
    private void createParentCubes(ref GameObject GO, int Id, Vector3 posVec)
    {
        // Parent Gameobject
        GO = new GameObject();
        GO.name = "CubeCarvedParent" + Id;
        GO.transform.position = posVec;
    }
    // create child cubes and add them to their parents: GO is the cube, GOP is the parent
    private void createChildCubes(ref GameObject GO1, ref GameObject GO2, ref GameObject GOP, int Id, Vector3 posVec, int typeI)
    {
        // if cube is line, still combined by two line object
        if (typeI == 0)
        {
            // Children Gameobject 0
            GO1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GO1.name = "CubeCarvedChild" + Id + "_0";
            GO1.transform.localScale += new Vector3(-(1 - brn / 2), -(1 - topYHeight), sizeCubes - 1 - brn / 2);
            GO1.transform.position = posVec + new Vector3(-brn / 4, 0, 0);
            rend = GO1.GetComponent<Renderer>();
            rend.material.color = topOriColor;
            GO1.transform.parent = GOP.transform;

            // Children Gameobject 1
            GO2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GO2.name = "CubeCarvedChild" + Id + "_1";
            GO2.transform.localScale += new Vector3(-(1 - brn / 2), -(1 - topYHeight), sizeCubes - 1 - brn / 2);
            GO2.transform.position = posVec + new Vector3(brn / 4, 0, 0); ;
            rend = GO2.GetComponent<Renderer>();
            rend.material.color = topOriColor;
            GO2.transform.parent = GOP.transform;

            //now randomly change the state of the cubes: 0 vetical, 1 horizontal
            int statI = Random.Range(0, 2);
            setCubeAConStat(Id, typeI, statI);
            if (statI == 1)
            {
                GOP.transform.eulerAngles = new Vector3(0, 90, 0);
            }
        }
        //if the cube is L shape, one vertical and one horizontal
        else
        {
            // Children Gameobject 0, vertical part
            GO1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GO1.name = "CubeCarvedChild" + Id + "_0";
            GO1.transform.localScale += new Vector3(-(1 - brn), -(1 - topYHeight), sizeCubes / 2 - brn / 2 + brn / 2 - 1);
            GO1.transform.position = posVec + new Vector3(0, 0, (sizeCubes / 2 - brn / 2 - brn / 2) / 2);
            rend = GO1.GetComponent<Renderer>();
            rend.material.color = topOriColor;
            GO1.transform.parent = GOP.transform;

            // Children Gameobject 1
            GO2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GO2.name = "CubeCarvedChild" + Id + "_1";
            GO2.transform.localScale += new Vector3(sizeCubes / 2 - brn / 2 - 1, -(1 - topYHeight), -(1 - brn));
            GO2.transform.position = posVec + new Vector3((sizeCubes / 2 - brn / 2) / 2, 0, 0);
            rend = GO2.GetComponent<Renderer>();
            rend.material.color = topOriColor;
            GO2.transform.parent = GOP.transform;

            //now randomly change the state of the cubes: 0 vetical, 1 horizontal
            int statI = Random.Range(0, 4);
            setCubeAConStat(Id, typeI, statI);
            if (statI != 0)
            {
                GOP.transform.eulerAngles = new Vector3(0, statI * 90, 0);
            }
        }
    }
    private void destroyCubes(int Id)
    {
        Destroy(cubeCarvedParent[Id], 0.1f);
    }
    private void setCubeAConStat(int Id, int typeI, int statI)
    {
        // TypeI: 0 is line , 1 is L shape
        if (typeI == 0)
        {
            if (statI == 0)
            {
                cubeStat[Id] = 0;
                conStat[Id, 0] = 1;
                conStat[Id, 1] = 0;
                conStat[Id, 2] = 0;
                conStat[Id, 3] = 1;
            }
            else
            {
                cubeStat[Id] = 1;
                conStat[Id, 0] = 0;
                conStat[Id, 1] = 1;
                conStat[Id, 2] = 1;
                conStat[Id, 3] = 0;
            }
        }
        else
        {
            if (statI == 0)
            {
                cubeStat[Id] = 0;
                conStat[Id, 0] = 1;
                conStat[Id, 1] = 1;
                conStat[Id, 2] = 0;
                conStat[Id, 3] = 0;
            }
            else if (statI == 1)
            {
                cubeStat[Id] = 1;
                conStat[Id, 0] = 0;
                conStat[Id, 1] = 1;
                conStat[Id, 2] = 0;
                conStat[Id, 3] = 1;
            }
            else if (statI == 2)
            {
                cubeStat[Id] = 2;
                conStat[Id, 0] = 0;
                conStat[Id, 1] = 0;
                conStat[Id, 2] = 1;
                conStat[Id, 3] = 1;
            }
            else if (statI == 3)
            {
                cubeStat[Id] = 3;
                conStat[Id, 0] = 1;
                conStat[Id, 1] = 0;
                conStat[Id, 2] = 1;
                conStat[Id, 3] = 0;
            }

        }


    }
    // check the left mouse change, to change the color of the base cube
    private void checkLeftMouseClicked(float szdis, float itdis)
    {
        // Check the left mouse input
        if (Input.GetMouseButtonDown(0))
        {
            // the position of mouses are changed into global coordinates
            // the position is determined by the camera height
            float camHgt = Camera.main.transform.position.y;
            float[] posTemp = giveGbMousePos(camHgt - baseYHeight);
            float xMousePos = posTemp[0];
            float zMousePos = posTemp[1];
            int xIndex = 0;
            int zIndex = 0;
            int nameCubeTemp = 0;
            xIndex = (int)Mathf.Floor(xMousePos / (szdis + itdis));
            zIndex = (int)Mathf.Floor(zMousePos / (szdis + itdis));
            if (((xMousePos > (xIndex - 1) * (szdis + itdis)) && ((xMousePos < (xIndex * (szdis + itdis) + szdis)))) && ((zMousePos > (zIndex - 1) * (szdis + itdis)) && ((zMousePos < (zIndex * (szdis + itdis) + szdis)))))
            {
                GameObject cubeTemp = new GameObject();
                nameCubeTemp = xIndex * numOfRows + zIndex;
                cubeTemp = GameObject.Find("Cube" + nameCubeTemp);
                rend = cubeTemp.GetComponent<Renderer>();
                if (rend.material.color == Color.yellow)
                {
                    rend.material.color = Color.green;
                }
                else if (rend.material.color == Color.green)
                {
                    rend.material.color = Color.yellow;
                }
            }
        }
    }
    // check the right mouse chagne, to change the color and make the rotation
    private void checkRightMouseClicked(float szdis, float itdis)
    {
        // Check the right mouse input
        if (Input.GetMouseButtonDown(0))
        {
            // The position of mouses are changed into global coordinates
            // the position is determined by the camera height
            float camHgt = Camera.main.transform.position.y;
            float[] posTemp = giveGbMousePos(camHgt - baseYHeight - topYHeight);
            float xMousePos = posTemp[0];
            float zMousePos = posTemp[1];

            int xId = 0;
            int zId = 0;
            // Get the x and z id of the cubes
            xId = (int)Mathf.Floor(xMousePos / (szdis + itdis));
            zId = (int)Mathf.Floor(zMousePos / (szdis + itdis));
            int nameCubeTemp = xId * numOfRows + zId;

            if ((xId < 0) || (xId >= numOfCols) || (zId < 0) || (zId >= numOfRows))
            {
                return;
            }

            // Let the change to that cube begin and disable more input on the cube induced
            if (((xMousePos > xId * (szdis + itdis)) && ((xMousePos < (xId * (szdis + itdis) + szdis)))) && ((zMousePos > zId * (szdis + itdis)) && ((zMousePos < (zId * (szdis + itdis) + szdis)))) && (rotateTag[nameCubeTemp] == 0))
            {
                // Get the corresponding cube
                GameObject cubeTemp = GameObject.Find("CubeCarvedParent" + nameCubeTemp);
                // Get ready for the rotation to begin and continue
                rotateTag[nameCubeTemp] = 1;
                cubeRotate[nameCubeTemp] = cubeTemp;
                countInUse[nameCubeTemp] = rotCount;
                // Change the color of the cube
                rend = cubeTemp.transform.GetChild(0).gameObject.GetComponent<Renderer>();
                rend.material.color = topRotColor;
                rend = cubeTemp.transform.GetChild(1).gameObject.GetComponent<Renderer>();
                rend.material.color = topRotColor;
            }
        }
    }
    private void rotationContinue()
    {
        int nameCube = -1;
        for (var i = 0; i < numOfCols; i++)
        {
            for (var j = 0; j < numOfRows; j++)
            {
                nameCube = i * numOfRows + j;
                if (rotateTag[nameCube] == 1)
                {
                    rotationMode(rotMode, cubeRotate[nameCube], countInUse[nameCube], rotCount);
                    countInUse[nameCube] -= 1;
                    if (countInUse[nameCube] == 0)
                    {
                        rotateTag[nameCube] = 0;
                        changeStatAfterRotation(nameCube);
                        //Debug.Log(" x=" + i + " y= " + j + " statI: " + cubeStat[nameCube] + " typeI: " + cubeType[nameCube]);
                    }
                }
            }
        }
    }
    private void changeStatAfterRotation(int Id)
    {
        int typeI = cubeType[Id];
        int statI = cubeStat[Id];
        // cubeType 0 is line shape
        if (typeI == 0)
        {
            if (statI == 0)
            {
                setCubeAConStat(Id, typeI, 1);
            }
            else if (statI == 1)
            {
                setCubeAConStat(Id, typeI, 0);
            }
        }
        // cubeType 1 is L shape
        else
        {
            if (statI == 0)
            {
                setCubeAConStat(Id, typeI, 1);
            }
            else if (statI == 1)
            {
                setCubeAConStat(Id, typeI, 2);
            }
            else if (statI == 2)
            {
                setCubeAConStat(Id, typeI, 3);
            }
            else if (statI == 3)
            {
                setCubeAConStat(Id, typeI, 0);
            }
        }
    }
    private void rotationMode(int md, GameObject tCb, int ct, int rotCot)
    {
        // Mode 0: constant speed; Mode 1: peak; Mode 2: valley
        if (md == 0)
        {
            tCb.transform.Rotate(Vector3.up * 90 / rotCot);
        }
        else if (md == 1)
        {
            float dt = (float)360 / rotCot / rotCot;
            Debug.Log("ct = " + ct);
            if (ct > (rotCot / 2))
            {
                tCb.transform.Rotate(Vector3.up * dt / 2 * (1 + (rotCot - ct) * 2));
            }
            else
            {
                tCb.transform.Rotate(Vector3.up * dt / 2 * (ct * 2 - 1));
            }
        }
        else if (md == 2)
        {
            float dt = (float)360 / rotCot / rotCot;
            if (ct > (rotCot / 2))
            {
                tCb.transform.Rotate(Vector3.up * dt / 2 * (ct * 2 - rotCot - 1));
            }
            else
            {
                tCb.transform.Rotate(Vector3.up * dt / 2 * (rotCot + 1 - 2 * ct));
            }
        }
    }
    private float[] giveGbMousePos(float calcuHeight)
    {
        var v3 = Input.mousePosition;
        v3.z = calcuHeight;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        float[] pos = { 0.0f, 0.0f };
        pos[0] = v3.x;
        pos[1] = v3.z;
        return pos;
    }
    // get the neighbor list
    private void getIniNeighbor()
    {
        int nameCube = -1;
        for (var i = 0; i < numOfCols; i++)
        {
            for (var j = 0; j < numOfRows; j++)
            {

                int tempup = j + 1;
                int tempright = i + 1;
                int tempdown = j - 1;
                int templeft = i - 1;
                if (tempup >= numOfRows)
                {
                    tempup -= numOfRows;
                }
                if (tempright >= numOfCols)
                {
                    tempright -= numOfCols;
                }
                if (tempdown < 0)
                {
                    tempdown += numOfRows;
                }
                if (templeft < 0)
                {
                    templeft += numOfCols;
                }
                nameCube = i * numOfRows + j;

                neiList[nameCube, 0] = i * numOfRows + tempup;
                neiList[nameCube, 1] = tempright * numOfRows + j;
                neiList[nameCube, 3] = i * numOfRows + tempdown;
                neiList[nameCube, 2] = templeft * numOfRows + j;
            }
        }
    }
    // get the connected neigbour list
    private void getConnectNeigbor()
    {

        for (var i = 0; i < numOfRows * numOfCols; i++)
        {
            conNeiList[i, 0] = 0;
            conNeiList[i, 1] = -1;
            conNeiList[i, 2] = -1;

            int typeI = cubeType[i];
            int statI = cubeStat[i];
            if (typeI == 0)
            {
                if (statI == 0)
                {
                    getConNeighborDir(i, 0);
                    getConNeighborDir(i, 3);
                }
                else if (statI == 1)
                {
                    getConNeighborDir(i, 1);
                    getConNeighborDir(i, 2);
                }
            }
            else if (typeI == 1)
            {
                if (statI == 0)
                {
                    getConNeighborDir(i, 0);
                    getConNeighborDir(i, 1);
                }
                else if (statI == 1)
                {
                    getConNeighborDir(i, 1);
                    getConNeighborDir(i, 3);
                }
                else if (statI == 2)
                {
                    getConNeighborDir(i, 2);
                    getConNeighborDir(i, 3);
                }
                else if (statI == 3)
                {
                    getConNeighborDir(i, 2);
                    getConNeighborDir(i, 0);
                }
            }
        }
    }
    // get the local connected neighbour
    private void getConNeighborDir(int i, int dir)
    {
        int nbn = neiList[i, dir];
        if ((conStat[i, dir] & conStat[nbn, 3 - dir]) == 1)
        {
            conNeiList[i, 0]++;
            conNeiList[i, conNeiList[i, 0]] = nbn;
        }

    }
    // check the lists connected for the first time
    private void createGlobalConList()
    {
        // cubeListNum stores the information, which list the cube is in and which index the cube is in, for later on interpolation use
        cubeCalYet = new int[numOfCols * numOfRows];
        int preventOverFlow = overFlowLength;
        for (var i = 0; i < numOfCols * numOfRows; i++)
        {
            cubeCalYet[i] = 0;
        }

        for (var i = 0; i < numOfCols * numOfRows; i++)
        {

            if (cubeCalYet[i] == 0)
            {
                cbList[listNum] = new ArrayList();
                cbList[listNum].Add(i);
                cubeCalYet[i] = 1;
                if (conNeiList[i, 0] == 0)
                {
                    listClosed[listNum] = 0;
                    listNum += 1;
                }
                // go cycling list
                else if (conNeiList[i, 0] == 1)
                {
                    // Get temp node,  add the node into list
                    int tempConNode = conNeiList[i, 1];
                    // tempConNodeBef stores the point that is ahead of tempConNodeBef
                    int tempConNodeBef = i;
                    // add the node int list
                    cbList[listNum].Add(tempConNode);
                    cubeCalYet[tempConNode] = 1;

                    while (conNeiList[tempConNode, 0] == 2)
                    {
                        preventOverFlow -= 1;
                        if (preventOverFlow == 0)
                        {
                            preventOverFlow = overFlowLength;
                            break;
                        }
                        // Get the other connecting point
                        int tt = tempConNode;
                        tempConNode = (tempConNodeBef == conNeiList[tempConNode, 1] ? conNeiList[tempConNode, 2] : conNeiList[tempConNode, 1]);
                        tempConNodeBef = tt;
                        // Reget temp node                       
                        cbList[listNum].Add(tempConNode);
                        cubeCalYet[tempConNode] = 1;
                    }
                    listClosed[listNum] = 0;
                    listNum += 1;
                }
                else if (conNeiList[i, 0] == 2)
                {
                    // Get the two temp node
                    int[] tempConNode = new int[2];
                    int tempConNodeBef = i;
                    tempConNode[0] = conNeiList[i, 1];
                    tempConNode[1] = conNeiList[i, 2];
                    // Add the left node, namely the tempConNode[0]

                    cbList[listNum].Add(tempConNode[0]);
                    cubeCalYet[tempConNode[0]] = 1;

                    int closeListFlag = 0;

                    while (conNeiList[tempConNode[0], 0] == 2)
                    {
                        preventOverFlow -= 1;
                        if (preventOverFlow == 0)
                        {
                            preventOverFlow = overFlowLength;
                            break;
                        }
                        // Get the other connecting point
                        int tt = tempConNode[0];
                        tempConNode[0] = (tempConNodeBef == conNeiList[tempConNode[0], 1] ? conNeiList[tempConNode[0], 2] : conNeiList[tempConNode[0], 1]);
                        tempConNodeBef = tt;

                        if (tempConNode[0] == i)
                        {
                            listClosed[listNum] = 1;
                            listNum += 1;
                            closeListFlag = 1;
                            break;
                        }
                        // Reget temp node
                        cbList[listNum].Add(tempConNode[0]);
                        cubeCalYet[tempConNode[0]] = 1;
                    }
                    // if the list is not closed
                    if (closeListFlag == 0)
                    {
                        cbList[listNum].Reverse();
                        // Add the left node, namely the tempConNode[0]
                        // set the tempConNodeBef to the original point
                        tempConNodeBef = i;
                        //coFrom1To2(tempConNode[1], ref tpi, ref tpj);
                        cbList[listNum].Add(tempConNode[1]);
                        cubeCalYet[tempConNode[1]] = 1;

                        while (conNeiList[tempConNode[1], 0] == 2)
                        {
                            preventOverFlow -= 1;
                            if (preventOverFlow == 0)
                            {
                                preventOverFlow = overFlowLength;
                                break;
                            }
                            // Get the other connecting point
                            int tt = tempConNode[1];
                            tempConNode[1] = (tempConNodeBef == conNeiList[tempConNode[1], 1] ? conNeiList[tempConNode[1], 2] : conNeiList[tempConNode[1], 1]);
                            tempConNodeBef = tt;
                            // Reget temp node
                            cbList[listNum].Add(tempConNode[1]);
                            cubeCalYet[tempConNode[1]] = 1;
                        }
                        listClosed[listNum] = 0;
                        listNum += 1;
                    }
                }

            }
        }

    }
    // check the lists every movement, whether they are enclosed, and upgrade the connection list
    IEnumerator updateGlobalConList()
    {
        desDone = false;
        // set the list number to zero, and recalculate it
        listNum = 0;
        getConnectNeigbor();
        createGlobalConList();
        calDone = true;
        yield return null;
    }
    // check closed list and destroy them, in the mean time create new things
    private void checkClose()
    {
        desListId = new int[listNum];
        desListNum = 0;
        for (var i = 0; i < listNum; i++)
        {
            if (listClosed[i] == 1)
            {
                desListId[desListNum] = i;
                desListNum += 1;
            }
        }
    }
    // change color of the closed list
    private void changeColorClosedLists()
    {
        if (desListNum > 0)
        {
            for (var i = 0; i < desListNum; i++)
            {
                // get the list ID
                int lisId = desListId[i];
                for (var j = 0; j < cbList[lisId].Count; j++)
                {
                    int tempNode = (int)cbList[lisId][j];
                    rend = cubeCarved[tempNode, 0].GetComponent<Renderer>();
                    rend.material.color = topAftColor;
                    Fade(cubeCarved[tempNode, 0]);
                    rend = cubeCarved[tempNode, 1].GetComponent<Renderer>();
                    rend.material.color = topAftColor;
                    Fade(cubeCarved[tempNode, 1]);
                }
            }
        }
    }
    // fade the color
    private void Fade(GameObject GO)
    {
        for (float f = 1f; f >= 0; f -= 0.1f)
        {
            rend = GO.GetComponent<Renderer>();
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
        }
    }
    // this destroy method is only for in Start()
    private void desClosedLists()
    {
        if (desListNum > 0)
        {
            for (var i = 0; i < desListNum; i++)
            {
                // get the list ID
                int lisId = desListId[i];
                // destroy the child object and establish new ones
                for (var j = 0; j < cbList[lisId].Count; j++)
                {
                    // get the object ID
                    int tempNode = (int)cbList[lisId][j];
                    Vector3 posVec = cubeCarvedParent[tempNode].transform.position;
                    // destroy the cubes
                    destroyCubes(tempNode);
                    // establish new one, first find the parent cube and attatch new ones
                    //generate random number 0 and 1 to represent different type of cubes, 0 is line, 1 is L
                    int typeI = Random.Range(0, 4);
                    if (typeI != 0)
                    {
                        typeI = 1;
                    }
                    cubeType[tempNode] = typeI;
                    createParentCubes(ref cubeCarvedParent[tempNode], tempNode, posVec);
                    createChildCubes(ref cubeCarved[tempNode, 0], ref cubeCarved[tempNode, 1], ref cubeCarvedParent[tempNode], tempNode, posVec, typeI);
                }
                pointsEarned += facTimes(cbList[lisId].Count);
                // show score Text
                scoreText.text = "Points:   " + pointsEarned.ToString();
                scoreText1.text = "Points:   " + pointsEarned.ToString();
            }
        }
        desListNum = 0;
        desListId = new int[listNum];
    }
    // destroy closed list and create new cubes
    private void desClosedListsDetermination()
    {
        if (desListNum > 0)
        {
            for (var i = 0; i < desListNum; i++)
            {
                // get the list ID
                int lisId = desListId[i];
                desCubes += cbList[lisId].Count;
                // destroy the child object and establish new ones
                for (var j = 0; j < cbList[lisId].Count; j++)
                {
                    // get the object ID
                    int tempNode = (int)cbList[lisId][j];
                    desCubeId.Add(tempNode);
                }
            }
            desTime = desCubes * desDeltaTime + 1;
            tDesI = desCubes;
        }
    }
    private void desContinue()
    {
        int tempNode = 0;
        if (desListNum > 0)
        {

            desTime -= 1;
            if (desTime == tDesI * desDeltaTime)
            {
                tempNode = (int)desCubeId[desCubes - tDesI];
                rend = cubeCarved[tempNode, 0].GetComponent<Renderer>();
                rend.material.color = topAftColor;
                rend = cubeCarved[tempNode, 1].GetComponent<Renderer>();
                rend.material.color = topAftColor;

                tDesI -= 1;
                if (tDesI == 0)
                {
                    for (var i = 0; i < desCubes; i++)
                    {
                        destroyCubes((int)desCubeId[i]);
                    }
                    for (var i = 0; i < desListNum; i++)
                    {
                        int lisId = desListId[i];
                        pointsEarned += facTimes(cbList[lisId].Count);
                        // show score Text
                        scoreText.text = "Points:   " + pointsEarned.ToString();
                        scoreText1.text = "Points:   " + pointsEarned.ToString();
                    }
                    desCubes = 0;
                    desCubeId = new ArrayList();
                    desProDone = true;
                }
            }
        }
    }
    private void createClosedListNew()
    {
        if (desListNum > 0)
        {
            for (var i = 0; i < desListNum; i++)
            {
                // get the list ID
                int lisId = desListId[i];
                // destroy the child object and establish new ones
                for (var j = 0; j < cbList[lisId].Count; j++)
                {
                    // get the object ID
                    int tempNode = (int)cbList[lisId][j];
                    Vector3 posVec = cubeCarvedParent[tempNode].transform.position;
                    // establish new one, first find the parent cube and attatch new ones
                    //generate random number 0 and 1 to represent different type of cubes, 0 is line, 1 is L
                    int typeI = Random.Range(0, 4);
                    if (typeI != 0)
                    {
                        typeI = 1;
                    }
                    cubeType[tempNode] = typeI;
                    createParentCubes(ref cubeCarvedParent[tempNode], tempNode, posVec);
                    createChildCubes(ref cubeCarved[tempNode, 0], ref cubeCarved[tempNode, 1], ref cubeCarvedParent[tempNode], tempNode, posVec, typeI);
                }
            }
        }
        desListNum = 0;
        desListId = new int[listNum];
    }
    // defines how to calculate score
    private int facTimes(int nnn)
    {
        int res = 0;
        for (int i = 1; i <= nnn; i++)
        {
            res = res + 2 * i - 1;
        }
        return res;
    }
    private void coFrom1To2(int cor, ref int tpi, ref int tpj)
    {
        tpi = (int)Mathf.Floor(cor / numOfRows);
        tpj = cor % numOfRows;
    }
    private void debugOutputScripts()
    {
        int cubesCaled = 0;
        for (var i = 0; i < numOfCols * numOfRows; i++)
        {
            if (cubeCalYet[i] == 1)
            {
                cubesCaled += 1;
            }
            Debug.Log("Points ID:" + i + " connection Stat: " + conStat[i, 0] + " " + conStat[i, 1] + " " + conStat[i, 2] + " " + conStat[i, 3] + " " + " \n");
            Debug.Log("Points ID:" + i + " neiList: " + neiList[i, 0] + " " + neiList[i, 1] + " " + neiList[i, 2] + " " + neiList[i, 3] + " " + " \n");
            Debug.Log("Points ID:" + i + " connecte neiList number: " + conNeiList[i, 0] + " node: " + conNeiList[i, 1] + " " + conNeiList[i, 2] + " \n");
        }

        for (var i = 0; i < listNum; i++)
        {
            Debug.Log("List:" + i + " Closed:" + listClosed[i] + " \n");
            for (var j = 0; j < cbList[i].Count; j++)
            {
                Debug.Log("  " + cbList[i][j]);
            }
            Debug.Log(" \n");
        }
        int testall = 0;
        int closeList = 0;
        Debug.Log(" List: " + listNum);
        for (var i = 0; i < listNum; i++)
        {
            testall += cbList[i].Count;
            if (listClosed[i] == 1)
            {
                closeList += 1;
            }
        }
        Debug.Log("Cubes scanned: " + cubesCaled + " Total Cubes in list: " + testall + " ClosedList: " + closeList);
        Debug.Log(" Points: " + pointsEarned);
    }
}
