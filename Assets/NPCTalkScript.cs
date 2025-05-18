using System.Collections.Generic;
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
    public GameObject objWithGameScript;
    public GameObject questArrow;

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

                        // the arrow is active, so check if it needs to be turned off.
            List<Quest> activeQuests = objWithGameScript.GetComponent<GameScript>().currentQuests;
            int NPCId = npc.GetComponent<NPCScript>().NPCId;
            //bool isPartOfActiveQuest = npc.GetComponent<NPCScript>().isPartOfActiveQuest;

            foreach (Quest quest in activeQuests)
            {
                foreach (QuestStep questStep in quest.questSteps)
                {
                    if (questStep.targetId == NPCId && questStep.isCompleted)
                    {
                        // This person is the target of a quest, so enable the arrow.
                        questArrow.SetActive(false);
                         npc.GetComponent<NPCScript>().isPartOfActiveQuest = false;
                        return;
                    }
                }
            }

            Quest questNPCCanGive = npc.GetComponent<NPCScript>().questNPCCanGive;
            if (questNPCCanGive != null)
            {
                // There is a quest to give, so make it active and show the UI.
                print("quest is not null");
                print(questNPCCanGive.title);

                questUIDocument.GetComponent<QuestUIScript>().OnShowQuest(questNPCCanGive);
            }

            //List<Quest> activeQuests = objWithGameScript.GetComponent<GameScript>().currentQuests;
            foreach (Quest quest in activeQuests)
            {
                int numOfCompletedSteps = 0;
                foreach (QuestStep questStep in quest.questSteps)
                {
                    if (questStep.isCompleted)
                    {
                        numOfCompletedSteps++;
                    }
                    else
                    {
                        // this step is not completed, so check if this NPS satisfies the requirement.
                        //int NPCId = npc.GetComponent<NPCScript>().NPCId;
                        if (questStep.targetId == NPCId && !questStep.isCompleted)
                        {
                            // This person is the target of a quest, so mark the step as completed.
                            questStep.isCompleted = true;
                            numOfCompletedSteps++;
                            print("QUEST STEP COMPLETED");
                            questArrow.SetActive(false);
                            //return;
                        }
                    }

                    print(numOfCompletedSteps);
                    if (numOfCompletedSteps == quest.questSteps.Count)
                    {
                        // This quest is completed. Pop it from the activeQuests.
                        print("QUEST COMPLETED");
                        objWithGameScript.GetComponent<GameScript>().CompleteQuest(quest.questId);
                        //activeQuests.Remove(quest);
                    }
                }
            }

        }
    }


}
