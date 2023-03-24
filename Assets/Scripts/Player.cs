using UnityEngine;

public class Player : MonoBehaviour
{   
    public Bullet bulletPrefab; //reference to BUllet file

    private Rigidbody2D _rigidbody; //reference to rigidbody component for consistent physics

    public float thrustSpeed = 1.0f;// speed of the player's forward movement 
    public float turnSpeed = 1.0f;//speed of player's turn movement

    private bool _thrusting; // move forward
    private bool _decelerate;//slow down
    private float _turndirection; // change directions

    private void Update(){
        _thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        _decelerate = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            _turndirection = 1.0f;
        }else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            _turndirection = -1.0f;
        }else{
            _turndirection = 0.0f;
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
            Shoot();
        }
    }

    private void Awake(){
        _rigidbody = GetComponent<Rigidbody2D>();  //checks the Player file for Rigidbody2D component
    }

    private void FixedUpdate(){
        if(_thrusting){
            _rigidbody.AddForce(this.transform.up * this.thrustSpeed);
        }
        if(_decelerate){
            _rigidbody.AddForce(-this.transform.up * this.thrustSpeed * 0.5f);
        }

        if(_turndirection != 0.0f){
            _rigidbody.AddTorque(_turndirection * this.turnSpeed);
        }
    }

    private void Shoot(){
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Asteroid"){
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = 0.0f;

            this.gameObject.SetActive(false); 

            FindObjectOfType<GameManager>().PlayerDied();
        }
    }
}
