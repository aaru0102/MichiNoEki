using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoadsideStationApp
{
    /// <summary>
    /// 道の駅データ管理Model
    /// </summary>
    public class MichiNoEkiDataModel
    {
        private App _app;

        /// <summary>
        /// 道の駅データリスト
        /// </summary>
        private Dictionary<int, MichiNoEkiInfo> _michiNoEkiInfoDic = new Dictionary<int, MichiNoEkiInfo>();

        /// <summary>
        /// 道の駅データロード完了イベント
        /// </summary>
        public EventHandler<LoadedMichiNoEkiInfoEventArgs>? LoadedMichiNoEkiInfoEvent { get; set; }

        /// <summary>
        /// 道の駅データ更新イベント
        /// </summary>
        public EventHandler<UpdateMichiNoEkiInfoEventArgs>? UpdateMichiNoEkiInfoEvent { get; set; }

        /// <summary>
        /// 道の駅データ詳細表示イベント
        /// </summary>
        public EventHandler<ShowDetailMichiNoEkiInfoEvetArgs>? ShowDetailMichiNoEkiInfoEvet { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MichiNoEkiDataModel()
        {
            // Appのインスタンス取得
            _app = App.Instance;

            // データベースファイルロード完了時処理を登録
            _app.DatabaseLoaded += OnDatabaseLoaded;
        }

        /// <summary>
        /// 道の駅データの取得(全件)
        /// </summary>
        public List<MichiNoEkiInfo> GetAllMichiNoEkiInfo()
        {
            List<MichiNoEkiInfo> infoList = new List<MichiNoEkiInfo>();
            foreach (var info in _michiNoEkiInfoDic.Values)
            {
                infoList.Add(info.DeepCopy());
            }

            return infoList;
        }

        /// <summary>
        /// 道の駅データの取得
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>道の駅データ(見つからなかった場合null)</returns>
        public MichiNoEkiInfo? GetMichiNoEkiInfo(int id)
        {
            _michiNoEkiInfoDic.TryGetValue(id, out MichiNoEkiInfo? info);
            return info?.DeepCopy();
        }

        /// <summary>
        /// 道の駅データの更新
        /// </summary>
        /// <param name="info">対象データ</param>
        /// <returns>結果</returns>
        public async Task<bool> UpadateMichiNoEkiInfo(MichiNoEkiInfo info)
        {
            // データベースに反映
            bool result = await _app.DatabaseAccess.UpdateAsync(ConvertToTable(info));

            // 保持データに反映して更新イベントを発火する
            if (result == true)
            {
                _michiNoEkiInfoDic[info.ID] = info;
                UpdateMichiNoEkiInfoEvent?.Invoke(this, new UpdateMichiNoEkiInfoEventArgs(info, UpdateKind.Update));
            }

            return result;
        }

        /// <summary>
        /// 道の駅データの追加
        /// </summary>
        /// <param name="info">対象データ</param>
        /// <returns>結果</returns>
        public async Task<bool> AddMichiNoEkiInfo(MichiNoEkiInfo info)
        {
            // データベースに反映
            bool result = await _app.DatabaseAccess.AddAsync(ConvertToTable(info));

            // 保持データに反映して更新イベントを発火する
            if (result == true)
            {
                _michiNoEkiInfoDic.Add(info.ID, info.DeepCopy());
                UpdateMichiNoEkiInfoEvent?.Invoke(this, new UpdateMichiNoEkiInfoEventArgs(info, UpdateKind.Add));
            }

            return result;
        }

        /// <summary>
        /// 道の駅データの削除
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>結果</returns>
        public async Task<bool> DeleteMichiNoEkiInfo(int id)
        {
            // データベースに反映
            bool result = await _app.DatabaseAccess.DeleteByIdAsync<MichiNoEkiInfo>(id);

            // 保持データに反映して更新イベントを発火する
            if (result == true)
            {
                MichiNoEkiInfo deleteInfo = _michiNoEkiInfoDic[id];
                _michiNoEkiInfoDic.Remove(id);
                UpdateMichiNoEkiInfoEvent?.Invoke(this, new UpdateMichiNoEkiInfoEventArgs(deleteInfo, UpdateKind.Delete));
            }

            return result;
        }

        /// <summary>
        /// 道の駅データ詳細表示
        /// </summary>
        /// <param name="id">ID</param>
        public void ShowDetailMichiNoEkiInfo(int id)
        {
            // 道の駅データ取得
            MichiNoEkiInfo? info = GetMichiNoEkiInfo(id);

            // 見つからなかった場合処理なし
            if (info == null)
            {
                return;
            }

            // 道の駅データ詳細表示イベント発火
            ShowDetailMichiNoEkiInfoEvet?.Invoke(this, new ShowDetailMichiNoEkiInfoEvetArgs(info));
        }

        /// <summary>
        /// データベースファイルロード完了時処理
        /// </summary>
        /// <param name="sender">イベント送信元</param>
        /// <param name="e">イベント引数</param>
        private async void OnDatabaseLoaded(object? sender, EventArgs e)
        {
            // データベースから道の駅データ全件取得
            List<MichiNoEkiInfoTable> tableList = await _app.DatabaseAccess.GetAllAsync<MichiNoEkiInfoTable>();

            // 道の駅データリストに展開
            SetAllMichiNoEkiInfoFromDatabase(tableList);

            // 道の駅データロード完了イベント発火
            LoadedMichiNoEkiInfoEvent?.Invoke(this, new LoadedMichiNoEkiInfoEventArgs(GetMichiNoEkiInfoCopyList()));
        }

        /// <summary>
        /// ディクショナリーのデータをコピーしてリストにする
        /// </summary>
        /// <returns>道の駅データリスト</returns>
        private List<MichiNoEkiInfo> GetMichiNoEkiInfoCopyList()
        {
            List<MichiNoEkiInfo> infoList = new List<MichiNoEkiInfo>();
            foreach (var info in _michiNoEkiInfoDic.Values)
            {
                infoList.Add(info.DeepCopy());
            }

            return infoList;
        }

        /// <summary>
        /// データベースから取得したデータを道の駅データリストに展開
        /// </summary>
        /// <param name="tableList">データベースから取得したデータ</param>
        private void SetAllMichiNoEkiInfoFromDatabase(List<MichiNoEkiInfoTable> tableList)
        {
            _michiNoEkiInfoDic.Clear();
            foreach (var item in tableList)
            {
                _michiNoEkiInfoDic.Add(item.ID, ConvertToData(item));
            }
        }

        /// <summary>
        /// テーブルからデータに変換
        /// </summary>
        /// <param name="table">テーブル</param>
        /// <returns>データ</returns>
        private static MichiNoEkiInfo ConvertToData(MichiNoEkiInfoTable table)
        {
            return new MichiNoEkiInfo()
            {
                ID = table.ID,
                Name = table.Name,
                Latitude = table.Latitude,
                Longitude = table.Longitude,
                Address = table.Address,
                Region = table.Region,
                Prefecture = table.Prefecture,
                OpenTimeList = ConvertSerializedTimesToList(table.OpenTimeListSerialized),
                CloseTimeList = ConvertSerializedTimesToList(table.CloseTimeListSerialized),
                CloseDayList = ConvertSerializedBoolsToList(table.CloseDayListSerialized),
                StampAllTimeOK = table.StampAllTimeOK,
                IsOpened = table.IsOpened,
                IsVisited = table.IsVisited,
                VisitedDate = table.VisitedDate,
                Notice = table.Notice,
                Comment = table.Comment,
            };
        }

        /// <summary>
        /// テーブルからデータに変換
        /// </summary>
        /// <param name="info">データ</param>
        /// <returns>テーブル</returns>
        private static MichiNoEkiInfoTable ConvertToTable(MichiNoEkiInfo info)
        {
            return new MichiNoEkiInfoTable()
            {
                ID = info.ID,
                Name = info.Name,
                Latitude = info.Latitude,
                Longitude = info.Longitude,
                Address = info.Address,
                Region = info.Region,
                Prefecture = info.Prefecture,
                OpenTimeListSerialized = ConvertTimeListToSerializedString(info.OpenTimeList),
                CloseTimeListSerialized = ConvertTimeListToSerializedString(info.CloseTimeList),
                CloseDayListSerialized = ConvertBoolListToSerializedString(info.CloseDayList),
                StampAllTimeOK = info.StampAllTimeOK,
                IsOpened = info.IsOpened,
                IsVisited = info.IsVisited,
                VisitedDate = info.VisitedDate,
                Notice = info.Notice,
                Comment = info.Comment,
            };
        }

        /// <summary>
        /// 12ヶ月分の時間の文字列からTimeSpanリストに変換するメソッド
        /// </summary>
        /// <param name="serializedTimes">シリアライズされた12ヶ月分の時間の文字列</param>
        /// <returns>12ヶ月分のTimeSpanリスト</returns>
        private static List<TimeSpan?> ConvertSerializedTimesToList(string? serializedTimes)
        {
            var list = new List<TimeSpan?>();

            if (string.IsNullOrEmpty(serializedTimes) == true)
            {
                for (int i = 0; i < 12; i++)
                {
                    list.Add(null);
                }
            }
            else
            {
                foreach (var item in serializedTimes.Split(','))
                {
                    if (string.IsNullOrEmpty(item) == true)
                    {
                        list.Add(null);
                    }
                    else
                    {
                        list.Add(TimeSpan.Parse(item));
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// TimeSpanリストから12ヶ月分の時間の文字列に変換するメソッド
        /// </summary>
        /// <param name="timeList">12ヶ月分のTimeSpanリスト</param>
        /// <returns>シリアライズされた12ヶ月分の時間の文字列</returns>
        private static string ConvertTimeListToSerializedString(List<TimeSpan?> timeList)
        {
            return string.Join(",", timeList.Select(t => t.HasValue ? t.ToString() : string.Empty));
        }

        /// <summary>
        /// 曜日ごとのboolの文字列からboolリストに変換するメソッド
        /// </summary>
        /// <param name="serializedTimes">シリアライズされた曜日ごとのboolの文字列</param>
        /// <returns>boolリスト</returns>
        private static List<bool?> ConvertSerializedBoolsToList(string? serializedTimes)
        {
            var list = new List<bool?>();

            if (string.IsNullOrEmpty(serializedTimes) == true)
            {
                for (int i = 0; i < 7; i++)
                {
                    list.Add(null);
                }
            }
            else
            {
                foreach (var item in serializedTimes.Split(','))
                {
                    if (string.IsNullOrEmpty(item) == true)
                    {
                        list.Add(null);
                    }
                    else
                    {
                        list.Add(bool.Parse(item));
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// boolリストから曜日ごとのboolの文字列に変換するメソッド
        /// </summary>
        /// <param name="timeList">boolリスト</param>
        /// <returns>シリアライズされた曜日ごとのboolの文字列</returns>
        private static string ConvertBoolListToSerializedString(List<bool?> timeList)
        {
            return string.Join(",", timeList.Select(t => t.HasValue ? t.ToString() : string.Empty));
        }
    }
}
