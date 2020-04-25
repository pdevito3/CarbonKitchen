namespace CarbonKitchen.Ingredients.Api.Data.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("Ingredients")]
    public class Ingredient
    {
        [Key]
        [Required]
        [Column("IngredientId")]
        [Sieve(CanFilter = true, CanSort = true)]
        public int IngredientId { get; set; }

        [Column("IngredientIntField1")]
        [Sieve(CanFilter = true, CanSort = true)]
        public int? IngredientIntField1 { get; set; }

        [Column("IngredientTextField1")]
        [Sieve(CanFilter = true, CanSort = true)]
        public string IngredientTextField1 { get; set; }

        [Column("IngredientTextField2")]
        [Sieve(CanFilter = true, CanSort = true)]
        public string IngredientTextField2 { get; set; }

        [Column("IngredientDateField1")]
        public DateTime? IngredientDateField1 { get; set; }
    }
}
