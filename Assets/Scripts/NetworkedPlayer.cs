using UnityEngine;
using System.Collections;

public class NetworkedPlayer : Photon.MonoBehaviour
{
	public GameObject avatar;

	public Transform playerGlobal;
	public Transform playerLocal;

	void Start ()
	{
		Debug.Log("i'm instantiated");

		if (photonView.isMine)
		{
			Debug.Log("player is mine");

			playerGlobal = GameObject.Find("CardboardMain").transform;
			playerLocal = playerGlobal.Find("Head");

			this.transform.SetParent(playerLocal);
			this.transform.localPosition = Vector3.zero;


			//this is useful to test whether one's own avatar is in the scene (ease confusion)
			 avatar.SetActive(false);
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(playerGlobal.position);
			stream.SendNext(playerGlobal.rotation);
			stream.SendNext(playerLocal.localPosition);
			stream.SendNext(playerLocal.localRotation);
		}
		else
		{
			this.transform.position = (Vector3)stream.ReceiveNext();
			this.transform.rotation = (Quaternion)stream.ReceiveNext();
			avatar.transform.localPosition = (Vector3)stream.ReceiveNext();
			avatar.transform.localRotation = (Quaternion)stream.ReceiveNext();
		}
	}
}