using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SceneHandler : Singleton<SceneHandler>
{
    //------ Scenes
    public void LoadSceneByID(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }


    //------ Navmesh
    public Vector3 GetRandomPointInNavMesh(Transform center, int rangeX, int rangeY)
    {
        Vector2 offset = Random.insideUnitCircle.normalized * Random.Range(rangeX, rangeY); //get a random direction and distance within the spawn ring
        Vector3 randomPos = center.position + new Vector3(offset.x, 0f, offset.y); //convert the offset into a world position around the player

        //find the nearest NavMesh point within 2 units
        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            return hit.position;

        return center.position + new Vector3(1f, 0f, 1f); //fallback
    }
}
