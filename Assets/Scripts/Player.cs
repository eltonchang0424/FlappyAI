using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    Rigidbody2D rb;
    float jumpForce = 5.5f;

    public NeuralNetwork nn;
    float[] input;

    float widthPlayer;
    float heightPlayer;
    float heightPipe;
    float widthPipe;
    float diffX;
    public float score = 1;

    bool alive;

	void Start () 
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        nn = new NeuralNetwork(3, 7, 1);
        input = new float[3];
        alive = true;
	}

	private void Update()
	{
        if (PlayerSpawner.instance.play)
        {
            if (Input.GetMouseButton(0))
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
	}

	private void FixedUpdate()
	{
        if (PlayerSpawner.instance.play)
        {
            return;
        }

        if (alive)
        {
            score = Mathf.Pow(ObstacleSpawner.instance.score, 3);

            GameObject nextObstacle = ObstacleSpawner.instance.obstaclesBot.First.Value;
            //widthPipe = nextObstacle.transform.position.x / 5; //- gameObject.transform.position.x / 5;
            heightPlayer = gameObject.transform.position.y / 5; //- (nextObstacle.transform.position.y + (nextObstacle.transform.localScale.y / 2) + ObstacleSpawner.instance.distance / 2);
            //widthPlayer = gameObject.transform.position.x / 5;
            heightPipe = (nextObstacle.transform.position.y + (nextObstacle.transform.localScale.y / 2) + ObstacleSpawner.instance.distance / 2) / 5;
            diffX = (nextObstacle.transform.position.x - gameObject.transform.position.x) / (1.75f * 3.25f);

            input[0] = heightPlayer;
            input[1] = heightPipe;
            input[2] = diffX;

            float[] output = nn.FeedForward(input);
            if (output[0] >= 0.5f)
            {
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag("Obstacle"))
        {
            Die();
        }

        if (collision.CompareTag("Floor"))
        {
            Die();
        }
	}

    void Die()
    {
        if (alive)
        {
            gameObject.SetActive(false);
            alive = false;
            PlayerSpawner.instance.populationAlive--;
        }
    }

	public void Reset()
	{
        gameObject.SetActive(true);
        alive = true;
        transform.position = new Vector3(-1.769f, 0.139f, 0);
        score = 1;
        ObstacleSpawner.instance.score = 1;
	}
}
