using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public List<Quest> currentQuests = new List<Quest>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void CompleteQuest(int questId)
    {
        print("REMOVING QUEST");
        //Quest questToRemove = currentQuests.Where<Quest>(questI)
        // currentQuests.Remove(questId);
        currentQuests = currentQuests.Where(quest => quest.questId != questId).ToList();

    }

}
