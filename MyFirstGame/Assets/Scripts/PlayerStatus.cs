using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;


public class PlayerStatus : MonoBehaviour
{
    int score = 0;
    [SerializeField] public float health = 100f;
    [SerializeField] public bool alive = true;
    private float lastPlayerHit = 0f;

    public TextMeshProUGUI TimeTextUiElement;
    public TextMeshProUGUI ScoreTextUiElement;

    public GameObject GameOverText;

    public GameObject healthBar;
    [NonSerialized] public Slider healthBarSlider;

    public GameTimer Timer;


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
                Debug.Log("Player is dead");
                alive = false;
                GameOverText.SetActive(true);
                Timer.Stop();
            }
        }
    }

    public void AddScore(int score) {

        this.score += score;
        ScoreTextUiElement.text = this.score.ToString();
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
