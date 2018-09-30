using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
    public float Acceleration;
    public float GrowthSpeed;
    public float ShrinkRate;
    public GameObject Node;

    private bool isGrowing;
    private bool touchedFirstNode;
    private Sound sound;
    private Player player;
    private Vector3 initialPosition;
    private Vector3 nodePosition;

    private void Start()
    {
        initialPosition = transform.position;

        isGrowing = true;

        player = GameObject.Find("/Script Holder").GetComponent<Player>();

        sound = GameObject.Find("/Script Holder").GetComponent<Sound>();

        sound.PlayRoot();

        transform.localScale = new Vector3(transform.localScale.x * Mathf.Pow(ShrinkRate, player.RootParts), 0, transform.localScale.z * Mathf.Pow(ShrinkRate, player.RootParts));
    }

    private void Update()
    {
        Grow();
    }

    private void Grow()
    {
        if (isGrowing)
        {
            transform.localScale += new Vector3(0, ((player.RootParts * Acceleration) + GrowthSpeed) * Time.deltaTime, 0);

            if (player.MovementDirection == Player.Directions.Right)
            {
                transform.position = initialPosition + new Vector3(transform.localScale.y, 0, 0);
            }
            else if (player.MovementDirection == Player.Directions.Left)
            {
                transform.position = initialPosition + new Vector3(-transform.localScale.y, 0, 0);
            }
            else if (player.MovementDirection == Player.Directions.Up)
            {
                transform.position = initialPosition + new Vector3(0, 0, transform.localScale.y);
            }
            else if (player.MovementDirection == Player.Directions.Down)
            {
                transform.position = initialPosition + new Vector3(0, 0, -transform.localScale.y);
            }
            else
            {
                Debug.LogError(gameObject + " could not determine players movement direction!");
            }

            if (transform.localScale.y >= 1)
            {
                StopGrowing();
            }
        }
    }

    private void StopGrowing()
    {
        isGrowing = false;
        transform.localScale = new Vector3(0.5f * Mathf.Pow(ShrinkRate, player.RootParts), 1, 0.5f * Mathf.Pow(ShrinkRate, player.RootParts));

        if (player.MovementDirection == Player.Directions.Right)
        {
            nodePosition = initialPosition + new Vector3(2, 0, 0);
        }
        else if (player.MovementDirection == Player.Directions.Left)
        {
            nodePosition = initialPosition + new Vector3(-2, 0, 0);
        }
        else if (player.MovementDirection == Player.Directions.Up)
        {
            nodePosition = initialPosition + new Vector3(0, 0, 2);
        }
        else if (player.MovementDirection == Player.Directions.Down)
        {
            nodePosition = initialPosition + new Vector3(0, 0, -2);
        }
        else
        {
            Debug.LogError(gameObject + " could not determine players movement direction!");
        }

        Instantiate(Node, nodePosition, transform.rotation);

        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isGrowing)
        {
            if (!touchedFirstNode)
            {
                if (other.gameObject.tag == "Root Node" || other.gameObject.tag == "Starting Node")
                {
                    touchedFirstNode = true;
                }
            }
            else
            {
                if (other.gameObject.tag == "Root Node" || other.gameObject.tag == "Starting Node")
                {
                    player.Die();
                    Destroy(this.gameObject);
                }
                else if(other.gameObject.tag == "Loot")
                {
                    sound.PlayLoot();
                    Destroy(other.gameObject);
                    player.AddScore(100);
                }
            }
        }
    }
}

