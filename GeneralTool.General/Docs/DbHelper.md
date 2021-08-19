# 使用方法
>该项目中有两个DbHelper类型,其中DbManager会自动管理连接并释放(在每一次执行方法完成后会自动释放连接)
而DbContext则需要手动调用对象的Dispose方法释放
推荐使用DbManager

使用sqlite示例
## 1.DbManager (推荐使用)

```
 var Db = new DbManager($"Data Source={DbFile};Version=3;", SQLiteFactory.Instance);
 
```