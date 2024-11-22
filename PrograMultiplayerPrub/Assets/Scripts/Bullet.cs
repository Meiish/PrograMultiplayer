using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviourPun
{
    private int ownerId;
    private Rigidbody rb;
    [SerializeField] private float speed;
    private Vector3 direction;
    [SerializeField] private int damage = 20;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetUp(Vector3 direction, int ownerId)
    {
        this.direction = direction;
        this.ownerId = ownerId;
    }

    void Update()
    {
        if (!photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            return;
        }
        rb.velocity = direction.normalized * speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        Player targetPlayer = other.GetComponent<Player>();
        if (targetPlayer != null && targetPlayer.photonView.ViewID != ownerId) // esto evita dañar al dueño de la bala
        {
            targetPlayer.photonView.RPC("TakeDamage", RpcTarget.AllBuffered, damage);
            PhotonNetwork.Destroy(gameObject); // destruye la bala al impactar
        }
    }
}