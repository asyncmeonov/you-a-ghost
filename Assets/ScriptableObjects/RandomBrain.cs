using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ai_newBrainDef", menuName = "AI Behaviour/New Random Brain Definition")]
public class RandomBrain : Brain
{
    //after how many seconds am I going to look for a new direction
    public float timer;

    private float _elapsed = 0f;
    private Vector2 _movDir = Vector2.down;
    public override void Think(MobController mob)
    {
        if (_elapsed >= timer)
        {
            _movDir = PickRandomDirection();
            _elapsed = 0f;
        }
        else
        {
            _elapsed += Time.deltaTime;
        }

        mob.Rb.velocity = _movDir * mob.MobDef.movSpeed;
    }

    private Vector2 PickRandomDirection()
    {
        List<Vector2> directions = new List<Vector2> { Vector2.down, Vector2.right, Vector2.left, Vector2.up };
        return directions[Random.Range(0, directions.Count)];
    }


}
