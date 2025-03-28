using DG.Tweening;
using DissolveExample;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShadowModePapa : Character
{

    public DissolveChilds dissolve;

    // Start is called before the first frame update

    protected override void Awake()
    {
        base.Awake();
        startPos = transform.position;
    }

    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        CharacterMove();
   
    }

   public override void  ResetCharacter()
    {
        base.ResetCharacter();
        Debug.Log("PapaReset");
        isLight= false;
       
        DOVirtual.DelayedCall(1f, () => dissolve.DIssolvessad(false));
    }

    public override void EnterReplayMode()
    {
        ResetCharacter();
        gameObject.SetActive(true);
    }

    public override void InLight()
    {
        
        Tile tile = TileFinding.GetOneTile(transform.position);
        tile.character = this;
        if (tile.isLight)
        {
            Debug.Log("papaDead");
            InGameManager.Inst.moveBlock = true;
            dissolve.DIssolvessad(true);
            CharacterDead();
            DOVirtual.DelayedCall(0.5f, () => dissolve.DIssolvessad(false));
            return;
        }
    }
}
