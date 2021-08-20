using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;
using Unity.Physics;

public class AsteroidFloat : JobComponentSystem//anything marked as JobComponentSystem will run in entites within ECS?
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dT = Time.DeltaTime;

        var jobHandle = Entities.WithName("AsteroidFloat")
            .ForEach((ref PhysicsVelocity physics,
            ref Translation position,
            ref Rotation rotation,
            ref FloatData floatData) =>//it will effect the physics, position and rotation of anything with the floatData script attached
            {
                float s = math.sin((dT + position.Value.x) * 0.1f) * floatData.speed;
                float c = math.cos((dT + position.Value.y) * 0.1f) * floatData.speed;

                float3 dir = new float3(s, c, s);
                physics.Linear += dir;
            })
            .Schedule(inputDeps);

        return jobHandle;
    }
}
