
using System;
using System.Collections.Generic;
using MEC;
using NJN.Runtime.Components;
using NJN.Runtime.UI.Panels;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace NJN.Runtime.Controllers
{
    [RequireComponent(typeof(Collider2D))]
    public class TruckController : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _playerLayer;
        
        private SignalBus _signalBus;

        private ParallaxBG _mainParallaxScript;

        private bool _truckIsMaxSpeed;
        private CoroutineHandle _coHandle;
        private CoroutineHandle _managerCoHandle;
        private int _secondsSpentInTransition;
        private bool _transitionStatus;

        private bool _midPointReached;

        private int _transitionLength;

        [BoxGroup("TransitionSettings"),SerializeField,Tooltip("Please note that this is multiplied by the fuel cost")] 
        public int _transitionLengthOnceMaxSpeed;
        [BoxGroup("TransitionSettings"),SerializeField] 
        private float _maxParallaxSpeed;
        [BoxGroup("TransitionSettings"),SerializeField,Tooltip("Lower number means faster acceleration")] 
        private float _parallaxAccelerationFactor;
        private int _fuelCost;
        
        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Start()
        {
            try
            {
                _mainParallaxScript = transform.parent.transform.GetChild(0).gameObject.GetComponent<ParallaxBG>();
            }
            catch(Exception e)
            {
                Debug.Log("Main Parallax Object not found, parallax object must be on top of this parent's hierarchy");
                Debug.Log("Error output: "+e);
            }

            
        }

        private void Update()
        {
            //HAD TO ADD THIS HACK BECAUSE FLOAT SUBTRACTION/ADDITION ADDS RANDOM NUMBERS TO THE END WHY??!?!?!?
            if(_mainParallaxScript._autoScrollIncrement<0.001f)
            {
                _mainParallaxScript._autoScrollIncrement=0;
            }



            //move buildings here
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<DestinationSelectedSignal>(TruckMove);
            //subscribe to new building spawned signal and set new building variable
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<DestinationSelectedSignal>(TruckMove);
        }

        private void OnDestroy()
        {
            Timing.KillCoroutines(_coHandle);
            Timing.KillCoroutines(_managerCoHandle);
        }

        [Button(ButtonSizes.Large)]
        private void TruckMove(DestinationSelectedSignal signal)
        {

            //Debug.Log("TruckController.cs: Called TruckMove() Method with following variables:"+_transitionStatus+_coHandle.IsRunning+_managerCoHandle.IsRunning);

            
            if(!_transitionStatus)
            {
                //Debug.Log("TruckController.cs: Started transition");
                _fuelCost = signal.DestinationData.FuelCost;
                _managerCoHandle = Timing.RunCoroutine(TransitionManager());
                Debug.Log("Started transition with "+_fuelCost+" fuel consumed");
            }

            else if(_transitionStatus && !_coHandle.IsRunning)
            {
                Debug.Log("TruckController.cs: Can't start a transition when truck is already transitioning");
                           
            }
            
            
        }


        private IEnumerator<float> IncDecParallaxSpeed(bool direction)
        {
            if(direction)
            {
                
                while(! (_mainParallaxScript._autoScrollIncrement >= _maxParallaxSpeed) )
                {
                    //Debug.Log("accelerating");
                    _mainParallaxScript._autoScrollIncrement+=0.1f;
                    
                    yield return Timing.WaitForSeconds(_parallaxAccelerationFactor);
                }
                _truckIsMaxSpeed = true;
            }
            else
            {
                _truckIsMaxSpeed = false;
                
                while(! (_mainParallaxScript._autoScrollIncrement < 0.1f))
                {
                    //Debug.Log("decelerating");
                    _mainParallaxScript._autoScrollIncrement-=0.1f;
                    yield return Timing.WaitForSeconds(_parallaxAccelerationFactor);
                }

                TransitionFinished();
                          
            }

            Timing.KillCoroutines(_coHandle);
        }



        private IEnumerator<float> TransitionManager()
        {
            TransitionStart();

            while(_secondsSpentInTransition!=_transitionLength)
            {
                //Debug.Log("waiting for truck to reach max speed");
                if(_truckIsMaxSpeed)
                {
                    if(!(_transitionLength==1))
                    {
                        CheckForHalfTransitionLength();
                    }

                    else
                    {
                        Timing.RunCoroutine(IncDecParallaxSpeed(false));
                    }
                    
                    _secondsSpentInTransition++;
                    
                }
                

                
                yield return Timing.WaitForSeconds(1);

            }


            
            
            
        }

        private void TransitionStart()
        {
            _transitionLength = _transitionLengthOnceMaxSpeed * _fuelCost;
            _transitionStatus = true;
            _secondsSpentInTransition = 0;
            Timing.RunCoroutine(IncDecParallaxSpeed(true));
        }

        private void CheckForHalfTransitionLength()
        {

            int halfDuration;
                //Debug.Log("Checking for halftime...");
                halfDuration =  _transitionLength/2;


            if(_secondsSpentInTransition>=halfDuration)
            {
                //Debug.Log("halftime reached");
                Timing.RunCoroutine(IncDecParallaxSpeed(false));
                _midPointReached=true;
            }
            //Debug.Log(_secondsSpentInTransition+" "+halfDuration);
        }

        private void TransitionFinished()
        {
            _transitionStatus = false;
            _truckIsMaxSpeed = false;
            _mainParallaxScript._autoScrollIncrement=0;
            _secondsSpentInTransition=0;
            _midPointReached=false;
            Timing.KillCoroutines(_coHandle);
            Timing.KillCoroutines(_managerCoHandle);
            //Debug.Log($"Transition ended... ");

        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _playerLayer) != 0)
            {
                _signalBus.Fire(new EnteredTruckSignal());
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & _playerLayer) != 0)
            {
                _signalBus.TryFire(new ExitedTruckSignal());
            }
        }
    }
}