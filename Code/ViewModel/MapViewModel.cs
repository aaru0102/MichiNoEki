using Microsoft.Maui.Maps;
using ObservableCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    /// <summary>
    /// マップページViewModel
    /// </summary>
    public class MapViewModel
    {
        private Dictionary<int, bool> _tempVisibleDic = new Dictionary<int, bool>();

        /// <summary>
        /// 道の駅データ管理Model
        /// </summary>
        private MichiNoEkiDataModel _michiNoEkiDataModel;

        /// <summary>
        /// フィルター設定データ管理Model
        /// </summary>
        private FilterSettingDataModel _filterSettingDataModel;

        /// <summary>
        /// 道の駅ピン
        /// </summary>
        public ObservableDictionary<int, MichiNoEkiPin> Pins { get; set; } = new ObservableDictionary<int, MichiNoEkiPin>();

        /// <summary>
        /// ピンの詳細ボタン押下時ハンドラ
        /// </summary>
        public Action<int>? DetailButtonClickHandler { get; set; }

        /// <summary>
        /// ピンの訪問ボタン押下時ハンドラ
        /// </summary>
        public Action<int>? VisitedButtonClickHandler { get; set; }


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MapViewModel()
        {
            // ピンのボタン押下時処理を追加
            DetailButtonClickHandler += DetailButtonClicked;
            VisitedButtonClickHandler += VisitedButtonClicked;

            // 道の駅データ管理Modelのインスタンス取得
            _michiNoEkiDataModel = App.Instance.MichiNoEkiDataModel;

            // 道の駅データ関連のイベント捕捉
            _michiNoEkiDataModel.LoadedMichiNoEkiInfoEvent += OnLoadedMichiNoEkiInfoEvent;
            _michiNoEkiDataModel.UpdateMichiNoEkiInfoEvent += OnUpdateMichiNoEkiInfoEvent;

            // フィルター設定データ管理Model取得
            _filterSettingDataModel = App.Instance.FilterSettingDataModel;

            // フィルター設定データ管理Modelのイベント捕捉
            _filterSettingDataModel.AllUpdateVisibleEvent += OnAllUpdateVisibleEvent;
            _filterSettingDataModel.UpdateVisibleEvent += OnUpdateVisibleEvent;
        }

        /// <summary>
        /// 詳細ボタンクリック時処理
        /// </summary>
        /// <param name="id">ID</param>
        private void DetailButtonClicked(int id)
        {
            // 道の駅データ詳細表示
            _michiNoEkiDataModel.ShowDetailMichiNoEkiInfo(id);
        }

        /// <summary>
        /// 訪問ボタンクリック時処理
        /// </summary>
        /// <param name="id">クリックした道の駅のID</param>
        private async void VisitedButtonClicked(int id)
        {
            // IDから道の駅データを取得
            MichiNoEkiInfo? info = _michiNoEkiDataModel.GetMichiNoEkiInfo(id);

            // 取得できた時に
            if (info != null)
            {
                // 訪問済み→未訪問
                if (info.IsVisited == true)
                {
                    // 確認ダイアログを表示
                    bool answer = await Application.Current!.MainPage!.DisplayAlert(
                        "確認",
                        "この道の駅を未訪問に変更しますか？\n(訪問日も削除されます)",
                        "いいえ",
                        "はい"
                    );

                    // ユーザーが「いいえ」を選択した場合、処理を中断
                    if (answer == true)
                    {
                        return;
                    }

                    // 訪問日を削除する
                    info.VisitedDate = null;
                }

                // 未訪問→訪問済み
                else
                {
                    // 訪問日に本日の日付を入れる
                    info.VisitedDate = DateTime.Now.Date;
                }

                // 訪問状態切り替え
                info.IsVisited = !info.IsVisited;

                // 道の駅データ更新
                _ = _michiNoEkiDataModel.UpadateMichiNoEkiInfo(info);
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
                Pins.Add(michiNoEkiInfo.ID, new MichiNoEkiPin(michiNoEkiInfo));

                if(_tempVisibleDic.ContainsKey(michiNoEkiInfo.ID) == true)
                {
                    Pins[michiNoEkiInfo.ID].Visibility.Value = _tempVisibleDic[michiNoEkiInfo.ID];
                }
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
                    UpdatePin(e.MichiNoEkiInfo);
                    break;

                // 追加
                case UpdateKind.Add:
                    AddPin(e.MichiNoEkiInfo);
                    break;

                // 削除
                case UpdateKind.Delete:
                    DeletePin(e.MichiNoEkiInfo);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 表示状態ロードイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void OnAllUpdateVisibleEvent(object? sender, AllUpdateVisibleEventArgs e)
        {
            // ピンの表示状態セット
            foreach (var item in e.VisibleDic)
            {
                if (Pins.ContainsKey(item.Key) == true)
                {
                    Pins[item.Key].Visibility.Value = item.Value;
                }
                else
                {
                    _tempVisibleDic.Add(item.Key, item.Value);
                }
            }
        }

        /// <summary>
        /// 表示状態ロードイベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private void OnUpdateVisibleEvent(object? sender, UpdateVisibleEventArgs e)
        {
            // ピンの表示状態更新
            Pins[e.ID].Visibility.Value = e.Visible;
        }

        /// <summary>
        /// ピン更新(道の駅データ更新)
        /// </summary>
        /// <param name="info">道の駅データ</param>
        private void UpdatePin(MichiNoEkiInfo info)
        {
            Pins[info.ID].UpdateFromMichiNoEkiInfo(info);
        }

        /// <summary>
        /// ピン追加
        /// </summary>
        /// <param name="info">道の駅データ</param>
        private void AddPin(MichiNoEkiInfo info)
        {
            Pins.Add(info.ID, new MichiNoEkiPin(info));
        }

        /// <summary>
        /// ピン削除
        /// </summary>
        /// <param name="info">道の駅データ</param>
        private void DeletePin(MichiNoEkiInfo info)
        {
            Pins.Remove(info.ID);
        }
    }
}
