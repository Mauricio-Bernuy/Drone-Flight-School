/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using System.Collections;

namespace Oculus.Interaction
{
    public class InteractorDebugVisual : MonoBehaviour
    {
        [SerializeField, Interface(typeof(IInteractorView))]
        private UnityEngine.Object _interactorView;


        public bool isGrabbedState = false;
        public InteractorState lastState = InteractorState.Normal;
        public InteractorState curState = InteractorState.Normal;
        public InteractorState nextState = InteractorState.Normal;

        public InteractorState curState1 = InteractorState.Normal;
        public InteractorState nextState1 = InteractorState.Normal;
        public float ax;
        
        public GrabInteractor GrabInteractorL;
        public GrabInteractor GrabInteractorR;
        public GrabInteractable target;
        public bool grabbedL = false;
        public bool grabbedR = false;
        
        private IInteractorView InteractorView;

        protected bool _started = false;

        protected virtual void Awake()
        {
            InteractorView = _interactorView as IInteractorView;
        }

        protected virtual void Start()
        {
            this.BeginStart(ref _started);
            this.AssertField(InteractorView, nameof(InteractorView));
            // this.AssertField(_renderer, nameof(_renderer));

            // _material = _renderer.material;
            this.EndStart(ref _started);
        }

        protected virtual void OnEnable()
        {
            if (_started)
            {
                InteractorView.WhenStateChanged += HandleStateChanged;
            }
        }

        protected virtual void OnDisable()
        {
            if (_started)
            {
                InteractorView.WhenStateChanged -= HandleStateChanged;
            }
        }

           
        private void HandleStateChanged(InteractorStateChangeArgs args) 
        {
            lastState = args.PreviousState;
            curState = args.NewState;
        }

        void Update(){
            if (target == GrabInteractorL.SelectedInteractable)
                grabbedL = true;
            else  
                grabbedL = false;
            
            if (target == GrabInteractorR.SelectedInteractable)
                grabbedR = true;
            else 
                grabbedR = false;

            // Check if each hand grabbing~!
            // grabbedL = GrabInteractorL.SelectedInteractable;
            // grabbedR = GrabInteractorR.SelectedInteractable;
        }

        #region Inject

        public void InjectAllInteractorDebugVisual(IInteractorView interactorView, Renderer renderer)
        {
            InjectInteractorView(interactorView);
        }

        public void InjectInteractorView(IInteractorView interactorView)
        {
            _interactorView = interactorView as UnityEngine.Object;
            InteractorView = interactorView;
        }

        #endregion
    }
}
