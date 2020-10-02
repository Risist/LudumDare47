using UnityEngine;

namespace Quest
{
    [CreateAssetMenu(fileName = "QuestScript", menuName = "Ris/Quest/Script", order = 0)]
    public abstract class QuestScript : ScriptableObject
    {
        public abstract void DefineQuest(QuestManager questManager);
    }
}