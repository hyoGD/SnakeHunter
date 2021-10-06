using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SizeGame : MonoBehaviour
{
    // Start is called before the first frame update
    public  int num = 0;
    public Text ScoreText;
    public Text endScore;
    public Text highScore;
    //Object Gameover
    public GameObject Panel;
    public GameObject score;
    // Pasue
    public GameObject PanelPause;
    public GameObject ButtonPause;
    public GameObject buttonOn;
    public GameObject buttonOff;
    public GameObject on;
    public GameObject off;

    //Audio
    public AudioClip eatFood;
    public AudioClip EatTails;
    public AudioClip gameOver;
    public AudioSource audioSource;
    private void Start()
    {
       highScore.text= PlayerPrefs.GetInt("HIGHSCORE", 0).ToString();
        score.SetActive(true);
        Panel.SetActive(false);
        ButtonPause.SetActive(true);

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        
    }

    public Vector2Int CheckPosition(Vector2Int GridPosion)      //di chuyen qua lai theo toa do x,y
    {
        if (GridPosion.x < -8)
        {
            GridPosion.x = 8;
        }
        if (GridPosion.x > 8)
        {
            GridPosion.x = -8;
        }
        if (GridPosion.y < -5)
        {
            GridPosion.y = 5;
        }
        if (GridPosion.y > 5)
        {
            GridPosion.y = -5;
        }

        return GridPosion;
    }
    public void GetScore()
    {
        Debug.Log("Cong diem!!!!!!!");
        num++;
        ScoreText.text = "Score: " + num.ToString();
        // endScore.text = ScoreText.text;
        if (num > PlayerPrefs.GetInt("HIGHSCORE", 0))
        {
            PlayerPrefs.SetInt("HIGHSCORE", num);

        }
    }
    public IEnumerator EndGame()
    {
        yield return new WaitForSeconds(1.2f);
        // Time.timeScale = 0;
        score.SetActive(false);
        Panel.SetActive(true);
        ButtonPause.SetActive(false);
        endScore.text = ScoreText.text;
        highScore.text = "HIGH SCORE \n" + PlayerPrefs.GetInt("HIGHSCORE", num);  
        Destroy(GetComponent<NewBehaviourScript>().CurrentFood.gameObject);         //pha huy food hien tai

        audioSource.clip = gameOver;        
        audioSource.Play();
       // Time.timeScale = 0;
    }
   
    public void PLAYY()
    {
        GetComponent<NewBehaviourScript>().Reset();
       Time.timeScale = 1;
        score.SetActive(true);
        Panel.SetActive(false);
        ButtonPause.SetActive(true);
        num = 0;
        ScoreText.text = "Score: 0";
        GetComponent<NewBehaviourScript>().SpawnFood(); //goi ham xuat  hien food
    }

    public void Pause()
    {
        //score.SetActive(false);
        ButtonPause.SetActive(false);
        PanelPause.SetActive(true);
        Time.timeScale = 0;
    }
    public void Return()
    {
       // score.SetActive(true);
        ButtonPause.SetActive(true);
        PanelPause.SetActive(false);
        Time.timeScale = 1;
    }
    public void MusicOff()
    {
        audioSource.volume = 0;
        on.SetActive(false);
        buttonOn.SetActive(false);
        off.SetActive(true);
        buttonOff.SetActive(true);

    }
    public void MusicOnn()
    {
        audioSource.volume = 0.5f;
        off.SetActive(false);
        buttonOff.SetActive(false);
        on.SetActive(true);
        buttonOn.SetActive(true);
    }

    
  
}
