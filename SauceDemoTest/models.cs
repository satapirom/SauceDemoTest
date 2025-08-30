public class Testdata
{
    public BaseUrl? baseUrl { get; set; }
    public List<User> User { get; set; }
    public List<Product> Product { get; set; }
    public List<Checkout> Checkout { get; set; }
    public List<Prices> prices { get; set; }
}

public class BaseUrl
{
    public string url { get; set; }  // ต้องเป็นตัวเล็กให้ตรง JSON
}

public class User
{
    public string username { get; set; }
    public string password { get; set; }
}

public class Product
{
    public string tshirt { get; set; }
    public string tshirt1 { get; set; }
    public string backpack { get; set; }
    public string flashlight { get; set; }
}

public class Checkout
{
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string postalcode { get; set; }
}

public class Prices
{
    public double itemtotal { get; set; }
    public double tax { get; set; }
    public double total { get; set; }
}
