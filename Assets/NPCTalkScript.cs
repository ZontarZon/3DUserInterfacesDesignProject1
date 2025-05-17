using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UIElements;

public class NPCTalkScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [Header("NPC Components")]
    public GameObject cone;
    public Canvas canvasWithName;
    public UIDocument questUIDocument;
    public GameObject npc;

    private bool playerIsWithinTalkRange = false;

    void OnTriggerEnter()
    {
        cone.GetComponent<MeshRenderer>().enabled = true;
        canvasWithName.GetComponent<Canvas>().enabled = true;
        playerIsWithinTalkRange = true;

    }

    void OnTriggerExit()
    {
        cone.GetComponent<MeshRenderer>().enabled = false;
        canvasWithName.GetComponent<Canvas>().enabled = false;
        playerIsWithinTalkRange = false;
    }

        void LateUpdate()
    {
        if (Input.GetKeyUp(KeyCode.E) && playerIsWithinTalkRange)
        {
            print("PLAYER TALK ACTION");

            Quest quest = npc.GetComponent<NPCScript>().questNPCCanGive;
            if (quest != null)
            {
                // There is a quest to give, so make it active and show the UI.
                print("quest is not null");
                print(quest.title);

                questUIDocument.GetComponent<QuestUIScript>().OnShowQuest(quest);
            }

        

        }
    }


}
