using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MySynchronisation : MonoBehaviour, IPunObservable
{
    Rigidbody rb;
    PhotonView photonView;

    Vector3 networkPosition;
    Quaternion networkRotation;

    private float distance;
    private float angle;

    public bool synchronizeVelocity = true;
    public bool synchronizeAngularVelocity = true;
    public bool isTeleportEnabled = true;
    public float teleportIfDistanceIsGreater = 1.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            rb.position = Vector3.MoveTowards(rb.position, networkPosition, distance*(1.0f / PhotonNetwork.SerializationRate));
            rb.rotation = Quaternion.RotateTowards(rb.rotation, networkRotation, angle*(1.0f / PhotonNetwork.SerializationRate));
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //Then, photon view is mine
            //Should send info to other players
            stream.SendNext(rb.position);
            stream.SendNext(rb.rotation);

            if(synchronizeVelocity)
            {
                stream.SendNext(rb.velocity);
            }
            if(synchronizeAngularVelocity)
            {
                stream.SendNext(rb.angularVelocity);
            }
        }
        // Is reading
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
            networkRotation = (Quaternion)stream.ReceiveNext();

            if (isTeleportEnabled)
            {
                if(Vector3.Distance(rb.position, networkPosition) > teleportIfDistanceIsGreater)
                {
                    rb.position = networkPosition;
                }
            }
            if (synchronizeVelocity || synchronizeAngularVelocity)
            {
                float lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));

                if (synchronizeVelocity)
                {
                    rb.velocity = (Vector3) stream.ReceiveNext();

                    networkPosition += rb.velocity * lag;

                    distance = Vector3.Distance(rb.position, networkPosition);

                }

                if (synchronizeAngularVelocity)
                {
                    rb.angularVelocity = (Vector3) stream.ReceiveNext();

                    networkRotation = Quaternion.Euler(rb.angularVelocity * lag) * networkRotation;

                    angle = Quaternion.Angle(rb.rotation, networkRotation);
                }
            }
        }
    }
}
