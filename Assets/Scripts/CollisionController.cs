using Unity.VisualScripting;
using UnityEngine;

public class CollisionController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private FishManager fishManager;
    private bool inWater;

    private void Start()
    {
        fishManager = FishManager.Instance;
        inWater = false;
        Debug.Log(inWater);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        fishManager.MergeFish(gameObject, collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        fishManager.MergeFish(gameObject, collision.gameObject);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "water")
    //    {
    //        inWater = true;
    //        fishManager.nextFish = true;
    //    }
    //}




}
