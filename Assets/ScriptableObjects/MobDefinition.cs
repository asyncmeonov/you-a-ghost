using UnityEngine;

[CreateAssetMenu(fileName = "mob_newMobDef", menuName = "Mob/New Mob Definition")]
public class MobDefinition : ScriptableObject
{
    public Animator animController;
    public float shootFrequency; //how often does it shoot (in sec)
    public float shootDelay; //how long does it take to make the first shot (in sec)
    public float movSpeed; //how fast the mob moves
    public ProjectileDefinition projectile; //definition of his attack
    public Brain mobBrain; //AI movement logic
}
