using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Thing.Designer.Infrastructure.Local.Models;
using Thing.Domain.CustomerAggregate;

namespace Thing.Designer.Infrastructure.Local.Data.Config;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers").HasKey(x => x.Id);
        builder.HasOne<ApplicationUser>().WithMany().HasForeignKey(c => c.ApplicationUserId);
        builder.HasIndex(x => x.ApplicationUserId).IsUnique();
    }
}
