using SQLite;

namespace RoadsideStationApp
{
    public class DatabaseAccess
    {
        private SQLiteAsyncConnection? _database;
        private readonly string _dbPath;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="dbPath">データベースファイルパス</param>
        public DatabaseAccess(string dbPath)
        {
            _dbPath = dbPath;
        }

        /// <summary>
        /// 全レコードを取得
        /// </summary>
        public async Task<List<T>> GetAllAsync<T>() where T : new()
        {
            var database = await GetDatabase();
            return await database.Table<T>().ToListAsync();
        }

        /// <summary>
        /// ID指定レコードを取得
        /// </summary>
        public async Task<T?> GetByIdAsync<T>(int id) where T : new()
        {
            var database = await GetDatabase();
            return await database.FindAsync<T>(id);
        }

        /// <summary>
        /// レコードをアップデート
        /// </summary>
        public async Task<bool> UpdateAsync<T>(T item) where T : new()

        {
            var database = await GetDatabase();
            int updateLineCnt = await database.UpdateAsync(item);
            return updateLineCnt != 0;
        }

        /// <summary>
        /// レコードを追加
        /// </summary>
        public async Task<bool> AddAsync<T>(T item) where T : new()
        {
            var database = await GetDatabase();
            int addLineCnt = await database.InsertAsync(item);
            return addLineCnt != 0;
        }

        /// <summary>
        /// ID指定でレコードを削除
        /// </summary>
        public async Task<bool> DeleteByIdAsync<T>(int id) where T : new()
        {
            var database = await GetDatabase();
            var deleteLineCnt = await database.DeleteAsync<T>(id);
            return deleteLineCnt != 0;
        }

        /// <summary>
        /// データベース取得
        /// </summary>
        /// <returns></returns>
        private async Task<SQLiteAsyncConnection> GetDatabase()
        {
            if (_database == null)
            {
                _database = new SQLiteAsyncConnection(_dbPath);
                await _database.CreateTableAsync<MichiNoEkiInfoTable>();
            }

            return _database;
        }
    }
}
