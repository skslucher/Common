using UnityEngine;
using System.Collections;

namespace OnPhysicsInteraction
{

    [System.Flags]
    public enum ResetTiming
    {
        None = 0,
        Awake = 1,
        Start = 2,
        Enable = 4,
        FixedUpdate = 8,
        Update = 16,
        Interaction = 32
    }

    [System.Flags]
    public enum InteractionType
    {
        Trigger = 1,
        Collision = 2
    }

    [System.Flags]
    public enum InteractionDimensions
    {
        Physics3D = 1,
        Physics2D = 2,
    }

    [System.Flags]
    public enum InteractionTiming
    {
        Enter = 1,
        Stay = 2,
        Exit = 4
    }


    public class OnPhysicsFunction : MonoBehaviour
    {

        private bool isActive = true;
        public virtual bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
            }
        }
        

        public virtual ResetTiming resetOn { get { return ResetTiming.Enable; } }
        
        public virtual InteractionType interactionType { get { return (InteractionType.Collision | InteractionType.Trigger); } }
        
        public virtual InteractionDimensions interactionDimensions { get { return (InteractionDimensions.Physics2D | InteractionDimensions.Physics3D); } }
        
        public virtual InteractionTiming interactionTiming { get { return InteractionTiming.Enter; } }


        #region Reset

        void Awake()
        {
            Reset(ResetTiming.Awake);
        }
        void Start()
        {
            Reset(ResetTiming.Start);
        }
        void OnEnable()
        {
            Reset(ResetTiming.Enable);
        }
        void FixedUpdate()
        {
            Reset(ResetTiming.FixedUpdate);
        }
        void Update()
        {
            Reset(ResetTiming.Update);
        }


        public virtual void Reset(ResetTiming thisTiming)
        {
            if (resetOn.HasFlag(thisTiming))
            {
                Reset();
            }
        }

        public virtual void Reset()
        {
            isActive = true;
        }

        #endregion


        #region Physics Callbacks

        #region Physics3D

        #region OnCollision
        void OnCollisionEnter(Collision collision)
        {
            if (IsValidInteraction(InteractionType.Collision, InteractionDimensions.Physics3D, InteractionTiming.Enter))
            {
                Interaction(collision);
            }
        }
        void OnCollisionStay(Collision collision)
        {
            if (IsValidInteraction(InteractionType.Collision, InteractionDimensions.Physics3D, InteractionTiming.Stay))
            {
                Interaction(collision);
            }
        }
        void OnCollisionExit(Collision collision)
        {
            if (IsValidInteraction(InteractionType.Collision, InteractionDimensions.Physics3D, InteractionTiming.Exit))
            {
                Interaction(collision);
            }
        }
        #endregion

        #region OnTrigger
        void OnTriggerEnter(Collider collider)
        {
            if (IsValidInteraction(InteractionType.Trigger, InteractionDimensions.Physics3D, InteractionTiming.Enter))
            {
                Interaction(collider);
            }
        }
        void OnTriggerStay(Collider collider)
        {
            if (IsValidInteraction(InteractionType.Trigger, InteractionDimensions.Physics3D, InteractionTiming.Stay))
            {
                Interaction(collider);
            }
        }
        void OnTriggerExit(Collider collider)
        {
            if (IsValidInteraction(InteractionType.Trigger, InteractionDimensions.Physics3D, InteractionTiming.Exit))
            {
                Interaction(collider);
            }
        }
        #endregion

        #endregion

        #region Physics2D

        #region OnCollision
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (IsValidInteraction(InteractionType.Collision, InteractionDimensions.Physics2D, InteractionTiming.Enter))
            {
                Interaction(collision);
            }
        }
        void OnCollisionStay2D(Collision2D collision)
        {
            if (IsValidInteraction(InteractionType.Collision, InteractionDimensions.Physics2D, InteractionTiming.Stay))
            {
                Interaction(collision);
            }
        }
        void OnCollisionExit2D(Collision2D collision)
        {
            if (IsValidInteraction(InteractionType.Collision, InteractionDimensions.Physics2D, InteractionTiming.Exit))
            {
                Interaction(collision);
            }
        }
        #endregion

        #region OnTrigger
        void OnTriggerEnter2D(Collider2D collider)
        {
            if (IsValidInteraction(InteractionType.Trigger, InteractionDimensions.Physics2D, InteractionTiming.Enter))
            {
                Interaction(collider);
            }
        }
        void OnTriggerStay2D(Collider2D collider)
        {
            if (IsValidInteraction(InteractionType.Trigger, InteractionDimensions.Physics2D, InteractionTiming.Stay))
            {
                Interaction(collider);
            }
        }
        void OnTriggerExit2D(Collider2D collider)
        {
            if (IsValidInteraction(InteractionType.Trigger, InteractionDimensions.Physics2D, InteractionTiming.Exit))
            {
                Interaction(collider);
            }
        }
        #endregion

        #endregion

        private bool IsValidInteraction(InteractionType thisType, InteractionDimensions thisDimensions, InteractionTiming thisTiming)
        {
            return interactionType.HasFlag(thisType) && interactionDimensions.HasFlag(thisDimensions) && interactionTiming.HasFlag(thisTiming);
        }

        #endregion


        #region Actions

        #region Interaction

        public virtual void Interaction(Collider collider)
        {
            if (TestInteraction(collider))
            {
                Action(collider);
                Reset(ResetTiming.Interaction);
            }
        }
        public virtual void Interaction(Collision collision)
        {
            if (TestInteraction(collision))
            {
                Action(collision);
                Reset(ResetTiming.Interaction);
            }
        }
        public virtual void Interaction(Collider2D collider)
        {
            if (TestInteraction(collider))
            {
                Action(collider);
                Reset(ResetTiming.Interaction);
            }
        }
        public virtual void Interaction(Collision2D collision)
        {
            if (TestInteraction(collision))
            {
                Action(collision);
                Reset(ResetTiming.Interaction);
            }
        }

        #endregion

        #region TestInteraction

        public virtual bool TestInteraction(Collision collision)
        {
            return TestInteraction(collision.collider);
        }
        public virtual bool TestInteraction(Collider collider)
        {
            return TestInteraction(collider.gameObject);
        }
        public virtual bool TestInteraction(Collision2D collision)
        {
            return TestInteraction(collision.collider);
        }
        public virtual bool TestInteraction(Collider2D collider)
        {
            return TestInteraction(collider.gameObject);
        }

        public virtual bool TestInteraction(GameObject otherObject)
        {
            return TestInteraction();
        }
        public virtual bool TestInteraction()
        {
            if (IsActive)
            {
                IsActive = false;
                return true;
            }
            return false;
        }

        #endregion

        #region Action

        public virtual void Action(Collider collider)
        {
            Action(collider.gameObject);
        }
        public virtual void Action(Collision collision)
        {
            Action(collision.gameObject);
        }
        public virtual void Action(Collider2D collider)
        {
            Action(collider.gameObject);
        }
        public virtual void Action(Collision2D collision)
        {
            Action(collision.gameObject);
        }

        public virtual void Action(GameObject otherObject)
        {
            Action();
        }

        public virtual void Action()
        {
            //Override me!
        }

        #endregion

        #endregion

    }


    public class OnPhysicsAction : MonoBehaviour
    {
        private bool isActive = true;
        public virtual bool IsActive
        {
            get
            {
                return isActive;
            }
            set
            {
                isActive = value;
            }
        }


        public virtual ResetTiming resetOn { get { return ResetTiming.Enable; } }
        
        public virtual InteractionType interactionType { get { return (InteractionType.Collision | InteractionType.Trigger); } }
        
        public virtual InteractionDimensions interactionDimensions { get { return (InteractionDimensions.Physics2D | InteractionDimensions.Physics3D); } }
        
        public virtual InteractionTiming interactionTiming { get { return InteractionTiming.Enter; } }


        #region Reset

        void Awake()
        {
            Reset(ResetTiming.Awake);
        }
        void Start()
        {
            Reset(ResetTiming.Start);
        }
        void OnEnable()
        {
            Reset(ResetTiming.Enable);
        }
        void FixedUpdate()
        {
            Reset(ResetTiming.FixedUpdate);
        }
        void Update()
        {
            Reset(ResetTiming.Update);
        }


        public virtual void Reset(ResetTiming thisTiming)
        {
            if (resetOn.HasFlag(thisTiming))
            {
                Reset();
            }
        }

        public virtual void Reset()
        {
            isActive = true;
        }

        #endregion


        #region Physics Callbacks

        #region Physics3D

        #region OnCollision
        void OnCollisionEnter()
        {
            if (IsValidInteraction(InteractionType.Collision, InteractionDimensions.Physics3D, InteractionTiming.Enter))
            {
                Interaction();
            }
        }
        void OnCollisionStay()
        {
            if (IsValidInteraction(InteractionType.Collision, InteractionDimensions.Physics3D, InteractionTiming.Stay))
            {
                Interaction();
            }
        }
        void OnCollisionExit()
        {
            if (IsValidInteraction(InteractionType.Collision, InteractionDimensions.Physics3D, InteractionTiming.Exit))
            {
                Interaction();
            }
        }
        #endregion

        #region OnTrigger
        void OnTriggerEnter()
        {
            if (IsValidInteraction(InteractionType.Trigger, InteractionDimensions.Physics3D, InteractionTiming.Enter))
            {
                Interaction();
            }
        }
        void OnTriggerStay()
        {
            if (IsValidInteraction(InteractionType.Trigger, InteractionDimensions.Physics3D, InteractionTiming.Stay))
            {
                Interaction();
            }
        }
        void OnTriggerExit()
        {
            if (IsValidInteraction(InteractionType.Trigger, InteractionDimensions.Physics3D, InteractionTiming.Exit))
            {
                Interaction();
            }
        }
        #endregion

        #endregion

        #region Physics2D

        #region OnCollision
        void OnCollisionEnter2D()
        {
            if (IsValidInteraction(InteractionType.Collision, InteractionDimensions.Physics2D, InteractionTiming.Enter))
            {
                Interaction();
            }
        }
        void OnCollisionStay2D()
        {
            if (IsValidInteraction(InteractionType.Collision, InteractionDimensions.Physics2D, InteractionTiming.Stay))
            {
                Interaction();
            }
        }
        void OnCollisionExit2D()
        {
            if (IsValidInteraction(InteractionType.Collision, InteractionDimensions.Physics2D, InteractionTiming.Exit))
            {
                Interaction();
            }
        }
        #endregion

        #region OnTrigger
        void OnTriggerEnter2D()
        {
            if (IsValidInteraction(InteractionType.Trigger, InteractionDimensions.Physics2D, InteractionTiming.Enter))
            {
                Interaction();
            }
        }
        void OnTriggerStay2D()
        {
            if (IsValidInteraction(InteractionType.Trigger, InteractionDimensions.Physics2D, InteractionTiming.Stay))
            {
                Interaction();
            }
        }
        void OnTriggerExit2D()
        {
            if (IsValidInteraction(InteractionType.Trigger, InteractionDimensions.Physics2D, InteractionTiming.Exit))
            {
                Interaction();
            }
        }
        #endregion

        #endregion

        private bool IsValidInteraction(InteractionType thisType, InteractionDimensions thisDimensions, InteractionTiming thisTiming)
        {
            return interactionType.HasFlag(thisType) && interactionDimensions.HasFlag(thisDimensions) && interactionTiming.HasFlag(thisTiming);
        }

        #endregion


        #region Actions

        public virtual void Interaction()
        {
            if (TestInteraction())
            {
                Action();
                Reset(ResetTiming.Interaction);
            }
        }

        public virtual bool TestInteraction()
        {
            if (IsActive)
            {
                IsActive = false;
                return true;
            }
            return false;
        }

        public virtual void Action()
        {
            //Override me!
        }

        #endregion

    }


    public class OnTriggerAction : OnPhysicsAction
    {
        public override InteractionType interactionType
        {
            get
            {
                return InteractionType.Trigger;
            }
        }
    }

    public class OnCollisionAction : OnPhysicsAction
    {
        public override InteractionType interactionType
        {
            get
            {
                return InteractionType.Collision;
            }
        }
    }

}