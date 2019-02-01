using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectController : ITileCallback
{
    GameObject baseObject;

    int counter;

    public ProjectController(GameObject projectTile)
    {
        baseObject = projectTile;
        counter = 0;
    }

    public void StartProject()
    {

        if (counter >= GameController.BULLET_LIMIT)
        {
            return;
        }

        //float player_x = player.transform.position.x;
        //float player_y = player.transform.position.y;

        float init_x = Random.Range(-1.0f, 1.0f);
        float init_y = Random.Range(0.0f, 1.0f) > 0.5f ? -1.0f : 1.0f;

        if (Random.Range(0.0f, 1.0f) > 0.5f)
        {
            float temp = init_x;
            init_x = init_y;
            init_y = temp;
        }


        Projectile tile = (Projectile)Object.Instantiate(baseObject).GetComponent<Projectile>();

        tile.SetSpeed(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));

        init_x *= 6.55f;
        init_y *= 5.0f;

        Debug.Log("StartProject: " + init_x + "  " + init_y);

        tile.SetPosition(init_x, init_y);
        tile.SetCallback(this);
        counter++;

    }

    public void OnDestroy()
    {
        counter--;
    }

    public int GetBulletNumber()
    {
        return counter;
    }
}

public interface ITileCallback {
    void OnDestroy();
}
