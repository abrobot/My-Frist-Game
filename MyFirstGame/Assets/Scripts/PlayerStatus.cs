using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class PlayerStatus : MonoBehaviour
{
    int score = 0;
    public float maxhealth = 100;
    public float health = 0;
    public int resistance = 0;
    public int damage = 0;
    public int speed = 0;

    public bool alive = true;
    public MoveDirection moveDirection;
    private float lastPlayerHit = 0f;

    public Dictionary<string, int> invintory = new Dictionary<string, int>();

    public TextMeshProUGUI TimeTextUiElement;
    public TextMeshProUGUI ScoreTextUiElement;
    public TextMeshProUGUI GameOverScoreTextUiElement;

    public TextMeshProUGUI BlobCountText;


    public InventoryManager inventoryManager;
    public GameObject GameOverText;

    public GameObject healthBar;
    [NonSerialized] public Slider healthBarSlider;
    public CharacterController characterController;

    public GameTimer Timer;

    public void AddItemToInvintory(string name, int amount)
    {
        if (invintory.ContainsKey(name))
        {
            invintory[name] += amount;
        }
        else
        {
            invintory[name] = amount;
        }
    }
    public int GetItemFromInvintory(string name)
    {
        return invintory[name];
    }

    public void RemoveItemFromInvintory(string name, int amount)
    {
        if (invintory.ContainsKey(name))
        {
            invintory[name] -= amount;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.gameObject.name == "RedBlobDrop" | other.gameObject.transform.parent.gameObject.name == "BlueBlobDrop")
        {
            inventoryManager.Add(inventoryManager.translateNameToInventoryItem(other.gameObject.transform.parent.gameObject.name), UnityEngine.Random.Range(1, 3));
            Destroy(other.gameObject);
        }
    }

    void Start()
    {
        Timer = new GameTimer();
        Timer.TextElement = TimeTextUiElement;
        Timer.StartTimer();

        healthBarSlider = healthBar.GetComponent<Slider>();
        healthBarSlider.maxValue = maxhealth;

    }

    void Update() {
        if ((Time.time - lastPlayerHit) > 5f && health < maxhealth) {
            heal(1 * Time.deltaTime);
        }
    }

    public void takeDamage(float amount)
    {
        if (alive)
        {
            health -= Mathf.Clamp(amount - resistance, 1, int.MaxValue);
            healthBarSlider.value = health;
            if (health <= 0f)
            {
                alive = false;
                GameOverText.SetActive(true);
                GameOverScoreTextUiElement.gameObject.SetActive(true);
                if (score != 0)
                {
                    string scoresPackage = PlayerPrefs.GetString("BestScores", "None");
                    PlayerScores playerScores = scoresPackage != "None" ? JsonUtility.FromJson<PlayerScores>(scoresPackage) : new PlayerScores();
                    playerScores.scores.Add(score);
                    scoresPackage = JsonUtility.ToJson(playerScores);
                    PlayerPrefs.SetString("BestScores", scoresPackage);
                }

                Timer.Stop();
                Game.instance.Restart();
            }
        }
    }


    public void heal(float amount)
    {
        if (alive)
        {
            health += amount;
            if (health > maxhealth)
            {
                health = maxhealth;
            }
            healthBarSlider.value = health;
        }
    }
    public void AddMaxHealth(float amount)
    {
        if (alive)
        {
            maxhealth += amount;
            healthBarSlider.maxValue = maxhealth;
        }
    }



    public void AddScore(int score)
    {

        this.score += score;
        ScoreTextUiElement.text = this.score.ToString();
        GameOverScoreTextUiElement.text = this.score.ToString();
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if ((Time.time - lastPlayerHit) > 0.5f)
            {
                takeDamage(10f);
                lastPlayerHit = Time.time;
            }
        }
    }
}
