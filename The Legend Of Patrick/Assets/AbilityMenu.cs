using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMenu : MonoBehaviour
{
    public GameObject abilityMenuUI;
    public static bool GameIsChoose = false;

    Rigidbody2D rb;

    private void Awake()
    {
        abilityMenuUI.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
    }

    public void Choose(bool choose)
    {

        if (choose)
        {
            if (Time.timeScale != 0.1f)
            {
                abilityMenuUI.SetActive(true);
                Time.timeScale = 0.1f;
                Time.fixedDeltaTime = Time.timeScale * .02f;
                GameIsChoose = true;
                

            }
     
        } else{

                abilityMenuUI.SetActive(false);
                Time.timeScale = 1;
                Time.fixedDeltaTime = .02f;
                GameIsChoose = false;

        }


    }


}
