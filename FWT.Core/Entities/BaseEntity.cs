using System.ComponentModel.DataAnnotations;

namespace FWTL.Core.Entities
{
    public abstract class BaseEntity<TKey>
    {
        [Key]
        public TKey Id { get; set; }
    }
}
