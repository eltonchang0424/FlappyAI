using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSpawner : MonoBehaviour {

    public static PlayerSpawner instance;

    public GameObject player;
    public Text generationText;
    public Text alive;

    public bool play = false;

    float mutationRate = 0.15f;
    int populationSize = 150;
    public int populationAlive = 0;
    public Player[] players;

    int generation = 1;

    private void Awake()
    {
        instance = this;
    }

	void Start () {
        players = new Player[populationSize];
        populationAlive = populationSize;
        if (!play)
        {
            Populate();
        }
	}

    void Populate()
    {
        for (int i = 0; i < populationSize; i++)
        {
            GameObject p = Instantiate(player);
            players[i] = p.GetComponent<Player>();
        }
    }
	
    void Repopulate()
    {
        generation++;
        generationText.text = ("Generation: " + generation);

        float totalFitness = 0;
        float[] normalizedFitness = new float[populationSize];

        for (int i = 0; i < populationSize; i++)
        {
            totalFitness += players[i].score;
        }

        for (int i = 0; i < populationSize; i++)
        {
            normalizedFitness[i] = players[i].score / totalFitness;
        }

        NeuralNetwork[] nnArray = new NeuralNetwork[populationSize];
        NeuralNetwork[] childBrains;

        for (int i = 0; i < populationSize; i += 2)
        {
            int index1 = pickParent(normalizedFitness);
            int index2 = pickParent(normalizedFitness);
            childBrains = players[index1].nn.Crossover(players[index2].nn);
            nnArray[i] = childBrains[0];
            nnArray[i + 1] = childBrains[1];
            nnArray[i] = nnArray[i].Mutate(mutationRate);
            nnArray[i + 1] = nnArray[i + 1].Mutate(mutationRate);
        }

        for (int i = 0; i < populationSize; i++)
        {
            players[i].nn = nnArray[i];
            players[i].Reset();
        }
    }

    int pickParent(float[] normalizedFitness)
    {
        int count = 0;

        float selectionValue = Random.value;

        while(selectionValue > 0)
        {
            selectionValue -= normalizedFitness[count];
            count++;
        }

        count--;
        return count;
    }

    public void SpeedUp()
    {
        Time.timeScale = 20;
    }

    public void SlowDown()
    {
        Time.timeScale = 1;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

	void Update () 
    {
        alive.text = ("Alive: " + populationAlive);

        if (play)
        {
            return;
        }

        if (populationAlive == 0)
        {
            GameObject[] pipes = GameObject.FindGameObjectsWithTag("Obstacle");

            for (int i = 0; i < pipes.Length; i++)
            {
                Destroy(pipes[i]);
            }

            ObstacleSpawner.instance.obstaclesBot.Clear();
            ObstacleSpawner.instance.StopAllCoroutines();
            ObstacleSpawner.instance.spawnFirstPipe();
            ObstacleSpawner.instance.StartCoroutine(ObstacleSpawner.instance.spawnObstacle());
            populationAlive = populationSize;

            Repopulate();
        }
	}
}
