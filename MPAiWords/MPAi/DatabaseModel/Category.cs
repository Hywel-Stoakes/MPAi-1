namespace MPAi.DatabaseModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    /// <summary>
    /// Entity class for mapping the category of a particular recording to a database.
    /// </summary>
    [Table("Category")]
    public partial class Category
    {
        /// <summary>
        /// Unique identifier for each category.
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        /// <summary>
        /// Unique name for each category.
        /// </summary>
        [Required]
        [StringLength(64)]
        [Index(IsUnique = true)]
        public string Name { get; set; }
        /// <summary>
        /// ONE-TO-MANY RELATIONSHIP
        /// Category-Word
        /// 
        /// A collection of the words in each category.
        /// </summary>
        public virtual ICollection<Word> Words { get; set; }
    }
}
