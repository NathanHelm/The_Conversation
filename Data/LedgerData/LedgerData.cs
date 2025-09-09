using UnityEngine;
using System.Collections;
using Data;
using System.Collections.Generic;
using UI;
using System;
using Codice.LogWrapper;
using System.Linq;
using ObserverAction;
using System.Data.Common;

namespace Data
{
    public class LedgerData : StaticInstanceData<LedgerData>, IExecution, IObserver<ObserverAction.LedgerActions>, IObserver<ObserverAction.LedgerMovementActions>
    {
        public List<LedgerImage> ledgerImages { get; set; } = new List<LedgerImage>();
        public bool isLedgerCreated { get; set; } = false;

        public bool isLeft { get; set; } = false;

        public float flipPageTime { get; set; } = 1f;

        public int pageIndex { get; set; } = 0;

        public int ledgerLength { get; set; } = 0;

        public Animator leftHandAnim { get; set; }
        public Animator rightHandAnim { get; set; }

        public Material drawingPageMaterial { get; set; }

        public GameObject leftHandObj { get; set; }
        public GameObject rightHandObj { get; set; }

        public PencilSketchPostEffect pencilSketchPostEffect { get; set; }
        public PencilSketchPostEffect pencilSketchPostEffectScreenShot { get; set; }
        public Action<LedgerManager> writeActionLedgerManager = lm => {
            //TODO change to hand state, not function 
            ImageUIAnimations.INSTANCE.DrawImageOnCurrentPage();
            GameEventManager.INSTANCE.OnEvent(typeof(WriteHandState));
        };
        
       
        public Action<LedgerManager> pointActionLedgerManager = lm => {
            if (!LedgerMovement.INSTANCE.IsFlipPageCoroutineRunning())
            {
                GameEventManager.INSTANCE.OnEvent(typeof(PointHandState));
            }
        };
        public Action<ImageUIAnimations> disableleftHandPage = pa => {
            Debug.LogError("disable left hand page has not been set :/ ");
        };
        public Action<LedgerManager> runOpenLedgerAndCutscene = lm =>
        {
            CutsceneManager.INSTANCE?.LedgerDialog();
            GameEventManager.INSTANCE.OnEvent(typeof(OpenLedgerState));
        }; //check trigger action manager

        public override void m_OnEnable()
        {

            disableleftHandPage = pa =>
            {
                HandAnimations.INSTANCE.DisableLeftHandPage();
                ImageUIAnimations.onAfterEraseImage.RemoveAction(disableleftHandPage);
                HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.LLastFlip, 1);
            };
            var temp = FindObjectsOfType<PencilSketchPostEffect>();
            if (temp.Length >= 2)
            {
                pencilSketchPostEffect = FindObjectsOfType<PencilSketchPostEffect>()[1]?.GetComponent<PencilSketchPostEffect>();
            }
            if (temp.Length > 0)
            {
                pencilSketchPostEffectScreenShot = FindObjectsOfType<PencilSketchPostEffect>()[0]?.GetComponent<PencilSketchPostEffect>();
            }
            if (temp.Length == 0)
            {
                Debug.LogError("PENCIL SKETCH NOT FOUND!");
            }
            DrawingManager.onStartDrawingManager.AddAction(dM =>
            {
                if (pencilSketchPostEffect == null || pencilSketchPostEffectScreenShot == null)
                {
                    Debug.LogError("PENCIL SKETCH NOT FOUND!");
                    return;
                }
                dM.pencilSketchPostEffectScreenShot = pencilSketchPostEffectScreenShot;
                dM.mat = pencilSketchPostEffect.compositeMat;

            });


  
            LedgerImageManager.onStartLedgerData.AddAction((LedgerImageManager lm) => { lm.ledgerImages = ledgerImages; lm.MaxLedgerImageLength = 15; /*change this to a proper variable*/ });

            UI.LedgerUIManager.onFlipPage.AddAction((LedgerUIManager luim) => { luim.flipPageSpeed = this.flipPageTime; luim.isLeft = this.isLeft; });

            LedgerUIManager.onBorderCheck.AddAction((LedgerUIManager ledgerManagerUI) => { ledgerManagerUI.isLeft = this.isLeft; });

            ImageUIAnimations.onDrawImageOnCurrentPage.AddAction(lia =>
            {
                lia.currentPageOverlayImage = LedgerUIManager.INSTANCE.GetImageObjectRenderer(pageIndex);
            });

            LedgerMovement.onEnableHand.AddAction((LedgerMovement ledgerAnimationsManager) =>
            {
                /*
                ledgerAnimationsManager.flipPageAnimationTime = this.flipPageTime;
                ledgerAnimationsManager.isLeft = this.isLeft;
                */

            });

            LedgerMovement.onPointHand.AddAction((LedgerMovement ledgerAnimationsManager) =>
            {
                ledgerAnimationsManager.isLeft = this.isLeft;
                ledgerAnimationsManager.pageObjectIndex = this.pageIndex;
                ledgerAnimationsManager.flipPageAnimationTime = flipPageTime;
                HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.PointAnim, flipPageTime * 0.5f);
            });

            LedgerMovement.onWritingHand.AddAction((LedgerMovement lm) =>
            {
                lm.flipPageAnimationTime = flipPageTime;
                lm.pageObjectIndex = this.pageIndex;
                HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.WriteAnim, flipPageTime);

            });

            LedgerMovement.onAfterWritingHand.AddAction((LedgerMovement lm) =>
            {
                GameEventManager.INSTANCE.OnEvent(typeof(OpenLedgerState)); //after writing animation is done, we return to new states
                GameEventManager.INSTANCE.OnEvent(typeof(PointHandState));
            }
            );

            LedgerManager.INSTANCE.subject.AddObserver(this);
            LedgerMovement.INSTANCE.subject.AddObserver(this);
            base.m_OnEnable();
        }

        public void OnNotify(LedgerActions data)
        {
            if (data == LedgerActions.activeLedger)
            {
                LedgerManager.INSTANCE.isLedgerCreated = isLedgerCreated;
            }
            else if (data == LedgerActions.createLedger)
            {
                ledgerImages = LedgerImageManager.INSTANCE.GetLedgerImageList();
                LedgerManager.INSTANCE.ledgerImages = ledgerImages;
            }
            else if (data == LedgerActions.movePageLeft)
            {
                pointActionLedgerManager(LedgerManager.INSTANCE);
            }
            else if (data == LedgerActions.movePageRight)
            {
                pointActionLedgerManager(LedgerManager.INSTANCE);
            }
            else if (data == LedgerActions.onSetTextureToPageImage)
            {
                LedgerManager.INSTANCE.ledgerImages = ledgerImages;
            }
            else if (data == LedgerActions.onAfterMovePageFurthestLeft)
            {
                LedgerManager.INSTANCE.MovePagesToFurthestRight();
               // LedgerMovement.INSTANCE.subject.AddObserver(this);
                ActionController.AFTERPAGEFLIP_LEDGER += ActionController.INSTANCE.afterFlipBehaviour.writeActionLedgerMovement;
            }
            else if (data == LedgerActions.onSelectPage)
            {
                //NOTE: I've made is so that 'onselectpage', is instead used as a static action on ActionController
            }
           

            
            
            //throw new NotImplementedException();
        }

        public void OnNotify(ObserverAction.LedgerMovementActions data)
        {
            if (data == LedgerMovementActions.onEnableHand)
            {
                LedgerMovement.INSTANCE.flipPageAnimationTime = flipPageTime;
                LedgerMovement.INSTANCE.isLeft = isLeft;
            }
            else if (data == LedgerMovementActions.onMoveHand)
            {
                LedgerMovement.INSTANCE.isLeft = isLeft;
                LedgerMovement.INSTANCE.pageObjectIndex = pageIndex;
                LedgerMovement.INSTANCE.flipPageAnimationTime = flipPageTime;

                if (flipPageTime >= 1)
                {
                    HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.FlipAnim, flipPageTime);
                }
                else
                {
                    HandAnimations.INSTANCE.PlayHandAnimation(HandAnimation.FlipAnim, flipPageTime * 10);
                }

            }
            else
            {

            }
            Debug.Log("CONTINUE THIS =!");
            /*
            else if(data == LedgerMovementActions)
            else
            {

            }
            */


        }
    }
        


    }

