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
    public GameObject wayfindingArrow;
    public GameObject compassBarImage;

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
            // the arrow is active, so check if it needs to be turned off.
            List<Quest> activeQuests = objWithGameScript.GetComponent<GameScript>().currentQuests;
            int NPCId = npc.GetComponent<NPCScript>().NPCId;

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
                questUIDocument.GetComponent<QuestUIScript>().OnShowQuest(questNPCCanGive);
                print("STARTING QUEST");
                wayfindingArrow.GetComponent<ArrowWayfindingScript>().UpdateTrackingQuestTarget();
                compassBarImage.GetComponent<CompassBarImageScript>().AddCompassMarkers();

            }

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
                            wayfindingArrow.GetComponent<ArrowWayfindingScript>().UpdateTrackingQuestTarget();
                            questArrow.SetActive(false);
                        }
                    }

                    if (numOfCompletedSteps == quest.questSteps.Count)
                    {
                        // This quest is completed. Pop it from the activeQuests.
                        print("QUEST COMPLETED");
                        objWithGameScript.GetComponent<GameScript>().CompleteQuest(quest.questId);
                        wayfindingArrow.GetComponent<ArrowWayfindingScript>().CompleteTracking();
                    }
                }
            }

        }
    }


}
