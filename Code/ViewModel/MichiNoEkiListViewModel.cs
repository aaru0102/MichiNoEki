using ObservableCollections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    public class MichiNoEkiListViewModel
    {
        /// <summary>
        /// 道の駅データ管理Model
        /// </summary>
        private MichiNoEkiDataModel _michiNoEkiDataModel;

        /// <summary>
        /// 道の駅名称リスト
        /// </summary>
        public ObservableCollection<MichiNoEkiListViewMichiNoEkiNameList> MichiNoEkiNameList { get; set; } = new ObservableCollection<MichiNoEkiListViewMichiNoEkiNameList>();

        /// <summary>
        /// 地方名称リスト
        /// </summary>
        public List<string> RegionNameList { get; set; } = new List<string>();

        /// <summary>
        /// 都道府県名称リスト
        /// </summary>
        public ObservableCollection<string> PrefectureNameList { get; set; } = new ObservableCollection<string>();

        /// <summary>
        /// 選択地方
        /// </summary>
        public AutoNotifyProperty<string?> SelectedRegion { get; set; } = new AutoNotifyProperty<string?>(null);

        /// <summary>
        /// 選択都道府県
        /// </summary>
        public AutoNotifyProperty<string?> SelectedPrefecture { get; set; } = new AutoNotifyProperty<string?>(null);

        /// <summary>
        /// 地方選択コマンド
        /// </summary>
        public DelegateCommand SelectRegionCommand { get; set; }

        /// <summary>
        /// 都道府県選択コマンド
        /// </summary>
        public DelegateCommand SelectPrefectureCommand { get; set; }

        /// <summary>
        /// 道の駅選択コマンド
        /// </summary>
        public DelegateCommand SelectMichiNoEkiCommand { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MichiNoEkiListViewModel()
        {
            // 道の駅データ管理Modelのインスタンス取得
            _michiNoEkiDataModel = App.Instance.MichiNoEkiDataModel;

            _michiNoEkiDataModel.UpdateMichiNoEkiInfoEvent += OnUpdateMichiNoEkiInfoEvent;

            // コマンドハンドラ登録
            SelectRegionCommand = new DelegateCommand(OnSelectRegionCommand);
            SelectPrefectureCommand = new DelegateCommand(OnSelectPrefectureCommand);
            SelectMichiNoEkiCommand = new DelegateCommand(OnSelectMichiNoEkiCommand);

            // 地方名称リスト初期化
            foreach (var item in RegionPrefectureDic.Dic.Keys)
            {
                RegionNameList.Add(item);
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
                    // 訪問状態更新
                    MichiNoEkiListViewMichiNoEkiNameList? info = MichiNoEkiNameList.FirstOrDefault(info => info.ID == e.MichiNoEkiInfo.ID);
                    if (info != null)
                    {
                        info.IsVisited.Value = e.MichiNoEkiInfo.IsVisited;
                    };
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
        /// 道の駅選択コマンドハンドラ
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void OnSelectMichiNoEkiCommand(object? param)
        {
            // 詳細表示
            if(param is int id)
            {
                _michiNoEkiDataModel.ShowDetailMichiNoEkiInfo(id);
            }
        }

        /// <summary>
        /// 都道府県選択コマンドハンドラ
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void OnSelectPrefectureCommand(object? param)
        {
            // 未選択時はスルー
            if (SelectedPrefecture.Value == null)
            {
                return;
            }

            // 道の駅データリスト取得
            List<MichiNoEkiInfo> infoList = _michiNoEkiDataModel.GetAllMichiNoEkiInfo();

            // 選択都道府県に合わせて道の駅名称リストを更新
            MichiNoEkiNameList.Clear();
            foreach (var info in infoList)
            {
                if (info.Prefecture == SelectedPrefecture.Value)
                {
                    MichiNoEkiNameList.Add(new MichiNoEkiListViewMichiNoEkiNameList(info));
                }
            }
        }

        /// <summary>
        /// 地方選択コマンドハンドラ
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void OnSelectRegionCommand(object? param)
        {
            // 未選択の場合はスルー
            if (SelectedRegion.Value == null)
            {
                return;
            }

            // 都道府県名称リストを更新
            PrefectureNameList.Clear();
            foreach (var item in RegionPrefectureDic.Dic[SelectedRegion.Value])
            {
                PrefectureNameList.Add(item);
            }
        }
    }
}
