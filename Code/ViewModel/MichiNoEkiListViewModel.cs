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
        public List<string> RegionNameList { get; set; } = new List<string>()
        {
            "北海道地方","東北地方","関東地方","北陸地方","中部地方","近畿地方","中国地方","四国地方","九州地方"
        };

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

            // コマンドハンドラ登録
            SelectRegionCommand = new DelegateCommand(OnSelectRegionCommand);
            SelectPrefectureCommand = new DelegateCommand(OnSelectPrefectureCommand);
            SelectMichiNoEkiCommand = new DelegateCommand(OnSelectMichiNoEkiCommand);
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
            foreach (var item in infoList)
            {
                if (item.Prefecture == SelectedPrefecture.Value)
                {
                    MichiNoEkiNameList.Add(new MichiNoEkiListViewMichiNoEkiNameList(item.ID, item.Name));
                }
            }
        }

        /// <summary>
        /// 地方選択コマンドハンドラ
        /// </summary>
        /// <param name="param">コマンド引数</param>
        private void OnSelectRegionCommand(object? param)
        {
            switch (SelectedRegion.Value)
            {
                case "北海道地方":
                    PrefectureNameList.Clear();
                    PrefectureNameList.Add("北海道");
                    break;
                case "東北地方":
                    PrefectureNameList.Clear();
                    PrefectureNameList.Add("青森県");
                    PrefectureNameList.Add("岩手県");
                    PrefectureNameList.Add("宮城県");
                    PrefectureNameList.Add("秋田県");
                    PrefectureNameList.Add("山形県");
                    PrefectureNameList.Add("福島県");
                    break;
                case "関東地方":
                    PrefectureNameList.Clear();
                    PrefectureNameList.Add("茨城県");
                    PrefectureNameList.Add("栃木県");
                    PrefectureNameList.Add("群馬県");
                    PrefectureNameList.Add("埼玉県");
                    PrefectureNameList.Add("千葉県");
                    PrefectureNameList.Add("東京都");
                    PrefectureNameList.Add("神奈川県");
                    PrefectureNameList.Add("山梨県");
                    break;
                case "北陸地方":
                    PrefectureNameList.Clear();
                    PrefectureNameList.Add("新潟県");
                    PrefectureNameList.Add("富山県");
                    PrefectureNameList.Add("石川県");
                    break;
                case "中部地方":
                    PrefectureNameList.Clear();
                    PrefectureNameList.Add("長野県");
                    PrefectureNameList.Add("岐阜県");
                    PrefectureNameList.Add("静岡県");
                    PrefectureNameList.Add("愛知県");
                    PrefectureNameList.Add("三重県");
                    break;
                case "近畿地方":
                    PrefectureNameList.Clear();
                    PrefectureNameList.Add("福井県");
                    PrefectureNameList.Add("滋賀県");
                    PrefectureNameList.Add("京都府");
                    PrefectureNameList.Add("大阪府");
                    PrefectureNameList.Add("兵庫県");
                    PrefectureNameList.Add("奈良県");
                    PrefectureNameList.Add("和歌山県");
                    break;
                case "中国地方":
                    PrefectureNameList.Clear();
                    PrefectureNameList.Add("鳥取県");
                    PrefectureNameList.Add("島根県");
                    PrefectureNameList.Add("岡山県");
                    PrefectureNameList.Add("広島県");
                    PrefectureNameList.Add("山口県");
                    break;
                case "四国地方":
                    PrefectureNameList.Clear();
                    PrefectureNameList.Add("徳島県");
                    PrefectureNameList.Add("香川県");
                    PrefectureNameList.Add("愛媛県");
                    PrefectureNameList.Add("高知県");
                    break;
                case "九州地方":
                    PrefectureNameList.Clear();
                    PrefectureNameList.Add("福岡県");
                    PrefectureNameList.Add("佐賀県");
                    PrefectureNameList.Add("長崎県");
                    PrefectureNameList.Add("熊本県");
                    PrefectureNameList.Add("大分県");
                    PrefectureNameList.Add("宮崎県");
                    PrefectureNameList.Add("鹿児島県");
                    PrefectureNameList.Add("沖縄県");
                    break;
                default:
                    break;
            }
        }
    }
}
