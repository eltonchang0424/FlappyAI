using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleSpawner : MonoBehaviour {

    public static ObstacleSpawner instance;

    public GameObject botObstacle;
    public GameObject topObstacle;
    public GameObject player;
    public Text scoreText;
    public Text bestScore;

    public float score = 1;
    int best = 1;

    public float distance = 11f;
    float xOffset = -1.769f;
    float speed = 3.25f;
    float waitTime = 1.75f;
    float yPos;

    public LinkedList<GameObject> obstaclesBot;

    private void Awake()
    {
        instance = this;
    }

	void Start () 
    {
        Screen.SetResolution(1600, 2560, false);
        obstaclesBot = new LinkedList<GameObject>();
        StartCoroutine(spawnObstacle());
        spawnFirstPipe();
	}
	
	void FixedUpdate () 
    {
        if (obstaclesBot.First.Value != null && xOffset >= obstaclesBot.First.Value.transform.position.x + obstaclesBot.First.Value.transform.localScale.x / 2 + player.transform.localScale.x / 2)
        {
            Vector3 middlePoint = new Vector3(obstaclesBot.First.Value.transform.position.x, obstaclesBot.First.Value.transform.position.y + (obstaclesBot.First.Value.transform.localScale.y / 2) + distance / 2, 0);
            for (int i = 0; i < PlayerSpawner.instance.players.Length; i++)
            {
                float yOffset = middlePoint.y - PlayerSpawner.instance.players[i].transform.position.y;
                if (yOffset <= 0.2f && yOffset >= 0)
                {
                    PlayerSpawner.instance.players[i].score += 50;
                }
            }

            obstaclesBot.RemoveFirst();
            score++;
        }

        if (score >= best)
        {
            best = (int)score;
            bestScore.text = ("Best: " + best);
        }

        scoreText.text = ("Score: " + score);
	}

    public void spawnFirstPipe()
    {
        yPos = Random.Range(-6.0f, -1.5f);

        GameObject bot = Instantiate(botObstacle, new Vector3(xOffset + speed * waitTime, yPos, 0), Quaternion.identity);
        obstaclesBot.AddLast(bot);
        Destroy(bot, 10);
        GameObject top = Instantiate(topObstacle, new Vector3(xOffset + speed * waitTime, yPos + distance, 0), Quaternion.identity);
        Destroy(top, 10);

        bot.GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * speed, 0);
        top.GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * speed, 0);
    }

    public IEnumerator spawnObstacle ()
    {
        yPos = Random.Range(-6.0f, -1.5f);

        GameObject bot = Instantiate(botObstacle, new Vector3(xOffset + speed * waitTime * 2, yPos, 0), Quaternion.identity);
        obstaclesBot.AddLast(bot);
        Destroy(bot, 10);
        GameObject top = Instantiate(topObstacle, new Vector3(xOffset + speed * waitTime * 2, yPos + distance, 0), Quaternion.identity);
        Destroy(top, 10);

        bot.GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * speed, 0);
        top.GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * speed, 0);

        yield return new WaitForSeconds(waitTime);

        StartCoroutine(spawnObstacle());
    }
}
