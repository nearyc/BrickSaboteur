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
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Tilemaps;
namespace BrickSaboteur
{
    public interface IGridTag : IModuleTag<IGridTag>
    {
        void ReleaseTileWorldPos(Vector2 point);
    }
    /// <summary>
    /// 网格管理
    /// </summary>
    /// <typeparam name="EntityManager">Self</typeparam>
    /// <typeparam name="IEntityTag">Tag</typeparam>
    public class GridManager : ManagerBase<GridManager, IGridTag>, IGridTag
    {
        [SerializeField] public Tilemap levelTile;
        [SerializeField] public GameObject bgTile;
        [SerializeField] private TileBase _wall;
        [SerializeField] public List<TileBase> brickTileList;
        [Sirenix.OdinInspector.ShowInInspector]
        public int leftBrickCount => brickTileList.Count;
        protected override void OnDestroy()
        {
            Mgr.Instance.UnRegisterModule(this);
            Debug.Log("UnRegister GridManager");
        }

        protected override System.Collections.IEnumerator OnInitializeRegisterSelf()
        {
            Mgr.Instance.RegisterModule(this);
            Debug.Log("Create GridManager");
            yield return BrickMgrM.WaitModule<ILoaderTag>();

            //游戏开始，加载对应关卡的LevelTile
            MessageBroker.Default.Receive<GameTag_GameStart>().Subscribe(x => StartCoroutine(LoadLevel(x.level, x.difficulty))).AddTo(this);;
        }
        private IEnumerator LoadLevel(int level, EDifficulty difficulty)
        {
            //bgTile
            if (bgTile != null)
            {
                BrickMgrM.LoaderManager.ReleaseObject(bgTile.gameObject);
            }
            var bgStream = BrickMgrM.LoaderManager.InstantiatePrefabByPath<GameObject>(AssetPath.LevelBackGround, null).Last().Do(x =>
            {
                bgTile = x;
            });
            yield return bgStream.ToYieldInstruction().AddTo(this);
            //levelTile
            if (levelTile != null)
                BrickMgrM.LoaderManager.ReleaseObject(levelTile.gameObject);
            var path = "";
            switch (difficulty)
            {
                case EDifficulty.Eazy:
                    path = $"{AssetPath.EasyLevel}{level}";
                    break;
                case EDifficulty.Hard:
                    path = $"{AssetPath.HardLevel}{level}";
                    break;
            }
            Debug.Log(path);
            var levelStram = BrickMgrM.LoaderManager.InstantiatePrefabByPath<GameObject>(path).Last().Do(x =>
            {
                x.transform.parent = bgTile.transform;
                levelTile = x.GetComponent<Tilemap>();

            });
            yield return levelStram.ToYieldInstruction().AddTo(this);

            BoundsInt bounds = levelTile.cellBounds;
            TileBase[] allTiles = levelTile.GetTilesBlock(bounds);
            brickTileList = allTiles.Where(x => x != null && x.name != _wall.name).ToList();
        }
        private Transform _bonusParent;
        public void ReleaseTileWorldPos(Vector2 worldPos)
        {
            if (_bonusParent == null) _bonusParent = GameObject.Find("BonusParent").transform;
            var gridPos = levelTile.WorldToCell(worldPos);
            var tile = levelTile.GetTile(gridPos);
            if (tile != null && tile.name != _wall.name)
            {
                brickTileList.Remove(tile);
                levelTile.SetTile(gridPos, null);
                int bonusNum = Random.Range(1, 50);
                if (bonusNum < 4)
                    BrickMgrM.LoaderManager.InstantiatePrefabByPath<GameObject>(AssetPath.Bonus + bonusNum)
                    .Last()
                    .Subscribe(x =>
                    {
                        x.transform.parent = _bonusParent;
                        x.transform.position = worldPos;
                    });
                if (leftBrickCount <= 0)
                {
                    BrickMgrM.LoaderManager.GameEnd(true);
                }
            }
        }
    }
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
// Queue<GridEntity> grids;
// GridEntity last;
// int currentDiff;
// public void GenerateNext()
// {
//     Generate(++currentDiff);
// }
// private void Generate(int difficulty)
// {
//     //Level/Level,1,
//     var path = "Level/Level,1," + difficulty;
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
// }
// private System.Func<System.IObservable<GridEntity>> CreateFunc()
// {

//     yield return null;
// }
