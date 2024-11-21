using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public class VisitedRateViewModel
    {
        /// <summary>
        /// 道の駅データ管理Model
        /// </summary>
        private MichiNoEkiDataModel _michiNoEkiDataModel;

        /// <summary>
        /// 訪問率リスト
        /// </summary>
        public Dictionary<string, VisitedRateViewRegionListItem> VisitedRateList { get; set; } = new Dictionary<string, VisitedRateViewRegionListItem>();

        /// <summary>
        /// 地方選択コマンド
        /// </summary>
        public DelegateCommand SelectRegionCommand { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public VisitedRateViewModel()
        {
            // 道の駅データ管理Modelのインスタンス取得
            _michiNoEkiDataModel = App.Instance.MichiNoEkiDataModel;

            // 道の駅データ関連のイベント捕捉
            _michiNoEkiDataModel.LoadedMichiNoEkiInfoEvent += OnLoadedMichiNoEkiInfoEvent;
            _michiNoEkiDataModel.UpdateMichiNoEkiInfoEvent += OnUpdateMichiNoEkiInfoEvent;

            // コマンドハンドラ登録
            SelectRegionCommand = new DelegateCommand(OnSelectRegionCommand);

            // 訪問率リストの初期化
            foreach (var item in RegionPrefectureDic.Dic.Keys)
            {
                VisitedRateList.Add(item, new VisitedRateViewRegionListItem(item));
            }
        }

        /// <summary>
        /// 道の駅データロード完了イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void OnLoadedMichiNoEkiInfoEvent(object? sender, LoadedMichiNoEkiInfoEventArgs e)
        {
            // 道の駅ピン追加
            foreach (var michiNoEkiInfo in e.MichiNoEkiInfoList)
            {
                VisitedRateList[michiNoEkiInfo.Region].UpdateRate(michiNoEkiInfo);
            }
        }

        /// <summary>
        /// 道の駅データ更新イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void OnUpdateMichiNoEkiInfoEvent(object? sender, UpdateMichiNoEkiInfoEventArgs e)
        {
            switch (e.Kind)
            {
                // 更新
                case UpdateKind.Update:
                    // 訪問率更新
                    VisitedRateList[e.MichiNoEkiInfo.Region].UpdateRate(e.MichiNoEkiInfo);
                    break;

                // 追加(機能なし)
                case UpdateKind.Add:
                    break;

                // 削除(機能なし)
                case UpdateKind.Delete:
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 地方選択コマンドハンドラ
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void OnSelectRegionCommand(object? param)
        {
            if (param is string name)
            {
                // 訪問率リストの地方別リストの表示状態を切り替える
                VisitedRateList[name].IsOpened.Value = !VisitedRateList[name].IsOpened.Value;
            }
        }
    }
}
