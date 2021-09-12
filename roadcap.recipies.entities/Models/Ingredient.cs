﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace roadcap.recipes.entities.Models
{
    [Table("ingredient")]
    public partial class Ingredient
    {
        [Key]
        [Column("ingredient_id")]
        public int IngredientId { get; set; }

        [Column("recipe_id")]
        public int RecipeId { get; set; }

        [Required]
        [Column("ingredient_name")]
        [DisplayName("Ingredient")]
        [StringLength(75)]
        public string IngredientName { get; set; }

        [Column("amount", TypeName = "decimal(6, 2)")]
        public decimal Amount { get; set; }

        [Column("units")]
        [StringLength(15)]
        public string Units { get; set; }

        [Column("special_instructions")]
        [StringLength(255)]
        [DisplayName("Special Instructions")]
        public string SpecialInstructions { get; set; }

        [ForeignKey(nameof(RecipeId))]
        [InverseProperty("Ingredients")]
        public virtual Recipe Recipe { get; set; }
    }
}