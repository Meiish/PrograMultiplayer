using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviourPun
{
    private static GameObject localInstance;

    [SerializeField] private TextMeshPro playerNameText;

    private Rigidbody rb;
    [SerializeField] private float speed;

    public static GameObject LocalInstance { get { return localInstance; } }

    [SerializeField] private GameObject bulletPrefab;

    private void Awake()
    {
        if (photonView.IsMine)
        {
            playerNameText.text = GameData.playerName;
            photonView.RPC("SetName", RpcTarget.AllBuffered, GameData.playerName); //rpc ejecuta el metodo en todos los otro clientes
            localInstance = gameObject;

            // Sincroniza el color con un RPC
            photonView.RPC("SetColor", RpcTarget.AllBuffered, GameData.playerColor.r, GameData.playerColor.g, GameData.playerColor.b);
        }
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody>();
    }


    [PunRPC]
    private void SetName(string playerName)
    {
        playerNameText.text = playerName;
    }

    [PunRPC]
    private void SetColor(float r, float g, float b)
    {
        // Aplica el color al renderer del jugador
        GetComponent<Renderer>().material.color = new Color(r, g, b);
    }
    
    void Update()
    {
        if (!photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            return;
        }
        Move();
        Shoot();
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector3(horizontal * speed, rb.velocity.y, vertical * speed);

        if (horizontal != 0 || vertical != 0)
        {
            transform.forward = new Vector3(horizontal, 0, vertical);
        }
    }

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj = PhotonNetwork.Instantiate(bulletPrefab.name, transform.position, Quaternion.identity);
            obj.GetComponent<Bullet>().SetUp(transform.forward, photonView.ViewID);
        }

    }
}