using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    public bool isPartOfActiveQuest = false;

    [Header("NPC traits")]
    public string NPCName;
    public int NPCId;
    public TextMeshProUGUI nameText;
    public GameObject questArrow;
    public GameObject objWithGameScript;

    public Quest questNPCCanGive;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        nameText.text = NPCName;
        // begin with a quest via the guide
        if (NPCId == 0)
        {
            List<QuestStep> newQuestSteps = new List<QuestStep>
        {
          new QuestStep(
            newType: QuestStepType.Talk,
            newTargetId: 1,
            newTargetName: "The merchant"
            ),
        new QuestStep(
            newType: QuestStepType.Talk,
            newTargetId: 2,
            newTargetName: "The blacksmith"

            ),
        new QuestStep(
            newType: QuestStepType.Talk,
            newTargetId: 3,
            newTargetName: "The The village elder"
            )
        };
            Quest newQuest = new Quest(
            newTitle: "Talk to the people",
            newDesc: "Talk to each of the merchants: The merchant, the blacksmith, and the village elder.",
            newIsActive: false,
            newQuestId: 1,
            newQuestSteps: newQuestSteps,
            newIsOrderImportant: false
            );
            questNPCCanGive = newQuest;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPartOfActiveQuest)
        {
            List<Quest> activeQuests = objWithGameScript.GetComponent<GameScript>().currentQuests;
            foreach (Quest quest in activeQuests)
            {
                foreach (QuestStep questStep in quest.questSteps)
                {
                    if (questStep.targetId == NPCId && !questStep.isCompleted)
                    {
                        // This person is the target of a quest, so enable the arrow.
                        questArrow.SetActive(true);
                        isPartOfActiveQuest = true;
                        return;
                    }
                }
            }
        }
    }
}
