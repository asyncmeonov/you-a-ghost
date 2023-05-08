using UnityEngine;

[CreateAssetMenu(fileName = "ai_newBrainDef", menuName = "AI Behaviour/New Roaming Brain Definition")]
public class RoamingBrain : Brain
{
    private GameObject[] roamingPoints;
    private Vector3 _targetPosition;


    private void OnEnable()
    {
        roamingPoints = GameObject.FindGameObjectsWithTag("roaming_point");
        _targetPosition = PickRandomRoamingPoint();
    }
    public override void Think(MobController mob)
    {
        if (Vector3.Distance(_targetPosition, mob.transform.position) <= 0.1f)
        {
            _targetPosition = PickRandomRoamingPoint();
        }
        Vector3 moveDir = _targetPosition - mob.transform.position;
        mob.Rb.velocity = new Vector2(moveDir.x, moveDir.y).normalized * mob.MobDef.movSpeed;
    }

    private Vector3 PickRandomRoamingPoint()
    {
        //OnEnable loads the script before roamingPositions can be populated and when changing scenes results in an IOE
        //This is a temporary bodge for the competition
        //TODO: investigate how Scriptable Objects relate to the Mono execution stack
        if(roamingPoints.Length > 0)
        {
            return roamingPoints[Random.Range(0, roamingPoints.Length)].transform.position;
        }
        else
        {
            return Vector3.zero;
        }
}


}
