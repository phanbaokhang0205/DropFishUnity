using Unity.VisualScripting;
using UnityEngine;

public class Fish : MonoBehaviour
{
    private FishManager fishManager;
    private Rigidbody2D fishRb;

    public bool inWater;
    public bool isDropped;

    void Start()
    {
        fishManager = FishManager.Instance;

        //fishRb = gameObject.GetComponent<Rigidbody2D>();

        inWater = false;
        isDropped = false;

        prepareToDrop();
    }

    void Update()
    {

    }

    public void prepareToDrop()
    {
        fishRb = GetComponent<Rigidbody2D>();

        fishRb.gravityScale = 0;
        fishRb.constraints |= RigidbodyConstraints2D.FreezeRotation;
    }

    public void dropped()
    {
        isDropped = true;
        fishRb.gravityScale = 1;
        fishRb.constraints &= ~RigidbodyConstraints2D.FreezeRotation;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "water")
        {
            inWater = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "water")
        {
            inWater = false;
            isDropped = false;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        fishManager.MergeFish(gameObject, collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        fishManager.MergeFish(gameObject, collision.gameObject);

    }


}
