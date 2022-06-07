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
    
    public float health = 0;
    public int damage = 0;
    public int speed = 0;

    public Dictionary<string, int> invintory = new Dictionary<string, int>();

    [SerializeField] public bool alive = true;
    private float lastPlayerHit = 0f;

    public TextMeshProUGUI TimeTextUiElement;
    public TextMeshProUGUI ScoreTextUiElement;
    public TextMeshProUGUI GameOverScoreTextUiElement;
    
    public TextMeshProUGUI BlobCountText;

    public GameObject GameOverText;

    public GameObject healthBar;
    [NonSerialized] public Slider healthBarSlider;

    public GameTimer Timer;

    public void AddItemToInvintory(string name, int amount) {
        if (invintory.ContainsKey(name)) {
            invintory[name] += amount;
        } else{
            invintory[name] = amount;
        }
    }
    public int GetItemFromInvintory (string name) {
        return invintory[name];
    }

    public void RemoveItemFromInvintory (string name, int amount) {
        if (invintory.ContainsKey(name)) {
            invintory[name] -= amount;
        }
    }


     void OnTriggerEnter(Collider other) {
        if (other.gameObject.transform.parent.gameObject.name == "RedBlobDrop") {
            AddItemToInvintory("RedBlobDrop", 10);
            BlobCountText.text = "Blob " + GetItemFromInvintory("RedBlobDrop");

            Destroy(other.gameObject);
        }
     }

    void Start(){
        Timer = new GameTimer();
        Timer.TextElement = TimeTextUiElement;
        Timer.StartTimer();

        healthBarSlider = healthBar.GetComponent<Slider>();

    }

    public void takeDamage(float amount)
    {
        if (alive)
        {
            health -= amount;
            healthBarSlider.value = health;
            if (health <= 0f)
            {
                alive = false;
                GameOverText.SetActive(true);
                GameOverScoreTextUiElement.gameObject.SetActive(true);
                Timer.Stop();
                Game.instance.Restart(); 
            }
        }
    }

    public void AddScore(int score) {

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
