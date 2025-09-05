public class paylasimlar
{ 
public int ID { get; set; }

public string PUrl { get; set; } = string.Empty;

public string Gonderi { get; set; } = string.Empty;

public int KullaniciID { get; set; }

public int? KategoriID { get; set; }

public DateTime GonderiTarihi { get; set; }
}