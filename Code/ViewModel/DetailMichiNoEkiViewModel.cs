using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RoadsideStationApp
{
    /// <summary>
    /// 詳細ページViewModel
    /// </summary>
    public class DetailMichiNoEkiViewModel
    {
        /// <summary>
        /// 道の駅データ管理Model
        /// </summary>
        private MichiNoEkiDataModel _michiNoEkiDataModel;

        /// <summary>
        /// 表示中の道の駅のID
        /// </summary>
        private int _id;

        /// <summary>
        /// 表示処理中
        /// </summary>
        private bool _isOpening = false;

        /// <summary>
        /// 名称
        /// </summary>
        public AutoNotifyProperty<string> Name { get; set; } = new AutoNotifyProperty<string>(string.Empty);

        /// <summary>
        /// 住所
        /// </summary>
        public AutoNotifyProperty<string> Address { get; set; } = new AutoNotifyProperty<string>(string.Empty);

        /// <summary>
        /// 営業時間リスト
        /// </summary>
        public List<DetailPageViewBusinessHours> BusinessHoursList { get; set; } = new List<DetailPageViewBusinessHours>();

        /// <summary>
        /// 定休日リスト
        /// </summary>
        public List<DetailPageViewRegularHoliday> RegularHolidayList { get; set; } = new List<DetailPageViewRegularHoliday>();

        /// <summary>
        /// スタンプ24時間押下OK
        /// </summary>
        public AutoNotifyProperty<bool> StampAllTimeOK { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// オープン済み
        /// </summary>
        public AutoNotifyProperty<bool> IsOpened { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// 訪問済み
        /// </summary>
        public AutoNotifyProperty<bool> IsVisited { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// 訪問日
        /// </summary>
        public DetailPageViewVisitedDate VisitedDate { get; set; } = new DetailPageViewVisitedDate();

        /// <summary>
        /// 注意事項あり
        /// </summary>
        public AutoNotifyProperty<bool> Notice { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// 備考
        /// </summary>
        public AutoNotifyProperty<string> Comment { get; set; } = new AutoNotifyProperty<string>(string.Empty);

        /// <summary>
        /// 編集モードVisivility
        /// </summary>
        public AutoNotifyProperty<bool> EditMode { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// 閲覧モードand訪問済みVisivility
        /// </summary>
        public AutoNotifyProperty<bool> NotEditModeAndVisitedVisible { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// 編集モードand訪問済みVisivility
        /// </summary>
        public AutoNotifyProperty<bool> EditModeAndVisitedVisible { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// 編集モードorスタンプ24時間押下OKVisivility
        /// </summary>
        public AutoNotifyProperty<bool> EditModeOrStampAllTimeOKVisible { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// 編集モードor未オープンVisivility
        /// </summary>
        public AutoNotifyProperty<bool> EditModeOrNotopenVisible { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// 編集モードor注意事項ありVisivility
        /// </summary>
        public AutoNotifyProperty<bool> EditModeOrNoticeVisible { get; set; } = new AutoNotifyProperty<bool>();

        /// <summary>
        /// 編集モードコマンド
        /// </summary>
        public DelegateCommand EditCommand { get; set; }

        /// <summary>
        /// 保存コマンド
        /// </summary>
        public DelegateCommand SaveCommand { get; set; }

        /// <summary>
        /// 保存キャンセルコマンド
        /// </summary>
        public DelegateCommand SaveCancelCommand { get; set; }

        /// <summary>
        /// 非表示コマンド
        /// </summary>
        public DelegateCommand CloseCommand { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DetailMichiNoEkiViewModel()
        {
            // 各データが編集されたときの処理の登録
            IsVisited.PropertyChanged += (s, e) =>
            {
                VisitedButtonClicked();
            };
            StampAllTimeOK.PropertyChanged += (s, e) =>
            {
                EditModeOrStampAllTimeOKVisible.Value = StampAllTimeOK.Value | EditMode.Value;
            };
            IsOpened.PropertyChanged += (s, e) =>
            {
                EditModeOrNotopenVisible.Value = !IsOpened.Value | EditMode.Value;
            };
            Notice.PropertyChanged += (s, e) =>
            {
                EditModeOrNoticeVisible.Value = Notice.Value | EditMode.Value;
            };
            EditMode.PropertyChanged += (s, e) =>
            {
                NotEditModeAndVisitedVisible.Value = IsVisited.Value & !EditMode.Value;
                EditModeAndVisitedVisible.Value = IsVisited.Value & EditMode.Value;
                EditModeOrStampAllTimeOKVisible.Value = StampAllTimeOK.Value | EditMode.Value;
                EditModeOrNotopenVisible.Value = !IsOpened.Value | EditMode.Value;
                EditModeOrNoticeVisible.Value = Notice.Value | EditMode.Value;
            };

            // 各ボタンが押下されたときの処理の登録
            EditCommand = new DelegateCommand(OnEditCommand);
            SaveCommand = new DelegateCommand(OnSaveCommand);
            SaveCancelCommand = new DelegateCommand(OnSaveCancelCommand);
            CloseCommand = new DelegateCommand(OnCloseCommand);

            // 道の駅データ管理Modelのインスタンス取得
            _michiNoEkiDataModel = App.Instance.MichiNoEkiDataModel;

            _michiNoEkiDataModel.ShowDetailMichiNoEkiInfoEvet += OnShowDetailMichiNoEkiInfoEvet;

            // 12ヶ月分初期化
            for (int i = 1; i <= 12; i++)
            {
                BusinessHoursList.Add(new DetailPageViewBusinessHours(i));
            }

            // 7曜分初期化
            for (int i = 1; i <= 7; i++)
            {
                RegularHolidayList.Add(new DetailPageViewRegularHoliday(i));
            }
        }

        /// <summary>
        /// 道の駅データ詳細表示イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private async void OnShowDetailMichiNoEkiInfoEvet(object? sender, ShowDetailMichiNoEkiInfoEvetArgs e)
        {
            // 表示処理中
            _isOpening = true;

            // 閲覧モード
            EditMode.Value = false;

            // 表示データ更新
            UpdateDispMichiNoEkiInfo(e.MichiNoEkiInfo);

            // 詳細画面を表示
            await Application.Current!.MainPage!.Navigation.PushAsync(App.Instance.DetailPage);

            // 表示処理終了
            _isOpening = false;
        }

        /// <summary>
        /// 編集モードコマンド ハンドラ
        /// </summary>
        /// <param name="param"></param>
        private void OnEditCommand(object? param)
        {
            // 編集モード
            EditMode.Value = true;
        }

        /// <summary>
        /// 保存コマンド ハンドラ
        /// </summary>
        /// <param name="param"></param>
        private void OnSaveCommand(object? param)
        {
            // 編集データ保存
            MichiNoEkiInfo? info = SetMichiNoEkiInfo();
            if (info != null)
            {
                _ = _michiNoEkiDataModel.UpadateMichiNoEkiInfo(info);
            }

            // 閲覧モード
            EditMode.Value = false;
        }

        /// <summary>
        /// 保存キャンセルコマンド ハンドラ
        /// </summary>
        /// <param name="param"></param>
        private void OnSaveCancelCommand(object? param)
        {
            // 表示データを最新に戻す
            MichiNoEkiInfo info = _michiNoEkiDataModel.GetMichiNoEkiInfo(_id)!;
            UpdateDispMichiNoEkiInfo(info);

            // 閲覧モード
            EditMode.Value = false;
        }

        /// <summary>
        /// 非表示コマンド ハンドラ
        /// </summary>
        /// <param name="param"></param>
        private void OnCloseCommand(object? param)
        {
            // 表示データを最新に戻す
            MichiNoEkiInfo info = _michiNoEkiDataModel.GetMichiNoEkiInfo(_id)!;
            UpdateDispMichiNoEkiInfo(info);

            // 閲覧モード
            EditMode.Value = false;
        }

        /// <summary>
        /// 訪問ボタンクリック時処理
        /// </summary>
        private async void VisitedButtonClicked()
        {
            if (_isOpening == false)
            {
                // 訪問済み→未訪問
                if (IsVisited.Value == false)
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
                        IsVisited.Value = true;
                        return;
                    }

                    // 訪問日を削除する
                    VisitedDate.SetDateTime(null);
                }

                // 未訪問→訪問済み
                else
                {
                    // 訪問日に本日の日付を入れる
                    if (VisitedDate.DateTime.Value == null)
                    {
                        VisitedDate.SetDateTime(DateTime.Now.Date);
                    }
                }

                // Visivility更新
                EditModeAndVisitedVisible.Value = IsVisited.Value & EditMode.Value;
                NotEditModeAndVisitedVisible.Value = IsVisited.Value & !EditMode.Value;

                // 閲覧モードの場合は保存する
                if (EditMode.Value == false)
                {
                    MichiNoEkiInfo? info = SetMichiNoEkiInfo();
                    if (info != null)
                    {
                        _ = _michiNoEkiDataModel.UpadateMichiNoEkiInfo(info);
                    }
                }
            }
        }

        /// <summary>
        /// 表示中の道の駅データを更新
        /// </summary>
        /// <param name="info">道の駅データ</param>
        private void UpdateDispMichiNoEkiInfo(MichiNoEkiInfo info)
        {
            _id = info.ID;
            Name.Value = info.Name;
            Address.Value = info.Address;
            StampAllTimeOK.Value = info.StampAllTimeOK;
            IsOpened.Value = info.IsOpened;
            IsVisited.Value = info.IsVisited;
            VisitedDate.SetDateTime(info.VisitedDate);
            Notice.Value = info.Notice;
            Comment.Value = info.Comment;

            for (int i = 0; i < 12; i++)
            {
                BusinessHoursList[i].SetOpenTime(info.OpenTimeList[i]);
                BusinessHoursList[i].SetCloseTime(info.CloseTimeList[i]);
            }

            for (int i = 0; i < 7; i++)
            {
                RegularHolidayList[i].Close.Value = info.CloseDayList[i] ?? false;
            }

            EditModeAndVisitedVisible.Value = IsVisited.Value & EditMode.Value;
            NotEditModeAndVisitedVisible.Value = IsVisited.Value & !EditMode.Value;
            EditModeOrNotopenVisible.Value = !IsOpened.Value | EditMode.Value;
            EditModeOrStampAllTimeOKVisible.Value = StampAllTimeOK.Value | EditMode.Value;
            EditModeOrNoticeVisible.Value = Notice.Value | EditMode.Value;
        }

        /// <summary>
        /// 表示中データから道の駅データを生成
        /// </summary>
        private MichiNoEkiInfo? SetMichiNoEkiInfo()
        {
            MichiNoEkiInfo? info = _michiNoEkiDataModel.GetMichiNoEkiInfo(_id);

            if (info != null)
            {
                info.Name = Name.Value;
                info.StampAllTimeOK = StampAllTimeOK.Value;
                info.IsOpened = IsOpened.Value;
                info.IsVisited = IsVisited.Value;
                info.VisitedDate = VisitedDate.DateTime.Value;
                info.Notice = Notice.Value;
                info.Comment = Comment.Value;

                for (int i = 0; i < 12; i++)
                {
                    info.OpenTimeList[i] = BusinessHoursList[i].OpenTime.Value == TimeSpan.Zero ? null : BusinessHoursList[i].OpenTime.Value;
                    info.CloseTimeList[i] = BusinessHoursList[i].CloseTime.Value == TimeSpan.Zero ? null : BusinessHoursList[i].CloseTime.Value;
                }

                for (int i = 0; i < 7; i++)
                {
                    info.CloseDayList[i] = RegularHolidayList[i].Close.Value;
                }
            }

            return info;
        }
    }
}
