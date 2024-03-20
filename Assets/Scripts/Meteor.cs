using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Meteor : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField][Range(0f, 0.8f)] public float forceMagnitudeRight = 0.2f;
    [SerializeField][Range(0f, 0.8f)] public float forceMagnitudeLeft = 0.2f;
    [SerializeField] public float forceMagnitudeDown = 50f;
    [SerializeField] float maxSpeed = 50f;
    [SerializeField] MeteorController meteorController;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Invoke("DestroySelf",2f);

    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            switch (meteorController.weatherType)
            {
                case WeatherType.EastWind:
                    //Debug.Log("东风!");
                    rb.AddForce(Vector2.right * forceMagnitudeRight, ForceMode2D.Impulse);
                    rb.AddForce(Vector2.down * (forceMagnitudeDown / 2), ForceMode2D.Force);
                    break;
                case WeatherType.WestWind:
                    //Debug.Log("西风!");
                    rb.AddForce(Vector2.left * forceMagnitudeLeft, ForceMode2D.Impulse);
                    rb.AddForce(Vector2.down * (forceMagnitudeDown / 2), ForceMode2D.Force);
                    break;
                case WeatherType.StraightWind:
                    //Debug.Log("垂直风!");
                    rb.AddForce(Vector2.down * forceMagnitudeDown, ForceMode2D.Force);
                    break;

            }
        }
        else
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        //Debug.Log(rb.velocity.magnitude);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BuildingBlock"))
        {
            collision.gameObject.GetComponent<BlockController>().takeDamage(1);
        }

        if (collision.gameObject.CompareTag("Factory"))
        {
            collision.gameObject.GetComponent<BuildingController>().takeDamage(1);
        }
        if (!collision.gameObject.CompareTag("Meteor"))
            Destroy(this.gameObject);

    }
    void DestroySelf()
    { 
        Destroy(this.gameObject);
    }
}
