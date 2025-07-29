using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;


//https://gamedevbeginner.com/how-to-load-a-new-scene-in-unity-with-a-loading-screen/

public class LoadSceneOnEnter : MonoBehaviour
{
 

    public string sampleScene;
    public string fossilHill;
    public GameObject loadingScreen;
   // AsyncOperation loadingOperation;
   // float loadProgress = loadingOperation.progress;
  //  float progressBar;

    void Start() {
          //loadingOperation = SceneManager.LoadSceneAsync(SampleScene);
    }

    void Update() {
        //progressBar = Mathf.Clamp01(loadingOperation.progress / 0.9f);
    }
    
    void OnTriggerEnter(Collider other) {
        if(other.tag == "TimeTravel") {

            StartCoroutine(ExampleCoroutine());
            SceneManager.LoadScene(sampleScene);
           
        }

        if (other.tag == "MoarFossils") {
            print("loading more fossils");
           SceneManager.LoadScene(fossilHill);

        }
    }

    IEnumerator ExampleCoroutine() {
        print("Let's go back in time bitch");
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(5);
        loadingScreen.SetActive(false);
        print("we did it");
    }
}
