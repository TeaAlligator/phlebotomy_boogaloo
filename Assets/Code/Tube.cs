using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tube : MonoBehaviour 
{
	public float FillTime;
	public Slider TubeSlider;

	private bool _filling = false;
	private float _endTime = 0;
	private float _currentTime = 0;

	// Use this for initialization
	void Start ()
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (_filling)
		{
			TubeSlider.value += Time.smoothDeltaTime;
			_currentTime = Time.timeSinceLevelLoad;

			if (_currentTime > _endTime)
				_filling = false;
		}
	}

	public void StartDraw()
	{
		TubeSlider = GetComponent<Slider>();
		_currentTime = Time.timeSinceLevelLoad;
		_endTime = _currentTime + 5; // tube fill time
		TubeSlider.maxValue = FillTime;
		_filling = true;
	}

	void OnAwake()
	{
		StartDraw();
	}
}
