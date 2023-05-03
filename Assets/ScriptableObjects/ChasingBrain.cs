using UnityEngine;

[CreateAssetMenu(fileName = "ai_newBrainDef", menuName = "AI Behaviour/New Chasing Brain Definition")]
public class ChasingBrain : Brain
{
    public GameObject _target;

    public override void Think(MobController mob)
    {
        Vector3 moveDir = _target.transform.position - mob.transform.position;
        mob.Rb.velocity = new Vector2(moveDir.x, moveDir.y).normalized * mob.MobDef.movSpeed;
    }
}
