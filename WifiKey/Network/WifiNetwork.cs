namespace WifiKey.Network
{
    public class WifiNetwork
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public override string ToString() => $"Name: {Name}\r\nPassword: {Password}";
    }
}
