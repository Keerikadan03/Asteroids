using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{   
    public Player player;
    public ParticleSystem explosion;

    public Text scoreText;
    public Text livesText;
    public int flag = 0;

    public int lives = 3;
    public int score = 0;

    public float respawnTime = 3.0f;            //respawn time after 3s
    public float InvulnerabilityTime = 3.0f;    //respawn invulnerability time

    void Start(){
        scoreText.text = score.ToString() + " POINTS";
        livesText.text = " LIVES = " + lives.ToString();
    }

    public void AsteroidDestroyed(Asteroid asteroid){
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        if(asteroid.size <= 0.75f){
            this.score += 100;
        }else if(asteroid.size <= 1.0f){
            this.score += 50;
        }else{
            this.score += 25;
        }
        scoreText.text = score.ToString() +  " POINTS";

        if(flag == 0){
            if(this.score >= 10000){
                AsteroidSpawner.instance.AddWave();
                flag = 1;
            }
        }
    }

    public void PlayerDied(){
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();
        this.lives--;
        livesText.text = " LIVES = " + lives.ToString();

        if(this.lives <= 0){                         // when player runs out of life gameover
            GameOver();
        }else{
            Invoke(nameof(Respawn), this.respawnTime); //invoking respawn after time delay of respawntime
        }
    }
    
    private void Respawn(){
        this.player.transform.position = Vector3.zero; //after respawn sets position to initial4
        this.player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollisions");//changes layer name to ignore collisions
        this.player.gameObject.SetActive(true);               //activates player object again after respawn 
        Invoke(nameof(TurnOnCollisions), 3.0f);    //turns on collisions after 3s(this is done so that player will get time to steer clear of asteroids for after respawn)
    }
    private void TurnOnCollisions(){
        this.player.gameObject.layer = LayerMask.NameToLayer("Player"); //reassign player layer
    }

    // private void GameOver(){
    //     this.lives = 3;
    //     this.score = 0;

    //     this.Invoke(nameof(Respawn), this.respawnTime);
    // }

    public void GameOver(){
        lives = 3;
        score = 0;
        SceneManager.LoadScene("GameOver");
    }
}
