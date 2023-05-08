using UnityEngine;

[CreateAssetMenu(fileName = "mob_newMobDef", menuName = "Mob/New Mob Definition")]
public class MobDefinition : ScriptableObject
{
    public float shootFrequency; //how often does it shoot (in sec)
    public float shootDelay; //how long does it take to make the first shot (in sec)
    public float movSpeed; //how fast the mob moves
    public int maxHealth; //how many hits does the mob take before they die
    public int reward; //how many score points it awards for being slain
    public ProjectileDefinition projectile; //definition of his attack
    public Brain mobBrain; //AI movement logic
    public float size; //factor for how big the sprite is
    public int cr; //Challenge Rating - a somewhat biased estimate of how difficult this mob is
}
