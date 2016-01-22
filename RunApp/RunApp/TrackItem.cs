namespace RunApp
{
    public class TrackItem
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string name { get; set; }
        public string value { get; set; }

        public TrackItem()
        {

        }

        public TrackItem(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}