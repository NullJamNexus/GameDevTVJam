using System;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace NJN.Runtime.Controllers.Data
{
    [Serializable]
    public class FeedbackTypes
    {
        [field: SerializeField]
        public MMF_Player HitFeedback { get; private set; }
    }
}