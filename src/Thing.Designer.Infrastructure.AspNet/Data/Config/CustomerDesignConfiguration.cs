using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thing.Designer.Infrastructure.Local.Models;
using Thing.Domain.CustomerAggregate;
using Thing.Domain.CustomerDesignAggregate;

namespace Thing.Designer.Infrastructure.Local.Data.Config;

public class CustomerDesignConfiguration : IEntityTypeConfiguration<CustomerDesign>
{
    public void Configure(EntityTypeBuilder<CustomerDesign> builder)
    {
        builder.ToTable("CustomerDesigns").HasKey(x => x.Id);
        builder.HasOne<Customer>().WithMany().HasForeignKey(c => c.CustomerId);
    }
}
