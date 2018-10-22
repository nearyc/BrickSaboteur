#region Author & Version
//==================================================== 
// Author：Nearyc 
// File name：......
// Version：V1.0.1
// Date : 2018.10.1
//*Function:
//===================================================
// Fix:
//===================================================

#endregion
using System.Collections;
using Nearyc.Roslyn;
using Nearyc.Skill;
using NearyFrame.Base;
using UniRx;
using UnityEngine;
namespace BrickSaboteur
{
    public class SkillHolder : ElementBaseSingle<SkillHolder, IInputTag>
    {
        [SerializeField] NormalSKill plus;
        [SerializeField] public Property plusCount;
        [SerializeField] NormalSKill multiply;
        [SerializeField] public Property multiplyCount;
        [SerializeField] public Property lifeCount;

        protected override void OnDestroy()
        {
            this.UnRegisterSelf(this);
        }
        protected override IEnumerator AfterStart()
        {
            yield return null;
            this.RegisterSelf(this);

            plusCount.Init(20);
            multiplyCount.Init(20);
            lifeCount.Init(3);

            plus = new NormalSKill(this, ESkillTag.Attack, 0.5f);
            plus.onSkillStart += PlusSKillExecute;

            multiply = new NormalSKill(this, ESkillTag.Attack, 0.5f);
            multiply.onSkillStart += MultiplySkillExecute;
        }
		private void Update()
		{
			if (Input.GetMouseButtonDown(0))
			{
				BrickMgrM.LoaderManager.InstantiateByPath<BallEntity>("Entity/Ball", this.transform, x =>
				{
					x.Result.transform.position = BrickMgrM.CameraManager.mainCam.ScreenToWorldPoint(Input.mousePosition);
				}).Subscribe();
			}
		}
		public void TryExecuteMultiply()
        {
            multiply.TryExecuteSKill(multiplyCount.Current > 1);
        }
        public void TryExecutePlus()
        {
            plus.TryExecuteSKill(plusCount.Current > 1);
        }
        private void PlusSKillExecute()
        {
            plusCount.ModifyCurrent(-1);
            for (int i = 0; i < 3; i++)
            {

                BrickMgrM.PoolModule.GetPool<BallEntity>()
                    .RentAsync()
                    .Subscribe(x =>
                    {
                        var zFloat = 45 * (i - 1) + Random.Range(-30, 30);
                        x.Init(BrickMgrM.EntityModule.boards[0].transform.position + new Vector3(0, 0.5f, 0), new Vector3(0, 0, zFloat));
                    });
            }
        }
        private void MultiplySkillExecute()
        {

            if (temp != null)
                temp.Dispose();
            temp = Observable.FromCoroutine(MultiplyAsync).Subscribe().AddTo(this);
        }
        System.IDisposable temp;
        private IEnumerator MultiplyAsync()
        {
            multiplyCount.ModifyCurrent(-1);
            var tempList = ListPool<BallEntity>.Allocate();
            var counter = 0;
            foreach (var item in BrickMgrM.EntityModule.balls)
            {
                tempList.Add(item);
            }
            foreach (var item in tempList)
            {
                if (item.gameObject.activeInHierarchy == true)
                {
                    var zFloat = Random.Range(-80, 45);
                    item.Init(item.transform.position, new Vector3(0, 0, zFloat));

                    BrickMgrM.PoolModule.GetPool<BallEntity>()
                        .RentAsync()
                        .Subscribe(x =>
                        {
                            zFloat = Random.Range(-45, 80);
                            x.Init(item.transform.position, new Vector3(0, 0, zFloat));
                        });
                    BrickMgrM.PoolModule.GetPool<BallEntity>()
                        .RentAsync()
                        .Subscribe(x =>
                        {
                            zFloat = Random.Range(110, 340);
                            x.Init(item.transform.position, new Vector3(0, 0, zFloat));
                        });
                    counter++;
                }
                if (counter > 10)
                {
                    counter = 0;
                    yield return null;
                }
            }
            ListPool<BallEntity>.Free(tempList);
        }
    }
}
