using UnityEngine;

[CreateAssetMenu(fileName = "ai_newBrainDef", menuName = "AI Behaviour/New Brain Definition")]
public abstract class Brain : ScriptableObject
{
    public abstract void Think(MobController mob);
}
