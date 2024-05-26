using UnityEngine;

public class San : MonoBehaviour
{
    float startX, endX;
    SpriteRenderer sr;
    [SerializeField] float speed;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        endX = transform.position.x + 11;
        startX = transform.position.x;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    bool left;
    void Update()
    {
        if(transform.position.x <= startX && left == true)
        {
            left = false;
        }
        else if(transform.position.x >= endX && left == false)
        {
            left= true;
        }

        if(!left) 
        {
            transform.position += Vector3.right * Time.deltaTime * speed;
            sr.flipX = false;
        }
        else
        {
            transform.position -= Vector3.right * Time.deltaTime * speed;
            sr.flipX = true;
        }

        
    }
}
