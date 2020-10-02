using Ai;
using UnityEngine;

namespace Quest.Scripts
{
    
    //[CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    public class TestQuestScript : QuestScript
    {
        public override void DefineQuest(QuestManager questManager)
        {
            var stateMachine = questManager.stateMachine;

            var quest1 = new State();
            var quest2 = new State();

            //quest1.AddOnUpdate();
            

            stateMachine.AddExistingState(quest1);
            stateMachine.AddExistingState(quest2);
        }
    }
}