using System.Collections.Generic;
using UnityEngine;

public class PooledFishFactory
{
    private FishComponentContainer _fishPrefab;
    private UnityLifecycleEventRunner _unityLifecycleEventRunner;
    private Bounds _fishMoveableBounds;
    private Queue<Fish> _fishQueue = new Queue<Fish>();
    private List<Fish> _activeFish = new List<Fish>();
    public IReadOnlyList<Fish> ActiveFish => _activeFish;

    public PooledFishFactory(UnityLifecycleEventRunner unityLifecycleEventRunner, FishComponentContainer fishPrefab, Bounds fishMoveableBounds)
    {
        _fishPrefab = fishPrefab;
        _unityLifecycleEventRunner = unityLifecycleEventRunner;
        _fishMoveableBounds = fishMoveableBounds;
    }

    public Fish Create(Vector3 position, Quaternion rotation)
    {
        return GetResetPooledFish(position, rotation);
    }

    public Fish GetResetPooledFish(Vector3 position, Quaternion rotation)
    {
        if(_fishQueue.Count == 0)
        {
            Fish newFish = CreateNewFish(position, rotation);
            _fishQueue.Enqueue(newFish);
        }

        Fish pooledFishComponentContainer = _fishQueue.Dequeue();
        pooledFishComponentContainer.ToggleActive(true);
        _activeFish.Add(pooledFishComponentContainer);
        return pooledFishComponentContainer;
    }

    private Fish CreateNewFish(Vector3 position, Quaternion rotation)
    {
        FishComponentContainer fishComponentContainer = GameObject.Instantiate(_fishPrefab, position, rotation);
        MoveToRandomPositionInBounds randomMovementMover = new MoveToRandomPositionInBounds(fishComponentContainer.RigidBody, _fishMoveableBounds, _unityLifecycleEventRunner);
        SetPositionToMousePhysics mouseFollowMover = new SetPositionToMousePhysics(fishComponentContainer.RigidBody);
        FallingPhysics fallingMover = new FallingPhysics(fishComponentContainer.RigidBody);

        Fish newFish = new Fish(_unityLifecycleEventRunner, fishComponentContainer, fallingMover, mouseFollowMover, randomMovementMover);

        return newFish;
    }

    public void Remove(int count)
    {
        List<Fish> fishToRemove = new List<Fish>();

        for(int i = 0; i < count && i < _activeFish.Count; i++)
        {
            _fishQueue.Enqueue(_activeFish[i]);
            _activeFish[i].ToggleActive(false);
            fishToRemove.Add(_activeFish[i]);
        }

        _activeFish.RemoveAll(x => fishToRemove.Contains(x));
    }
}
