using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Carnival;

public class PointCloudSampleBehavior : MonoBehaviour
{
	private MeshFilter _handMeshVertices;
	private ParticleSystem _particleSystem;
	private const float particleSize = 0.003f;
	private Controller _carnivalController;
	private int[] _indices;
	private ParticleSystem.Particle[] _particles;

	// Use this for initialization
	void Start()
	{
		Debug.Log("PointCloudSampleBehavior: Initializing sensor");
		_carnivalController = new Controller();
		_carnivalController.Init();
		_carnivalController.Start();

		_handMeshVertices = GameObject.Find("handMeshVertices").GetComponent<MeshFilter>();
		_particleSystem = GameObject.Find("handParticleSystem").GetComponent<ParticleSystem>();
	}

	// Update is called once per frame
	void Update()
	{
		// Get new fingerTips from current frame
		Frame frame = _carnivalController.Frame();

		/* Point cloud is raw data from sensor that can be used for visualization of hand or whatever is in view, provided as discrete Vector3 points.
         * In this sameple we show two approach in unity to do this:
         * - use mesh with "Points" topology and a vertex color material, each vertex's color is depends on its depth value
         * - use particle system that can be set particle size, cast/recieve shadow etc., even collision
         * Such visualization is much faster than using mesh, which is optimal for mobile devices.
         */

		// Clear points of last frame
		_handMeshVertices.mesh.Clear();
		_particleSystem.Clear();

		// Vector3 positions of mesh vertices
		_handMeshVertices.mesh.vertices = frame.PointCloud;

		_indices = new int[frame.PointCloud.Length];

		for(int i = 0; i < frame.PointCloud.Length; i++)
		{
			// Use depth confidence to filter noise points in raw data, this is very useful if you want distinguish hand
			// from other objects in background
			if(frame.DepthConfidence[i] > 0
                // Show positions on left side with point cloud
			             && frame.PointCloud[i].x < 0)
				_indices[i] = i;
		}

		_handMeshVertices.mesh.SetIndices(_indices, MeshTopology.Points, 0);

		_particles = new ParticleSystem.Particle[frame.PointCloud.Length];

		for(int i = 0; i < frame.PointCloud.Length; i++)
		{
			// Show positions on right side with particles
			if(frame.DepthConfidence[i] > 0 && frame.PointCloud[i].x > 0)
			{
				_particles[i].position = frame.PointCloud[i];
				_particles[i].startSize = particleSize;
			}
		}

		_particleSystem.SetParticles(_particles, _particles.Length);
	}

	void OnDestroy()
	{
		Debug.Log("PointCloudSampleBehavior: OnDestroy");
		_carnivalController.Stop();
	}

	void OnApplicationPause(bool pauseStatus)
	{
		if(_carnivalController == null)
		{
			return;
		}

		Debug.Log("PointCloudSampleBehavior: OnApplicationPause -> " + pauseStatus);

		if(pauseStatus)
		{
			_carnivalController.Stop();
		}
		else
		{
			_carnivalController.Start();
		}
	}
}

