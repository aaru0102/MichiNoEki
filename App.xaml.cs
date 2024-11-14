using System.IO;

namespace RoadsideStationApp
{
    public partial class App : Application
    {
        private readonly string defaultDbPath;
        private readonly string dbPath;
        private static App? instance;

        /// <summary>
        /// 道の駅データ管理Model
        /// </summary>
        public MichiNoEkiDataModel MichiNoEkiDataModel { get; private set; }

        /// <summary>
        /// 詳細ページView
        /// </summary>
        public DetailPage DetailPage { get; private set; }

        /// <summary>
        /// インスタンス
        /// </summary>
        public static App Instance
        {
            get
            {
                instance ??= new App();
                return instance;
            }
            private set
            {
                instance ??= value;
            }
        }

        /// <summary>
        /// データベース
        /// </summary>
        public DatabaseAccess DatabaseAccess { get; private set; }

        /// <summary>
        /// データベースロード完了イベント
        /// </summary>
        public EventHandler? DatabaseLoaded { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。'required' 修飾子を追加するか、Null 許容として宣言することを検討してください。
        public App()
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。'required' 修飾子を追加するか、Null 許容として宣言することを検討してください。
        {
            Instance = this;

            InitializeComponent();

            // 道の駅データ管理Model生成
            MichiNoEkiDataModel = new MichiNoEkiDataModel();

            // 詳細ページView生成
            DetailPage = new DetailPage();

            // デフォルトのデータベースパス
            defaultDbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "defaultmichinoeki.db");

            // アクセスするデータベースパス
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "michinoeki.db");

            // アプリ起動時にデータベースをインポート
            _ = LoadDatabaseFileAsync(); // 非同期メソッドを呼び出す

            // 画面読み込み
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // アプリがアクティブになるとき
        }

        protected override void OnSleep()
        {
            // アプリがバックグラウンドに入るとき
        }

        protected override void OnResume()
        {
            // アプリがフォアグラウンドに戻るとき
        }

        /// <summary>
        /// データベースファイルロード
        /// </summary>
        /// <returns>Task</returns>
        private async Task LoadDatabaseFileAsync()
        {
            // アクセス用データベースファイルが存在しない場合(初回起動時)はデフォルトデータベースをコピーする
            if (!File.Exists(dbPath))
            {
                // デフォルトデータベースファイルを参照可能なパスへコピー
                using var stream = await FileSystem.OpenAppPackageFileAsync("defaultmichinoeki.db");
                using var fs = File.OpenWrite(defaultDbPath);
                FileCopy(stream, fs);

                // コピーしたデフォルトデータベースファイルをアクセスするデータベースファイルにコピー
                using var fs1 = File.OpenRead(defaultDbPath);
                using var fs2 = File.OpenWrite(dbPath);
                FileCopy(fs1, fs2);
            }

            // データベース接続の初期化
            DatabaseAccess = new DatabaseAccess(dbPath);

            // データベースロードイベント
            DatabaseLoaded?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Streamからファイルコピー
        /// </summary>
        /// <param name="source">コピー元</param>
        /// <param name="target">コピー先</param>
        private void FileCopy(Stream source, Stream target)
        {
            using (var reader = new BinaryReader(source))
            using (var writer = new BinaryWriter(target))
            {
                while (true)
                {
                    var data = reader.ReadBytes(1024 * 1024);
                    writer.Write(data);
                    if (data.Length < 1024 * 1024) break;
                }
            }
        }
    }
}
