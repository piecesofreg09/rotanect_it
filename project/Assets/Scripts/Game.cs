using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game
{
    // Corresponds to the SaveLoad class
    public static Game current = new Game();
    public int numOfRows = 12;
    public int numOfCols = 12;
    public int score;
    public float pastTime;
    public int aveScore;
    // Label the type of the cubes
    public int[] cubeType;
    // Label the state of the cubes
    public int[] cubeStat;

    public Game()
    {
        score = new int();
        aveScore = new int();
        cubeType = new int[numOfCols * numOfRows];
        cubeStat = new int[numOfCols * numOfRows];
        pastTime = new float();
    }

}
