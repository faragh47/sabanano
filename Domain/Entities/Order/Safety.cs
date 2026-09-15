namespace CleanArchitecture.Domain.Entities.Orders;
public class Safety:BaseEntity<long>
{
    public bool HasNoSafety { get; set; }
    public bool IsPoisonous { get; set; }
    public bool IsEscapable { get; set; }
    public bool IsFlammable { get; set; }
    public bool IsBadForBreathing { get; set; }
    public bool IsAdsorbBySkin { get; set; }
    public bool IsNanoSize { get; set; }
    public bool IsSickness { get; set; }
    public bool IsExplosive { get; set; }
    public ICollection<OrderAnalyze> OrderAnalyzes { get; set; }
}
    