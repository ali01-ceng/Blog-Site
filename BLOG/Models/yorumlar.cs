public class yorumlar
{
  
public int ID { get; set; }

public string Yorum { get; set; } = string.Empty;
    
public int GonderiID { get; set; }

public int KullaniciID { get; set; }

public DateTime YorumTarihi { get; set; }

}
