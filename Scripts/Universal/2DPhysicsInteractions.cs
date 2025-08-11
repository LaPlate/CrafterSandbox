using System.Security.AccessControl;
using Godot;

public static class PhysicsInteractions2D
{
    public static bool HasLineOfSight(Node2D source, Node2D target, uint collisionMask)
    {
        var space = source.GetWorld2D().DirectSpaceState;

        var exclude = new Godot.Collections.Array<Rid>();
        if (source is CollisionObject2D collisionSource)
            exclude.Add(collisionSource.GetRid());

        var rayParams = new PhysicsRayQueryParameters2D
        {
            From = source.GlobalPosition,
            To = target.GlobalPosition,
            CollisionMask = collisionMask,
            Exclude = exclude
        };

        var rayResult = space.IntersectRay(rayParams);

        if (rayResult.Count == 0)
            return true;

        if (rayResult.TryGetValue("collider", out var hit))
        {
            var hitObj = hit.AsGodotObject();
            if (hitObj == target)
                return true;
        }
        return false;
    }

}