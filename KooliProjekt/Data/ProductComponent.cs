namespace KooliProjekt.Data
{
    public class ProductComponent
    {
        public int Id { get; set; }
        public string Name { get; set; } // Max length: 100
        public string Unit { get; set; } // Max length: 10
        public float Quantity { get; set; }
        public float UnitPrice { get; set; }
        public float Total { get; set; }
    }
}
