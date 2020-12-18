using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;




public class WeatherSystemController : MonoBehaviour
{
    //Setting state
    private bool isRaining;
    private bool isSnowing;


    [Header("Weather")]
    public States[] WeatherConditions;
    private int WeatherCondtionState;//pattern
    public States state = States.Sunny;
    public GameObject RainPrefab;
    public GameObject SnowPrefab;
    

    [Header("Light")]
    public GameObject GlobalLight;
    [Range(0.001f, 1.0f)]
    public float OverCastAtmosphere;
    [Range(0.001f, 1.0f)]
    public float SunnyAtmophere;
    [Range(0.001f,1.0f)]
    public float RainAtmosphere;
    [Range(0.001f, 1.0f)]
    public float SnowAtmosphere;

    private float AmountOfLight;
    private float CurrentLight;



    [Header("Sounds")]
    public GameObject AudioSource;
    public AudioClip SunnyCalm;
    public AudioClip Raining;
    public AudioClip Breez;
    public AudioClip Snow;
    public enum States
    {
        Sunny =0,
        Overcast=1,
        Rain=2,
        Snow=3
    }
    private AudioClip PlayUpcoming;

    [Header("Transition")]
    public float WeatherTransition; 
    private float WeatherTime;  
    private bool Transition;
    [Range(1, 1000)]
    public int MaxRain;
    private int WishedRain;
    [Range(1, 100)]
    public int MaxSnow;
    public int NumOfParticles;

    public float TimeCycle;  
    private float WeatherDelay; 
    public bool WeatherChange; 


    // Start is called before the first frame update
    void Start()
    {
        NumOfParticles = 0;
        CurrentLight = 1;
        AudioSource.GetComponent<AudioSource>().clip = SunnyCalm;
        AudioSource.GetComponent<AudioSource>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(!Transition)
        {
            WeatherDelay += Time.deltaTime;
            if(WeatherDelay > TimeCycle ) // checking the time difference
            {
                WeatherDelay = 0;
                TransitionState();
                WeatherChange = true;
            }
            if(AudioSource.GetComponent<AudioSource>().volume < 1)
            {
                AudioSource.GetComponent<AudioSource>().volume += 0.001f;
            }
        }

        if(Transition)
        {
            WeatherTime += Time.deltaTime;
            if(WeatherTime > WeatherTransition)
            {
                WeatherTime = 0;
                Transition = false;
                WeatherChange = false;
            }

            if(!isRaining && !isSnowing)
            {
                if(NumOfParticles > 0)
                {
                    NumOfParticles -= 2;
                }
                ParticleSystem particleSystem = RainPrefab.GetComponent<ParticleSystem>();
                var main = particleSystem.main;
                main.maxParticles = NumOfParticles;

                particleSystem = SnowPrefab.GetComponent<ParticleSystem>();
                main = particleSystem.main;
                main.maxParticles = NumOfParticles;
            }
            //Setting the rain
            if(isRaining)
            {
                if(NumOfParticles < WishedRain)
                {
                    NumOfParticles += 1;
                    ParticleSystem particleSystem = RainPrefab.GetComponent<ParticleSystem>();
                    var main = particleSystem.main;
                    main.maxParticles = NumOfParticles;
                }
            }
            // Setting the snow
            if(isSnowing)
            {
                if(NumOfParticles < MaxSnow)
                {
                    NumOfParticles += 1;
                    ParticleSystem particleSystem = SnowPrefab.GetComponent<ParticleSystem>();
                    var main = particleSystem.main;
                    main.maxParticles = NumOfParticles;
                }
            }
            // Setting the lights
            if(AmountOfLight > CurrentLight)
            {
                CurrentLight += 0.001f;
            }
            if(AmountOfLight < CurrentLight)
            {
                CurrentLight -= 0.001f;
            }

            //Setting the fade sound 
            if(AudioSource.GetComponent<AudioSource>().volume >0)
            {
                AudioSource.GetComponent<AudioSource>().volume -= 0.001f;
            }
            else
            {
                AudioSource.GetComponent<AudioSource>().clip = PlayUpcoming;
                AudioSource.GetComponent<AudioSource>().Play();
            }
            GlobalLight.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>().intensity = CurrentLight;
        }

    }

    private void TransitionState()
    {
        //setting the transition between weather.
        Transition = true;
        if(WeatherCondtionState == WeatherConditions.Length -1 )
        {
            WeatherCondtionState = 0;
        }
        else
        {
            WeatherCondtionState += 1;
        }
        state = WeatherConditions[WeatherCondtionState];

        // Setting the different states

        switch(state)
        {
            case States.Sunny:
                AmountOfLight = SunnyAtmophere;
                WishedRain = 0;
                PlayUpcoming = SunnyCalm;
                isRaining = false;
                isSnowing = false;
                break;
            case States.Overcast:
                AmountOfLight = OverCastAtmosphere;
                PlayUpcoming = Breez;
                isSnowing = false;
                isRaining = false;
                break;
            case States.Rain:
                AmountOfLight = RainAtmosphere;
                PlayUpcoming = Raining;
                NumOfParticles = 0;
                WishedRain = MaxRain / 50;
                RainPrefab.SetActive(true);
                SnowPrefab.SetActive(false);
                isSnowing = false;
                isRaining = true;
                break;
            case States.Snow:
                AmountOfLight = SnowAtmosphere;    // A developer can easily add any state.
                PlayUpcoming = Snow;
                NumOfParticles = 0;
                WishedRain = MaxRain;
                SnowPrefab.SetActive(true);
                RainPrefab.SetActive(true);
                isRaining = true;
                isSnowing = true;
                break;
            default:
                break;
        }
    }
}
