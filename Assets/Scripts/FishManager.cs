using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public static FishManager Instance { get; private set; }

    public GameObject[] fishPrefabs;
    public List<GameObject> droppedFishes;
    public Rigidbody2D fishRb = null;
    public GameObject chosenFish = null;
    public bool nextFish;

    private string oldTag;
    private string newTag;
    private int newTagIndex;


    private string[] hexColors = {
        "#FF0000", // Đỏ
        "#00FF00", // Xanh lá
        "#0000FF", // Xanh dương
        "#FFFF00", // Vàng
        "#FF00FF", // Hồng
        "#00FFFF", // Xanh cyan
        "#FFA500", // Cam
        "#800080", // Tím
        "#008000", // Xanh lục đậm
        "#808080", // Xám
        "#FFC0CB"  // Hồng nhạt
    };
    private int colorIndex;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        droppedFishes = new List<GameObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //nextFish = false;
    }

    private void Update()
    {
        //nextFish = !chosenFish;
    }

    public void CreateFish(Vector3 spawnPosition)
    {
        if (fishPrefabs.Length == 0) return;

        int randomIndex = Random.Range(0, 4);
        GameObject newFish = Instantiate(fishPrefabs[randomIndex], spawnPosition, Quaternion.identity);

        chosenFish = newFish;
        fishRb = chosenFish.GetComponent<Rigidbody2D>();

        fishRb.gravityScale = 0;
        fishRb.constraints |= RigidbodyConstraints2D.FreezeRotation;

        newFish.transform.parent = this.transform;
    }

    public void addToDroppedFishes()
    {
        fishRb.gravityScale = 1;
        fishRb.constraints &= ~RigidbodyConstraints2D.FreezeRotation;
        droppedFishes.Add(chosenFish);

        chosenFish = null;
        fishRb = null;
    }

    public void movingFish(Vector3 touchPosition)
    {
        chosenFish.transform.position = touchPosition;
        fishRb.gravityScale = 0;
        fishRb.constraints |= RigidbodyConstraints2D.FreezeRotation;
    }

    public void MergeFish(GameObject collision1, GameObject collision2)
    {
        if (collision1.tag == collision2.tag)
        {
            //fishManager.MergeFish(gameObject, collision.gameObject);
            if (collision1.transform.position.y > collision2.transform.position.y)
            {
                Destroy(collision1);
                droppedFishes.Remove(collision1);


                collision2.transform.localScale *= 1.2f;
                SplitTag(collision2.tag, collision2);
                ChangeColor(collision2);
            }
        }

    }

    void SplitTag(string tag, GameObject newFish)
    {
        oldTag = tag;
        string[] parts = oldTag.Split('_'); // Tách chuỗi thành ["fish", "1"]

        if (parts.Length == 2) // Kiểm tra có đúng định dạng "fish_x"
        {
            newTagIndex = int.Parse(parts[1]); // Lấy số
            newTagIndex++; // Tăng số lên 1
            newTag = parts[0] + "_" + newTagIndex; // Ghép lại thành "fish_2"

            newFish.tag = newTag;
        }
    }
    public void ChangeColor(GameObject newFish)
    {
        Color newColor;
        colorIndex = newTagIndex; // Tăng màu theo vòng lặp
        Debug.Log(colorIndex);

        if (ColorUtility.TryParseHtmlString(hexColors[colorIndex-1], out newColor))
        {
            newFish.GetComponent<SpriteRenderer>().color = newColor;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "water")
        {
            Debug.Log("Water");
        }
    }





}
