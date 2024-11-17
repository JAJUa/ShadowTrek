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

    private void Awake()
    {
        startPos = transform.position;
    }

    public override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    void Update()
    {
        CharacterMove();
   
    }

   public override void  ResetPos()
    {
        base.ResetPos();
        Debug.Log("PapaReset");
        isLight= false;
       
        DOVirtual.DelayedCall(1f, () => dissolve.DIssolvessad(false));
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
            DOVirtual.DelayedCall(0,()=> CharacterDead());
            DOVirtual.DelayedCall(0.5f, () => dissolve.DIssolvessad(false));
            return;
        }
    }
}
