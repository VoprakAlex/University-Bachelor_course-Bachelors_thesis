using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/StartRound")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "StartRound", message: "Target", category: "Events", id: "36052193b762e1ee0dd36ea8c70a11cc")]
public sealed partial class StartRound : EventChannel { }


