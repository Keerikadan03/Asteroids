using UnityEngine;

public class Asteroid : MonoBehaviour
{   
    public Sprite[] sprites; // creating an arrayof sprites  
       

    public float size = 1.0f; //initialising asteroid size
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float speed = 5.0f;

    public float maxLifeTime = 30.0f;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody;

    private void Awake(){
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start(){
        _spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)]; //random from array

        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f); //randomize location
        this.transform.localScale = Vector3.one * this.size; //assigning size to the asteroid

        _rigidbody.mass = this.size;

    }

    void Update () {
 
        // Teleport the game object
        if(transform.position.x > 9){
 
            transform.position = new Vector3(-9, transform.position.y, 0);
 
        }
        else if(transform.position.x < -9){
            transform.position = new Vector3(9, transform.position.y, 0);
        }
 
        else if(transform.position.y > 6){
            transform.position = new Vector3(transform.position.x, -6, 0);
        }
 
        else if(transform.position.y < -6){
            transform.position = new Vector3(transform.position.x, 6, 0);
        }
    }

    public void SetTrajectory(Vector2 direction){
       _rigidbody.AddForce(direction * this.speed);
       Destroy(this.gameObject,this.maxLifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Bullet"){ // when asteroid collides with bullet
            if(this.size * 0.5f >= minSize){      //splits into two halfs if mass/2 >= min size
                CreateSplit();
                CreateSplit();
            }
            FindObjectOfType<GameManager>().AsteroidDestroyed(this);
            Destroy(this.gameObject);
        }else if(collision.gameObject.tag == "Asteroid"){
            this.speed = -this.speed/2;
        }
    }


    private void CreateSplit(){
        Vector2 position = this.transform.position; //same position of parent asteroid
        position += Random.insideUnitCircle * 0.5f; // offsets the created asteroids in random direction
        Asteroid half = Instantiate(this, position, this.transform.rotation); //instantiates each half
        half.size = this.size * 0.5f;//sets their size to half of parent asteroid
        half.SetTrajectory(Random.insideUnitCircle.normalized * this.speed);
    }
}
