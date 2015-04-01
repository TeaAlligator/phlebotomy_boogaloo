using System.Collections.Generic;
using Assets.Code;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Tube : MonoBehaviour 
{
	public static readonly Dictionary<TubeType, Color> VialCapColors = new Dictionary<TubeType, Color>
	{
	    { TubeType.BloodCultures, new Color((float)253 / (float)255, (float)247 / (float)255, (float)151 / (float)255) },
	    { TubeType.Citrate, new Color((float)112 / (float)255, (float)206 / (float)255, (float)231 / (float)255) },
	    { TubeType.GelSeparator, new Color((float)197 / (float)255, (float)41 / (float)255, (float)42 / (float)255) },
	    { TubeType.Serum, new Color((float)228 / (float)255, (float)25 / (float)255, (float)55 / (float)255) },
	    { TubeType.RapidSerum, new Color((float)234 / (float)255, (float)141 / (float)255, (float)71 / (float)255) },
	    { TubeType.HeparinGelSeparator, new Color((float)158 / (float)255, (float)200 / (float)255, (float)190 / (float)255) },
	    { TubeType.Heparin, new Color((float)1 / (float)255, (float)135 / (float)255, (float)84 / (float)255) },
	    { TubeType.Edta, new Color((float)233 / (float)255, (float)172 / (float)255, (float)206 / (float)255) },
	    { TubeType.EdtaWithGel, new Color((float)255 / (float)255, (float)255 / (float)255, (float)255 / (float)255) },
	    { TubeType.Glucose, new Color((float)184 / (float)255, (float)188 / (float)255, (float)191 / (float)255) }
	};

	public float FillTime;
	public Slider TubeSlider;

	private bool _filling = false;
	private float _endTime = 0;
	private float _currentTime = 0;

	// Use this for initialization
	void Start ()
	{
	}

	public void Initialize(TubeType type)
	{
		TubeSlider = GetComponent<Slider>();
		var lid = transform.FindChild("VialLid");
		var lidImage = lid.GetComponent<Image>();
		var lidColor = VialCapColors[type];
		lidImage.color = new Color(lidColor.r, lidColor.g, lidColor.b);
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
