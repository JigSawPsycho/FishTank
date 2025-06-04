using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] FishComponentContainer _fishPrefab;
    [SerializeField] Collider _fishSpawnBounds;
    [SerializeField] private List<FishSettings> _fishSettings;

    public int _fishCount = 1;

    private UnityLifecycleEventRunner _unityLifecycleEventRunner = new UnityLifecycleEventRunner();
    private PooledFishFactory _fishFactory;

    // -- Test values
    [SerializeField] private int _desiredFishCount;
    [SerializeField] private int _maxFishCount;
    
    public void Start()
    {
        _fishFactory = new PooledFishFactory(_unityLifecycleEventRunner, _fishPrefab, _fishSpawnBounds.bounds);
        SpawnFish(_fishCount);
    }

    public void SpawnFish(int count)
    {
        for(int i = 0; i < count; i++)
        {
            _fishFactory.Create(BoundsUtility.GetRandomPointInBounds(_fishSpawnBounds.bounds), Quaternion.identity);
        }
    }

    public void FixedUpdate()
    {
        _unityLifecycleEventRunner.FixedUpdate();
    }

    public void Update()
    {
        _unityLifecycleEventRunner.Update();
    }

    public void OnDrawGizmos()
    {
        if(Application.isPlaying) _unityLifecycleEventRunner.OnDrawGizmos();
    }

    public void OnDestroy()
    {
        _unityLifecycleEventRunner.OnDestroy();
    }

    private void MatchFishCount(int count)
    {
        if(_fishCount != count)
        {
            if(_fishCount < count)
            {
                SpawnFish(count - _fishCount);
            }
            else
            {
                _fishFactory.Remove(_fishCount - count);
            }

            _fishCount = count;
        }
    }

    public void OnGUI()
    {
        _desiredFishCount = (int) GUI.HorizontalSlider(new Rect(25, 25, 100, 25), _desiredFishCount, 0, _maxFishCount);
        MatchFishCount((int) _desiredFishCount);

        float buttonStartPosY = 50f;
        for(int i = 0; i < _fishSettings.Count; i++)
        {
            float yPos = buttonStartPosY + 25 * (i + 1);

            if(GUI.Button(new Rect(25, yPos, 100, 25), _fishSettings[i].Name))
            {
                FishSettings settings = _fishSettings[i];
                _fishSettings[i].Material.color = _fishSettings[i].Color;
                foreach(var fish in _fishFactory.ActiveFish)
                {
                    fish.SetMoveSpeed(settings.MoveSpeed)
                        .SetHead(settings.HeadMesh, settings.Material)
                        .SetMiddle(settings.MidMesh, settings.Material)
                        .SetTail(settings.TailMesh, settings.Material);
                }
            }
        }
    }
}
