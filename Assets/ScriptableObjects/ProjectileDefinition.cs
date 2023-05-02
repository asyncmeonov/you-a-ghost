using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "projectile_newProjectileDef", menuName = "Projectile/New Projectile Definition")]
public class ProjectileDefinition : ScriptableObject
{
    //how big is the projectile (compared to the player's bullet
    //size == 1 -- as big as player's
    //size == 2 -- twice as big as player's
    //size == 0.5 -- twice as small as player's
    public float size; 

    //how many projectiles at once does it shoot. Consider adding a new field for different spread patterns
    public float count;

    //how fast does the projectile travel
    public float speed;
}
