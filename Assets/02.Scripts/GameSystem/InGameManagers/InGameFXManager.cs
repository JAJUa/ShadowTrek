using System;
using DG.Tweening;
using UnityEngine;
using Collections.Shaders.CircleTransition;

public class InGameFXManager : MonoBehaviour
{
    public static InGameFXManager Inst;

    [Header("-- Shadow Camera Particle --")]

    public float SphereSize = 230;
    public GameObject EndObjectBlackScreen, ChangerBlackScreen;
   // [ReadOnly] public Transform PlayerTransform;
    [SerializeField] Transform ShadowObjectTransform;

    bool On;
    bool Player;

    [Space(10)] [Header("-- Particles --")]

    public GameObject[] tileClickParticle = new GameObject[3];
    public ParticleSystem[] tileclickSystem = new ParticleSystem[2];

    [Space(10)] [Header("-- Screen Transition --")]

    public CircleTransition circletransition;

    private void Awake()
    {
        Inst = this;

      

        if (tileClickParticle[0] != null)
        {
            tileClickParticle[0] = Instantiate(tileClickParticle[0], new Vector3(0, -10, 0), tileClickParticle[0].transform.rotation);
            tileClickParticle[1] = Instantiate(tileClickParticle[1], new Vector3(0, -10, 0), tileClickParticle[1].transform.rotation);
            tileClickParticle[2] = Instantiate(tileClickParticle[2], new Vector3(0, -10, 0), tileClickParticle[2].transform.rotation);

            tileclickSystem[0] = tileClickParticle[0].GetComponent<ParticleSystem>();
            tileclickSystem[1] = tileClickParticle[1].GetComponent<ParticleSystem>();
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (On) { MoveToObject(); }
    }

    void MoveToObject()
    {
        if (Player)
          //  EndObjectBlackScreen.transform.position = PlayerTransform.position;
        //else
            EndObjectBlackScreen.transform.position = ShadowObjectTransform.position;
    }

    public void BlackScreenON()
    {
        ChangerBlackScreen.transform.DOScale(SphereSize, 1f).SetEase(Ease.OutSine);
    }

    public void BlackScreenOFF(bool player)
    {
        Player = player;
        On = true;
        DOTween.Kill(ChangerBlackScreen.transform);
        EndObjectBlackScreen.transform.localScale = new Vector3(SphereSize, SphereSize, SphereSize);
        ChangerBlackScreen.transform.localScale = Vector3.zero;
        EndObjectBlackScreen.transform.DOScale(0, 0.5f).SetEase(Ease.OutSine).OnComplete(() =>
        On = false);
    }

    public void TileClickParticle(Vector3 Tr)
    {
        tileClickParticle[0].transform.position = Tr;
        tileClickParticle[1].transform.position = Tr;
        tileClickParticle[2].transform.position = Tr;

        tileclickSystem[0].Play();
        tileclickSystem[1].Play();
    }

    public void CircleTransition(float duration, float beginRadius, float endRadius)
    {
        circletransition.Transition(duration, beginRadius, endRadius);
    }
}
