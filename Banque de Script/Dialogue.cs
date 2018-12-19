using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

[System.Serializable]
public class PlayerAnswersGroup
{
    //les 3 réponses du joueurs correspodant aux 3 boutons
    //(Il serait intéressant de pouvoir rendre le chiffre disponible au designer)
    [Multiline(2)]
    public string[] m_PlayerAnswersGroup = new string[3];
    //la bonne réponse (0-3)
    //(Il serait intéressant de pouvoir rendre le chiffre disponible au designer)
    public int m_RightAnswer = 0;
}

public class Dialogue : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_BeepTalk;
    [SerializeField]
    private AudioSource m_GenieAttack;
    [SerializeField]
    private AudioSource m_PhenixAttack;
    [SerializeField]
    private AudioSource m_Music;
    [SerializeField]
    private Animator m_GenieAnimator;
    [SerializeField]
    private Animator m_BirdAnimator;
    //nombre de question que le designer veux
    [SerializeField]
    private int m_QuestionQuantity = 5;
    //nombre de réponse possible pour le joueur
    [SerializeField]
    private int m_AnswersQuantity = 3;
    //liste des dialogues de l'ennemie que le designer veux 
    [SerializeField]
    private string[] m_EnnemySentences = new string[0];
    //Liste des groupes de réponses du joueurs 
    [SerializeField]
    private PlayerAnswersGroup[] m_PlayerAnswersGroupOfGroup = new PlayerAnswersGroup[0];
    //vitesse de défilement du texte de l'ennemie
    [SerializeField]
    private float m_EnnemyTextSpeed = 0.1f;
    //Text mesh de l'ennemi
    [SerializeField]
    private TextMeshProUGUI m_EnnemyTalk;
    //Boîte de dialogue de l'ennemie (sprite)
    [SerializeField]
    private GameObject m_EnnemyDialogueBox;
    //Boîte de dialogue du joueur (sprite)
    [SerializeField]
    private GameObject m_PlayerDialogueBox;
    //les boutons interactifs sur lesquels les choix de dialogues du joueur sont affichés
    [SerializeField]
    private Button[] m_ButtonsGroup = new Button[3];
    //le bouton pour commencer la partie
    [SerializeField]
    private Button m_StartButton;
    //le bouton pour commencer la partie
    [SerializeField]
    private Button m_PlayAgainButton;
    //bool servant à mettre les parallax en pause(pour faire le menu à même la scène) 
    [HideInInspector]
    public bool m_IsPlay;
    //Vitesse actuel de déroulement du text de l'ennemie(on le met à zéro pour afficher one shot)
    private float m_ActualEnnemyTextSpeed;
    //array de bool qui tourne à true quand une réponse est bonne
    private bool[] m_AnsweredQuestion = new bool[0];
    //nombre de bonne réponse du joueur
    private int m_Score;
    //int random qui détermine le dialogue de l'ennemi à afficher
    private int m_Random;


    //OnValidate (gérer l'inspecteur)
    public void OnValidate()
    {
        //Resize (forcer le size d'un élément de l'inspecteur)
        Array.Resize(ref m_PlayerAnswersGroupOfGroup, m_QuestionQuantity);
        Array.Resize(ref m_EnnemySentences, m_QuestionQuantity);
        Array.Resize(ref m_AnsweredQuestion, m_QuestionQuantity);
        Array.Resize(ref m_ButtonsGroup, m_AnswersQuantity);

        for (int i = 0; i < m_PlayerAnswersGroupOfGroup.Length; i++)
        {
            Array.Resize(ref m_PlayerAnswersGroupOfGroup[i].m_PlayerAnswersGroup, m_AnswersQuantity);
        }
    }

    private void Start()
    {
        m_EnnemyTalk.gameObject.SetActive(false);
        m_PlayAgainButton.gameObject.SetActive(false);
        m_ButtonsGroup[0].gameObject.SetActive(false);
        m_ButtonsGroup[1].gameObject.SetActive(false);
        m_ButtonsGroup[2].gameObject.SetActive(false);
        m_EnnemyDialogueBox.SetActive(false);
        m_PlayerDialogueBox.SetActive(false);

        for (int i = 0; i < m_QuestionQuantity; i++)
        {
            m_AnsweredQuestion[i] = false;
        }

        m_EnnemyTalk.SetText("");
    }

    private void Update()
    {
        //Pour faire afficher le texte de l'ennemie one shot
        if (Input.GetKeyDown(KeyCode.Mouse0))
        { 
            m_ActualEnnemyTextSpeed = 0f;
        }
    }

    //Est appelé par les bouton Start et Play Again 
    public void StartToPlay()
    {
        m_PlayAgainButton.gameObject.SetActive(false);
        m_Score = 0;   
        m_IsPlay = true;
        m_GenieAnimator.SetTrigger("GenieAnimate");
        m_BirdAnimator.SetTrigger("BirdAnimate");
        m_Music.Stop();
        m_Music.Play();
        m_StartButton.gameObject.SetActive(false);
        StartCoroutine(LetsPlay());
    }

    private void RandomEnnemySentence()
    {
        //Choisi une question au hasard parmis le array de dialogue de l'ennemie
            do
            {
                m_Random = UnityEngine.Random.Range(0, m_QuestionQuantity);
            }
            while (m_AnsweredQuestion[m_Random] != false);
        StartCoroutine(ShowSentences());
    }

    public void ValidateAnswers(int answers)
    {
        //Si bonne réponse Score++, jouer animation et son
        if (answers == m_PlayerAnswersGroupOfGroup[m_Random].m_RightAnswer)
        {
            m_Score += 1;
            m_AnsweredQuestion[m_Random] = true;
            
            m_GenieAnimator.SetTrigger("GenieAttack");
            m_BirdAnimator.SetTrigger("BirdGetHit");

            m_GenieAttack.Play();
        }
        //Si mauvaise réponse Score=0, jouer animation et son
        else
        {
            for (int i = 0; i < m_QuestionQuantity; i++)
            {
                m_AnsweredQuestion[i] = false;
            }
            m_Score = 0;

            m_BirdAnimator.SetTrigger("BirdAttack");
            m_GenieAnimator.SetTrigger("GenieGetHit");

            m_PhenixAttack.Play();
        }

        m_EnnemyTalk.gameObject.SetActive(false);
        m_ButtonsGroup[0].gameObject.SetActive(false);
        m_ButtonsGroup[1].gameObject.SetActive(false);
        m_ButtonsGroup[2].gameObject.SetActive(false);
        m_EnnemyDialogueBox.SetActive(false);
        m_PlayerDialogueBox.SetActive(false);

        //Si score != 5 on joue un autre tours
        if(m_Score != 5)
        {
            StopCoroutine(LetsPlay());
            StartCoroutine(LetsPlay());   
        }
        //Si score = 5, victoire
        else
        {
            StopCoroutine(Victory());
            StartCoroutine(Victory());   
        }
    }

    private IEnumerator LetsPlay()
    {
        yield return new WaitForSeconds(3f);     
        m_ActualEnnemyTextSpeed = m_EnnemyTextSpeed;
        m_EnnemyTalk.text = "";
        RandomEnnemySentence();
    }

    private IEnumerator ShowSentences()
    {
        m_EnnemyDialogueBox.SetActive(true);
        m_EnnemyTalk.gameObject.SetActive(true);
        int textSize = 0;

        //Faire afficher lettre par lettre
        while (textSize < m_EnnemySentences[m_Random].ToString().Length)
        {
            //pas de son si c'est un espace
            if (m_EnnemySentences[m_Random].ToString()[textSize] != ' ')
            {
                m_BeepTalk.Play();
            }

            m_EnnemyTalk.text += m_EnnemySentences[m_Random].ToString()[textSize++];          

            //Si le TextSpeed==0 Affiche one shot
            if (m_ActualEnnemyTextSpeed > 0)
            {
                yield return new WaitForSeconds(m_ActualEnnemyTextSpeed);
            }
        }

        yield return new WaitForSeconds(1f);

        m_PlayerDialogueBox.SetActive(true);
        m_ButtonsGroup[0].gameObject.SetActive(true);
        m_ButtonsGroup[1].gameObject.SetActive(true);
        m_ButtonsGroup[2].gameObject.SetActive(true);

        TextMeshProUGUI playerTalk;

        //Affiche les choix de réponse correspondant au dialogue afficher par l'ennemie
        for (int i = 0; i < m_ButtonsGroup.Length; i++)
        {
            playerTalk = m_ButtonsGroup[i].gameObject.GetComponentInChildren<TextMeshProUGUI>();
            playerTalk.SetText(m_PlayerAnswersGroupOfGroup[m_Random].m_PlayerAnswersGroup[i]);
        }

    }

    //Met tout en pause 
    private IEnumerator Victory()
    {
        m_GenieAnimator.SetTrigger("Pause");
        m_BirdAnimator.SetTrigger("Pause");
        yield return new WaitForSeconds(1f);
        m_IsPlay = false;
        m_PlayAgainButton.gameObject.SetActive(true);    
    }

}
