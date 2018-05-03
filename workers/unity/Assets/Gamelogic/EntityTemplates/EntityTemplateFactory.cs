using Improbable.Core;
using Improbable.Worker;
using Improbable.Unity.Core.Acls;
using Improbable.Unity.Entity;
using Improbable;
using Improbable.Collections; 
using UnityEngine;
using Assets.Gamelogic.Core;
using Lilsumn.Avalon.Core;
using Lilsumn.Avalon.World;

namespace Assets.Gamelogic.EntityTemplates
{
    public static class EntityTemplateFactory
    {
        public static Entity CreateChunkTemplate(Chunk.Data d, RegionSet.Data r, Vector3 pos) {
            return EntityBuilder.Begin()
                .AddPositionComponent(pos, CommonRequirementSets.PhysicsOnly)
                .AddMetadataComponent("chunk")
                .SetPersistence(true)
                .SetReadAcl(CommonRequirementSets.PhysicsOrVisual)
                .AddComponent(new GloballyVisible.Data(), CommonRequirementSets.PhysicsOnly)
                .AddComponent(d, CommonRequirementSets.PhysicsOnly)
                .AddComponent(r, CommonRequirementSets.PhysicsOnly)
                .Build();
        }

    }


}
