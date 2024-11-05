using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

public class MoveTutorial : Tutorial
{
    [SerializeField] Transform triggerObj;
    [SerializeField] bool haveCutScene;
    bool tutoFnish;
    [ShowIf("haveCutScene")]
    [SerializeField] CutSceneManager cutSceneManager;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(haveCutScene + "움직임");
    }

    // Update is called once per frame
    void Update()
    {
        if (!tutoFnish && triggerObj != null)
        {
            if (Vector3.Distance(player.position, triggerObj.position) < 2f)
            {
                tutoFnish = true;
                player.position = triggerObj.position;
                if (haveCutScene)
                {
                    InGameManager.Inst.StopMoving();
                    cutSceneManager.StartCutScene();
                }
                else
                {

                    Debug.Log("컷씬 없음");
                    TutorialManager.Inst.FinshTutorial();
                }
            }
        }
    }
}
