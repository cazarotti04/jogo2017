using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public Estado estado { get; private set; }

    public GameObject obstaculo;
    public float espera;
    public float tempoDestruicao;
    public GameObject menu;
    public GameObject PanelMenu;
    public GameObject gameOverPanel;
    public GameObject pontosPanel;

    public static GameController instancia = null;

    public Text txtPontos;
    public Text MPT;

    private int pontos;

    private void Awake() {
        if (instancia == null) {
            instancia = this;
        }
        else if (instancia != null) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start () {
        estado = Estado.AguardandoComecar;
        PlayerPrefs.SetInt("HighScore", 0);
        menu.SetActive(true);
        PanelMenu.SetActive(true);
        gameOverPanel.SetActive(false);
        pontosPanel.SetActive(false);
	}
	

	IEnumerator GerarObstaculos() {
        while (GameController.instancia.estado == Estado.Jogando) {
            Vector3 pos = new Vector3(4.3f, Random.Range(-3f, 3f), 3.13f);
            GameObject obj = Instantiate(obstaculo, pos, Quaternion.identity) as GameObject;
            Destroy(obj, tempoDestruicao);
            yield return new WaitForSeconds(espera);
        }
    }

    public void PlayerComecou() {
        estado = Estado.Jogando;
        menu.SetActive(false);
        PanelMenu.SetActive(false);
        pontosPanel.SetActive(true);
        atualizarPontos(0);
        StartCoroutine(GerarObstaculos());
    }


    public void PlayerMorreu() {
        estado = Estado.GameOver;
        if (pontos > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", pontos);
            MPT.text = "" + pontos;
        }
        gameOverPanel.SetActive(true);
    }

    private void atualizarPontos(int x) {
        pontos = x;
        txtPontos.text = "" + x;
    }

    public void acrescentarPontos(int x) {
        atualizarPontos(pontos + x);
    }

    public void PlayerVoltou()
    {
        estado = Estado.AguardandoComecar;
        menu.SetActive(true);
        PanelMenu.SetActive(true);
        gameOverPanel.SetActive(false);
        pontosPanel.SetActive(false);
        GameObject.Find("Nave").GetComponent<PlayerControllerFINAL>().recomecar();
    }
		
}
