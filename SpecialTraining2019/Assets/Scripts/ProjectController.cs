using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectController : ITileCallback
{
    readonly GameObject baseObject;

    int counter;
    readonly float widthRatio;
    readonly float heightRatio;

    public ProjectController(GameObject projectTile)
    {
        baseObject = projectTile;
        counter = 0;

        Vector3 posBotLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 posTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0));
        widthRatio = (posTopRight.x - posBotLeft.x) / 2;
        heightRatio = (posTopRight.y - posBotLeft.y) / 2;
        baseObject.transform.position = posBotLeft;
    }

    public void StartProject()
    {

        if (GameObject.FindGameObjectsWithTag("Projectile").Length >= GameController.BULLET_LIMIT)
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

        init_x *= widthRatio;
        init_y *= heightRatio;

        tile.SetPosition(init_x, init_y);
        tile.SetType(Projectile.TYPE_GUIDED);
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
