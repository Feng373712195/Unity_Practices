using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PingSystem {

    private const float MOVE_ENEMY_DOUBLE_CLICK_TIME = .5f;
    private static Ping lastPing;
    private static float lastPingTime;

    private static List<Ping> pingList;

    public static void Initialize()
    {
        pingList = new List<Ping>();
    }

    public static void AddPing(Vector3 position)
    {
        if (lastPing != null && lastPing.GetPingType() == Ping.Type.Move)
        {
            // Test Debug
            // Debug.Log(Time.time + " Time.time");
            // Debug.Log(lastPingTime + "lastPingTime");
            // Debug.Log(lastPingTime + MOVE_ENEMY_DOUBLE_CLICK_TIME + " click time");


            // Last ping was a Move ping
            if(Time.time < lastPingTime + MOVE_ENEMY_DOUBLE_CLICK_TIME)
            {
                DestroyPing(lastPing);
                // Pings in quick succession
                AddPing(new Ping(Ping.Type.Enemy, position));
            }
            else
            {
                AddPing(new Ping(Ping.Type.Move, position));
            }
        }
        else
        {
            AddPing(new Ping(Ping.Type.Move, position));
        }
    }

    public static void AddPing(Ping ping)
    {
        pingList.Add(ping);
        Transform pingTransform = UnityEngine.Object.Instantiate(GameAssetsSystem.i.pfPingWorld, ping.GetPosition() , Quaternion.identity);
        switch (ping.GetPingType())
        {
            default:
            case Ping.Type.Move:
                break;
            case Ping.Type.Enemy:
                pingTransform.GetComponent<SpriteRenderer>().sprite = GameAssetsSystem.i.pingEnemySprite;
                pingTransform.Find("distanceText").GetComponent<TextMeshPro>().color = GameAssetsSystem.i.pingEnemyColor;
                break;
        }

        ping.OnDestroyed += delegate (object sender, EventArgs e){
            UnityEngine.Object.Destroy(pingTransform.gameObject);
	    };

        PingWindow.AddPing(ping);

        lastPing = ping;
        lastPingTime = Time.time;
    }

    public static void DestroyPing(Ping ping)
    {
        ping.DestroySelf();
        pingList.Remove(ping);
    }

    public static void Update() {
        for(int i = 0; i < pingList.Count; i++)
        {
            Ping ping = pingList[i];
            if(Time.time > ping.GetDestroyTime())
            {
                // Time to destroy this ping
                DestroyPing(ping);
                i--;
            }
        }
    }

    /*
     * Handles a single Ping
     */
    public class Ping
    {
        public enum Type
        {
            Move,
            Enemy,
        }

        public event EventHandler OnDestroyed;

        private Type type;
        private Vector3 position;
        private bool isDestroyed;
        private float destroyTime;

        public Ping(Type type, Vector3 position)
        {
            this.type = type;
            this.position = position;
            isDestroyed = false;
            destroyTime = Time.time + 10f;
        }

        public Type GetPingType()
        {
            return type;
        }

        public Vector3 GetPosition()
        {
            return position;
        }

        public void DestroySelf()
        {
            isDestroyed = true;
            if (OnDestroyed != null) OnDestroyed(this, EventArgs.Empty);
        }

        public bool IsDestroyed()
        {
            return isDestroyed;
        }

        public float GetDestroyTime()
        {
            return destroyTime;
        }
    }

}
