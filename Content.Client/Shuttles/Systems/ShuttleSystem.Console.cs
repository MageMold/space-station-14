using Content.Client.Resources;
using Content.Client.Shuttles.UI;
using Content.Shared.Shuttles.Components;
using Content.Shared.Shuttles.UI.MapObjects;
using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Shared.Map;
using Robust.Shared.Physics.Components;

namespace Content.Client.Shuttles.Systems;

public sealed partial class ShuttleSystem
{
    [Dependency] private readonly IResourceCache _resource = default!;

    /// <summary>
    /// Gets the parallax to use for the specified map or uses the fallback if not available.
    /// </summary>
    public Texture GetTexture(Entity<ShuttleMapParallaxComponent?> entity)
    {
        if (!Resolve(entity, ref entity.Comp, false))
        {
            return _resource.GetTexture(ShuttleMapParallaxComponent.FallbackTexture);
        }

        return _resource.GetTexture(entity.Comp.TexturePath);
    }

    /// <summary>
    /// Gets the map coordinates of a map object.
    /// </summary>
    public MapCoordinates GetMapCoordinates(IMapObject mapObj)
    {
        switch (mapObj)
        {
            case ShuttleBeaconObject beacon:
                return GetCoordinates(beacon.Coordinates).ToMap(EntityManager, XformSystem);
            case ShuttleExclusionObject exclusion:
                return GetCoordinates(exclusion.Coordinates).ToMap(EntityManager, XformSystem);
            case GridMapObject grid:
                var gridXform = Transform(grid.Entity);
                Entity<PhysicsComponent?, TransformComponent?> gridEnt = (grid.Entity, null, gridXform);
                return new MapCoordinates(Maps.GetGridPosition(gridEnt), gridXform.MapID);
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
