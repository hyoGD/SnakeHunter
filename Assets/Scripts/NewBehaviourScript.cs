using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
   // public GameObject SnakePrefab;
    public Transform snake;
    public Transform FoodPrefab;
    public Transform snaketailsSegmentPrefab;

    //location
   public Vector2Int GridPosion;    //vi tri ban dau
    private Vector2Int GridDiriction;       //vi tri tang len

    //food
    public Vector2 CurrentFoodLocation;
    public Transform CurrentFood;

    //tails
    private List<Transform> tails = new List<Transform>();
    public List<Vector2Int> tailsPosition = new List<Vector2Int>();

    //movement value
    //public float moveIntervalInSeconds = 0.5f;
    //public float speedUpInseconds = 0.01f;
    //public float minIntervalInSeconds = 0.05f;

    //time keeping
    private float timer;
    private float timerMax = 1f;
    public float speed;

    //Start is called before the first frame update
    private void Awake()
    {
        GridPosion = new Vector2Int(-7, 0); 
        GridDiriction = new Vector2Int(1, 0);
        timer = timerMax;
    }
    void Start()
    {
        SpawnFood();
        //Reset();
       
    }

    // Update is called once per frame
    void Update()
    {
        snake.position = new Vector3 (GridPosion.x, GridPosion.y, snake.position.z);
        if (Input.GetKeyDown(KeyCode.RightArrow) && GridDiriction.x != -1)
        {
            GridDiriction.x = 1;
            GridDiriction.y = 0;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && GridDiriction.x != 1)
        {
            GridDiriction.x = -1;
            GridDiriction.y = 0;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && GridDiriction.y != -1)
        {
            GridDiriction.x = 0;
            GridDiriction.y = 1;
        }
     else   if (Input.GetKeyDown(KeyCode.DownArrow) && GridDiriction.y != 1)
        {
            GridDiriction.x = 0;
            GridDiriction.y = -1;
        }
       timer += Time.deltaTime *speed;
        if(timer>= timerMax)
       //if(Time.time > timerMax)
        {
            timer -= timerMax;

            tailsPosition.Insert(0, GridPosion);    //them đầu rắn vào đầu đoạn của thân rắn
            tailsPosition.RemoveAt(tailsPosition.Count - 1);    
           GridPosion += GridDiriction;
            GridPosion = GetComponent<SizeGame>().CheckPosition(GridPosion);            //di chuyen qua lai theo toa do x,y
            for (int i = 0; i < tails.Count; i++)       //lặp thân rắn theo List
            {
                tails[i].position = new Vector3(tailsPosition[i].x, tailsPosition[i].y, snake.position.z);

            }
            //check if we have hit our own tail
            bool isDead = false;
            foreach(Vector2Int tailSegmentPosition in tailsPosition)
            {
                if (GridPosion.Equals(tailSegmentPosition))
                {
                   
                    isDead = true;
                    break;
                    
                }
               
            }
            //did we hit our own tail?
            if (isDead)
            {
                GetComponent<SizeGame>().audioSource.clip = GetComponent<SizeGame>().EatTails;
                GetComponent<SizeGame>().audioSource.Play();
                Reset();
                StartCoroutine(GetComponent<SizeGame>().EndGame());
                Debug.Log("game Over!");
                // timerMax = Time.time + 2f;
                snake.position = new Vector3(GridPosion.x, GridPosion.y, snake.position.z);
                //Time.timeScale = 0;
            }
            else
            {
                //timerMax = Time.time + timer;
                //snake.position = new Vector3(GridPosion.x, GridPosion.y, snake.position.z);

            }
            if (GridPosion==CurrentFoodLocation)
            {
                Debug.Log("Snake ate food");
                GetComponent<SizeGame>().GetScore();
                AddTail(GridPosion);
                Destroy(CurrentFood.gameObject);
                SpawnFood();
                speed+=0.2f;
                GetComponent<SizeGame>().audioSource.clip = GetComponent<SizeGame>().eatFood;
                GetComponent<SizeGame>().audioSource.Play();
                
            }
            snake.position = new Vector3(GridPosion.x, GridPosion.y);
            snake.eulerAngles = new Vector3(0, 0, GetAngleFromVector(GridDiriction) - 90);  //xoay dau ran
        }
    }
    private float GetAngleFromVector(Vector2Int dir)        //tinh toan xoay dau ran
    {
        float n = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        if (n > 0)
        {
            n += 360;
        }
        return n;
    }
    public void SpawnFood()     //khoi tao ham xuat hien food
    {
        CurrentFoodLocation = new Vector2Int(Random.Range(-8, 8), Random.Range(-5, 5));
        CurrentFood = Instantiate<Transform>(FoodPrefab, new Vector3(CurrentFoodLocation.x, CurrentFoodLocation.y), Quaternion.identity);

        for( int i=0; i< GetComponent<SizeGame>().num; i++)
        {
            if(GetComponent<SizeGame>().num % 10 == 0)
            {
                CurrentFood.localScale = new Vector2(1, 1);         //tang kich thuoc food
                GetComponent<SizeGame>().num += 3;

            }
        }
    }
    public void AddTail(Vector2Int location)        //khoi tao ham tails
    {
        tails.Add(Instantiate<Transform>(snaketailsSegmentPrefab, new Vector3(location.x, location.y), Quaternion.identity));
        tailsPosition.Add(location);
    }


    public void Reset()
    {
        //Time.timeScale = 0;
        GridPosion = new Vector2Int(-7, 0);
        GridDiriction = new Vector2Int(1, 0);
        //destroy any tail pnject
        foreach (Transform tailSegment in tails)
        {
            Destroy(tailSegment.gameObject);
           
        }
        tails.Clear();
        tailsPosition.Clear();
        speed = 3;
       

      
    }

    
}
