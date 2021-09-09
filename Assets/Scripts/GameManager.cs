using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MessageQueue
{
    public GameSocket socket;

    public GameObject heroPrefab;

    public GameObject mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        type = GameSocket.MessageID.GameSystem;
        socket.AddMessageHandle(this);
    }

    // Update is called once per frame
    void Update()
    {
        while (MsgCount() > 0)
        {
            string data = MsgDequeue();
            Debug.Log(data);
            int idx = data.IndexOf(":");
            int id = int.Parse(data.Substring(0, idx));
            string value = data.Substring(idx + 1);

            switch (id)
            {
                case GameSocket.MessageID.GameSystem_ShakeHand:
                    socket.SendData(GameSocket.MessageID.GameSystem + ":" + GameSocket.MessageID.GameSystem_CreatePlayer+":"+Random.Range(-50,50));
                    break;
                case GameSocket.MessageID.GameSystem_CreatePlayer:

                    Vector3 position = new Vector3(0, 2.0f, float.Parse(value));
                    GameObject heroInstance = Instantiate(heroPrefab, position, Quaternion.identity);

                    heroInstance.GetComponent<SocketMoveRequest>().socket = socket;
                    heroInstance.GetComponent<SocketMoveResponse>().socket = socket;
                    heroInstance.GetComponent<SocketShootRequest>().socket = socket;
                    heroInstance.GetComponent<SocketShootResponse>().socket = socket;

      
                    mainCamera.GetComponent<Transform>().Translate(new Vector3(0,0, position.y - 17));
                    mainCamera.GetComponent<LookAt>().target = heroInstance.transform;
                    mainCamera.GetComponent<Follow>().SetTarget(heroInstance.transform);

                    break;
            }

        }
    }
}
