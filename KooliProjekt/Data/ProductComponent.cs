namespace KooliProjekt.Data
{
    public class ProductComponent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Unit { get; set; }
        public float UnitPrice { get; set; }
        public float Quantity { get; set; }
        public float TotalCost => UnitPrice * Quantity; // Calculated property
    }
}
