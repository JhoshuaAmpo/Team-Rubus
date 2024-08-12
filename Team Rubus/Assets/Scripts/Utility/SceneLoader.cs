using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Animator backdropFade, titleFade, buttonsAnim;
    public Button play, quit;
    public void LoadScene(int buildIndex)
    {
        StartCoroutine(LoadScene());
    }
    public void Quit() {
        Application.Quit();
    }

    public IEnumerator LoadScene()
    {
        play.interactable = false; quit.interactable = false;
        titleFade.SetTrigger("TitleFadeOut");
        backdropFade.SetTrigger("BDFadeIn");
        buttonsAnim.SetTrigger("ButtonsMoveOut");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(1);
    }
}