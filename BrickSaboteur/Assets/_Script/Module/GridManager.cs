#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 
//*Function:
//===================================================
// Fix:
//===================================================

#endregion
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NearyFrame;
using NearyFrame.Base;
using Sirenix.Serialization;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;
namespace BrickSaboteur
{
    public interface IGridTag : IModuleTag<IGridTag> { }
    /// <summary>
    /// 网格管理
    /// </summary>
    /// <typeparam name="EntityManager">Self</typeparam>
    /// <typeparam name="IEntityTag">Tag</typeparam>
    public class GridManager : ManagerBase<GridManager, IGridTag>
    {
        [SerializeField] public Tilemap levelTile;
        [SerializeField] TileBase _wall;
        [SerializeField] public List<TileBase> bricktileList;
        [OdinSerialize] public int leftBrickCount => bricktileList.Count;
        protected override void OnDestroy()
        {
            Mgr.Instance.UnRegisterModule(this);
            Debug.Log("UnRegister GridManager");
        }

        protected override System.Collections.IEnumerator OnInitializeRegisterSelf()
        {
            Mgr.Instance.RegisterModule(this);
            yield return null;
            Debug.Log("Create GridManager");
            // var scene = GameObject.Find("Scene");
            // this.transform.parent = scene.transform;

            // Addressables.LoadAssets<GameObject>("Grids", x => Debug.Log(x.Result.name));
            // grids = new Queue<GridEntity>();
            // var initDiff = 3;
            // for (int i = 1; i <= initDiff; i++)
            // {
            //     currentDiff = i;
            //     Generate(currentDiff);
            // }
            // var pool = MgrM.PoolModule.GetOrCreate<GridEntity>(CreateFunc(), nameof(GridEntity) + "Pool");
            BoundsInt bounds = levelTile.cellBounds;
            TileBase[] allTiles = levelTile.GetTilesBlock(bounds);
            bricktileList = allTiles.Where(x => x != null && x.name != _wall.name).ToList();
        }
        public void ReleaseTileWorldPos(Vector2 point)
        {
            var gridPos = levelTile.WorldToCell(point);
            var tile = levelTile.GetTile(gridPos);
            if (tile != null && tile.name != _wall.name)
            {
                bricktileList.Remove(tile);
                levelTile.SetTile(gridPos, null);
                int bonusNum = Random.Range(1, 50);
                if (bonusNum < 4)
                    BrickMgrM.LoaderManager.InstantiateByPath<SkillBonusEntity>("Entity/Bonus_" + bonusNum, this.transform, x =>
                    {
                        x.Result.transform.position = point;
                    })
                    .Subscribe();
                if (leftBrickCount <= 0)
                {
                    NextLevel();
                }
            }
        }
        private void NextLevel()
        {
            //todo 
            Debug.Log("Yesss");
            // StopAllCoroutines();
            // Observable.Timer(System.TimeSpan.FromSeconds(5))
            //     .Subscribe(__ =>
            //     {
            //         // BrickMgrM.PoolModule.GetPool<BallEntity>().shrinkDisposable.Dispose();
            //         BrickMgrM.PoolModule.GetPool<BallEntity>().Clear();
            //     });
        }
        // int maxDifficulty;
        // int sum;
        // List<int> difList;
        // private void Calculate(int maxDifficulty)
        // {
        //     difList = new List<int>();
        //     for (int i = 1; i <= maxDifficulty; i++)
        //     {
        //         difList.Add(i);
        //         sum += i;
        //     }
        // }
        // private float GetProbability(int difficulty)
        // {
        //     difList.Remove(difficulty);
        //     var weight = maxDifficulty - difficulty + 1;
        //     return weight / sum;
        // }
        Queue<GridEntity> grids;
        GridEntity last;
        int currentDiff;
        public void GenerateNext()
        {
            Generate(++currentDiff);
        }
        private void Generate(int difficulty)
        {
            //Level/Level,1,
            var path = "Level/Level,1," + difficulty;
            // Debug.Log(path);
            // var op = MgrM.LoaderManager.InstantiateGOByPath(path, this.transform);
            // op.Completed += x =>
            // {
            //     var grid = x.Result.GetComponent<GridEntity>();
            //     grids.Enqueue(grid);
            //     if (last != null)
            //     {

            //         Debug.Log("Generate --" + difficulty + "-- Last is " + last.right.transform.position);
            //         x.Result.transform.position = last.right.transform.position;
            //     }
            //     if (currentDiff >= 5)
            //     {
            //         var obj = grids.Dequeue();
            //         Addressables.ReleaseInstance(obj.gameObject);
            //     }
            //     last = grid;
            // };
            // var ob = MgrM.LoaderManager.InstantiateByPath<GameObject>(path, this.transform);
            // ob.Subscribe(x =>
            // {
            //     var grid = x.GetComponent<GridEntity>();
            //     grids.Enqueue(grid);
            //     if (last != null)
            //     {
            //         Debug.Log("Generate --" + difficulty + "-- Last is " + last.right.transform.position);
            //         x.transform.position = last.right.transform.position;
            //     }
            //     if (currentDiff >= 5)
            //     {
            //         var obj = grids.Dequeue();
            //         Addressables.ReleaseInstance(obj.gameObject);
            //     }
            //     last = grid;
            // });
            // yield return op;
        }
        // private System.Func<System.IObservable<GridEntity>> CreateFunc()
        // {

        //     yield return null;
        // }
    }
}
