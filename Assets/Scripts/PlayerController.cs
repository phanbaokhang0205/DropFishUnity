using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
/**
 * 1. Tại sao phải gán giá trị trong start mà không gán trực tiếp ngoài ?
 * 2. Tại sao Init Fish không dùng touchPosition mà phải tạo 1 Vector3 mới là spamwnPosition ?
 * 3. Tại sao điền tham số trong vector3 là 0 rồi mà vẫn gán lại cho spawnPosition.z = 0
 */
public class PlayerController : MonoBehaviour
{
    public GameObject line;

    private FishManager fishManager;
    private Vector3 touchPosition;
    private Touch touch;

    private int heightDrop;
    private int widthDrop;
    private Renderer rendLine;
    private Vector3 targetCenter;

    private void Start()
    {
        fishManager = FishManager.Instance;

        heightDrop = Screen.height / 2;
        widthDrop = Screen.width / 4;

        touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, heightDrop, 10));
        fishManager.CreateFish(touchPosition);

        rendLine = line.GetComponent<Renderer>();
        targetCenter = fishManager.chosenFish.GetComponent<Renderer>().bounds.center;

        setLinePosition();
    }

    void Update()
    {

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, heightDrop, 10));
            
            if (touch.phase == TouchPhase.Began)
            {
                if (fishManager.fishScript.isDropped) return;

                fishManager.PrepareFish(touchPosition);
                setLinePosition();
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (fishManager.fishScript.isDropped) return;

                checkDragPosition(touch);
                fishManager.MoveFish(touchPosition);
                setLinePosition();
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                fishManager.DropFish();
                line.SetActive(false);
            }

        }

        if (fishManager.chosenFish == null)
        {
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, heightDrop, 10));
            fishManager.CreateFish(touchPosition);
            
            setLinePosition();
        }
    }

    void checkDragPosition(Touch touch)
    {
        Vector3 newPosition = fishManager.chosenFish.transform.position;

        if (touch.position.x <= widthDrop)
        {
            newPosition.x = Camera.main.ScreenToWorldPoint(new Vector3(widthDrop, touch.position.y, 10)).x;
            fishManager.chosenFish.transform.position = newPosition;
        }
        else if (touch.position.x >= Screen.width - widthDrop)
        {
            newPosition.x = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - widthDrop, touch.position.y, 10)).x;
            fishManager.chosenFish.transform.position = newPosition;
        }
    }

    void setLinePosition()
    {
        float objectHeight = rendLine.bounds.size.y;
        Vector3 newLinePosition = line.transform.position;
        newLinePosition.y = targetCenter.y - (objectHeight / 2);
        newLinePosition.x = fishManager.chosenFish.transform.position.x;
        line.transform.position = newLinePosition;
        line.SetActive(true);

    }

}
