using UnityEngine;
using System.Collections;

namespace SKSCommon
{
    public class GUIFactory<T>
    {

    }

    public class FactoryLabelAttribute : PropertyAttribute
    {

    }



    [System.Serializable]
    public class UpdateInterval
    {
        public enum Label { Update, LateUpdate, FixedUpdate }
    }



    [System.Serializable]
    public class Target
    {
        public enum Label { Position, Transform, Tag, Player }
        
        public Label behavior = Label.Position;
        
        [ConditionalHideIntCustomDisplay("behavior", (int)Label.Position, ConditionalHideBehavior.Hide, CustomDisplayMode.NoLabel)]
        public TargetPosition targetPosition;

        [ConditionalHideIntCustomDisplay("behavior", (int)Label.Transform, ConditionalHideBehavior.Hide, CustomDisplayMode.NoLabel)]
        public TargetTransform targetTransform;

        [ConditionalHideIntCustomDisplay("behavior", (int)Label.Tag, ConditionalHideBehavior.Hide, CustomDisplayMode.NoLabel)]
        public TargetTag targetTag;

        //[ConditionalHideIntCustomDisplay("behavior", (int)Label.Player, ConditionalHideBehavior.Hide, CustomDisplayMode.NoLabel)]
        [HideInInspector]
        public TargetPlayer targetPlayer;


        private ITargetBehavior thisBehavior
        {
            get
            {
                switch (behavior)
                {
                    case Label.Position:
                        return targetPosition;
                    case Label.Transform:
                        return targetTransform;
                    case Label.Tag:
                        return targetTag;
                    case Label.Player:
                        return targetPlayer;
                    default:
                        return null;
                }
            }
        }
        

        public Vector3 position
        {
            get
            {
                return thisBehavior.GetPosition();
            }
        }
        

        
        public interface ITargetBehavior
        {
            Vector3 GetPosition();
        }

        [System.Serializable]
        public class TargetPosition : ITargetBehavior
        {
            public Vector3 targetPosition;
            
            public Vector3 GetPosition()
            {
                return targetPosition;
            }
        }

        [System.Serializable]
        public class TargetTransform : ITargetBehavior
        {
            public Transform targetTransform;
            
            public Vector3 GetPosition()
            {
                if (targetTransform == null)
                {
                    return Vector3.zero;
                }
                else
                {
                    return targetTransform.position;
                }
            }
        }

        [System.Serializable]
        public class TargetTag : ITargetBehavior
        {
            private Transform targetTransform;
            [Tag]
            public string targetTag = "Untagged";
            
            public Vector3 GetPosition()
            {
                if (targetTransform == null || targetTransform.gameObject.tag != targetTag)
                {
                    targetTransform = null;

                    GameObject tObj = GameObject.FindWithTag(targetTag);
                    if (tObj != null)
                    {
                        targetTransform = tObj.transform;
                    }
                }

                if (targetTransform == null)
                {
                    return Vector3.zero;
                }
                else
                {
                    return targetTransform.position;
                }
            }
        }

        [System.Serializable]
        public class TargetPlayer : ITargetBehavior
        {
            public Vector3 GetPosition()
            {
                if (Game.Player == null)
                {
                    return Vector3.zero;
                }
                else
                {
                    return Game.Player.position;
                }
            }
        }

    }


    [System.Serializable]
    public class Interpolation
    {

    }



}

