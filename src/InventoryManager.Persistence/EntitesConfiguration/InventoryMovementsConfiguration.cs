using InventoryManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace InventoryManager.Persistence.EntitesConfiguration
{
    public class InventoryMovementsConfiguration : IEntityTypeConfiguration<InventoryMovement>
    {
        public void Configure(EntityTypeBuilder<InventoryMovement> builder)
        {
            builder.HasKey(im => im.Id);
            builder.Property(im => im.Type).IsRequired().HasMaxLength(50);
            builder.Property(im => im.Reason).HasMaxLength(200);
            builder.HasOne<Product>().WithMany().HasForeignKey(im => im.ProductId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
