using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Model : MonoBehaviour
{
    public GameObject LoadingScreen;
    public GameObject Comics1;
    public GameObject Comics2;

    private void Awake()
    {
        StartCoroutine(LoadingSimulator());
    }

    public IEnumerator LoadingSimulator()
    {
         LoadingScreen.SetActive(true);
         if (Comics1 != null & Comics2 != null)
         {
             Comics1.SetActive(true);
             Comics2.SetActive(true);
         }
         
         yield return new WaitForSeconds(3);
         for (int i = 0; i <= 100; i++)
         {
             LoadingScreen.GetComponent<Image>().color = new Color(LoadingScreen.GetComponent<Image>().color.r,
                 LoadingScreen.GetComponent<Image>().color.g, LoadingScreen.GetComponent<Image>().color.b,
                 Mathf.Lerp(1, 0, (float)i / 100));
             Debug.Log(i);
             yield return null;
         }
         LoadingScreen.SetActive(false);

         if (Comics1 != null)
         {
             yield return new WaitForSeconds(3);
             for (int i = 0; i <= 100; i++)
             {
                 Comics1.GetComponent<Image>().color = new Color(Comics1.GetComponent<Image>().color.r,
                     Comics1.GetComponent<Image>().color.g, Comics1.GetComponent<Image>().color.b,
                     Mathf.Lerp(1, 0, (float)i / 100));
                 Debug.Log(i);
                 yield return null;
             }
             Comics1.SetActive(false);
             yield return new WaitForSeconds(3);
             for (int i = 0; i <= 100; i++)
             {
                 Comics2.GetComponent<Image>().color = new Color(Comics2.GetComponent<Image>().color.r,
                     Comics2.GetComponent<Image>().color.g, Comics2.GetComponent<Image>().color.b,
                     Mathf.Lerp(1, 0, (float)i / 100));
                 Debug.Log(i);
                 yield return null;
             }

             
             Comics2.SetActive(false);
         }
    }
}
