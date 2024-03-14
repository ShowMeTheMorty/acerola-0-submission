using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DogAnimations : MonoBehaviour
{
    [Serializable]
    public class DogIdleMoment
    {
        public Vector3 position;
        public Vector3 rotation;
        public PlayerDetector detector;
        public PlayerDetector vanishDetector;
        public RuntimeAnimatorController controller;
        public bool fired;

        public delegate void TriggerMomentHandler (DogIdleMoment moment);
        
        public event TriggerMomentHandler OnTriggerMoment;
        public event TriggerMomentHandler OnVanish;

        public void TriggerMoment (PlayerDetector _detector)
        {
            OnTriggerMoment.Invoke(this);
        }

        public void Vanish (PlayerDetector _detector)
        {
            OnVanish.Invoke(this);
        }
    }

    [Serializable]
    public class DogRunMoment
    {
        public PlayerDetector detector;
        public RuntimeAnimatorController controller;
        public bool fired;

        public delegate void TriggerMomentHandler (DogRunMoment moment);

        public event TriggerMomentHandler OnTriggerMoment;

        public void TriggerMoment (PlayerDetector _detector)
        {
            OnTriggerMoment.Invoke(this);
        }
    }

    Animator animator;

    public List<DogIdleMoment> dogIdleMoments;
    public List<DogRunMoment> dogRunMoments;

    void Start()
    {
        animator = GetComponent<Animator>();
        foreach (DogIdleMoment moment in dogIdleMoments)
        {
            moment.detector.OnPlayerEnter += moment.TriggerMoment;
            moment.OnTriggerMoment += IdleEnterHandler;
            if (moment.vanishDetector != null) 
            {
                moment.vanishDetector.OnPlayerEnter += moment.Vanish;
                moment.OnVanish += IdleVanishHandler;
            }
        }
        foreach (DogRunMoment moment in dogRunMoments)
        {
            moment.detector.OnPlayerEnter += moment.TriggerMoment;
            moment.OnTriggerMoment += RunEnterHandler;
        }
    }

    private void RunEnterHandler(DogRunMoment moment)
    {
        if (moment.fired) return;
        AudioManager.singleton.CreateSoundEffect(AudioManager.SFX_TYPE.BARKING, transform.position);
        animator.runtimeAnimatorController = moment.controller;
        transform.position = new Vector3(0f, -200f, 0f);
        moment.fired = true;
        moment.OnTriggerMoment -= RunEnterHandler;
    }

    private void IdleEnterHandler(DogIdleMoment moment)
    {
        if (moment.fired) return;
        AudioManager.singleton.CreateSoundEffect(AudioManager.SFX_TYPE.BARKING, transform.position);
        animator.runtimeAnimatorController = moment.controller;
        transform.position = moment.position;
        transform.rotation = Quaternion.Euler(moment.rotation);
        moment.fired = true;
        moment.OnTriggerMoment -= IdleEnterHandler;
    }

    private void IdleVanishHandler(DogIdleMoment moment)
    {
        animator.runtimeAnimatorController = null;
        transform.position = new Vector3(0f, -200f, 0f);
        moment.OnVanish -= IdleVanishHandler;
    }
}
