using UnityEngine;

[CreateAssetMenu(fileName = "ai_newBrainDef", menuName = "AI Behaviour/New Chasing Brain Definition")]
public class ChasingBrain : Brain
{
    public override void Think(MobController mob)
    {
        Vector3 _playerPosition = GameObject.FindWithTag("player").transform.position;
        Vector3 moveDir = _playerPosition - mob.transform.position;
        mob.Rb.velocity = new Vector2(moveDir.x, moveDir.y).normalized * mob.MobDef.movSpeed;
    }
}
