namespace Actions.Domain.Entities
{
    public class Phone : Model
    {
        public int? Number { get; set; }
        public int? Area_code { get; set; }
        public string Country_code { get; set; }
    }

}
