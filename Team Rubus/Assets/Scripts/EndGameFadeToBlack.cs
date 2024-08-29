using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameFadeToBlack : MonoBehaviour
{
    [SerializeField]
    private Canvas HUD;
    private UnityEngine.UI.Image blackScreen;
    // Start is called before the first frame update

    private void Start() {
        blackScreen = GetComponent<UnityEngine.UI.Image>();
        if(blackScreen) {
            blackScreen.enabled = false;
        }
    }
    public void ProcessEnd() {
        if (blackScreen.enabled == false) {
            HUD.gameObject.SetActive(false);
            StartCoroutine(ActivateEnd());
        }
    }
    private IEnumerator ActivateEnd() {
        blackScreen.enabled = true;
        float countdown = 60f;
        float fadeRate = 2/60f;
        blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, 0);
        while (countdown > 0) {
            blackScreen.color = new Color(blackScreen.color.r, blackScreen.color.g, blackScreen.color.b, blackScreen.color.a + fadeRate * Time.deltaTime);
            countdown -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
