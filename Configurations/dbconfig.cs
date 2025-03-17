namespace TimeFourthe.Configurations {
    public class MongoDbSettings {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
        public List<string>? CollectionName { get; set; }
    }
}
